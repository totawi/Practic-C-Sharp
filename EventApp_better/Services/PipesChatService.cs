using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventApp.Services
{
    /// <summary>
    /// Обсуждение мероприятий через Named Pipes.
    /// Один экземпляр — сервер (IsServer=true), второй — клиент.
    /// Для демонстрации в одном процессе оба запускаются вместе.
    /// </summary>
    public class PipesChatService : IDisposable
    {
        private const string PipeName = "EventAppDiscussion";
        private CancellationTokenSource _cts = new();

        public event Action<string> MessageReceived;

        // ── SERVER (слушатель) ───────────────────────────────────────────────

        public void StartServer()
        {
            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        using var server = new NamedPipeServerStream(
                            PipeName, PipeDirection.In, 1,
                            PipeTransmissionMode.Message, PipeOptions.Asynchronous);

                        await server.WaitForConnectionAsync(_cts.Token);

                        using var reader = new StreamReader(server, Encoding.UTF8);
                        var msg = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(msg))
                            MessageReceived?.Invoke(msg);
                    }
                    catch (OperationCanceledException) { break; }
                    catch { /* reconnect */ await Task.Delay(500); }
                }
            }, _cts.Token);
        }

        // ── CLIENT (отправитель) ─────────────────────────────────────────────

        public async Task SendMessageAsync(string message)
        {
            try
            {
                using var client = new NamedPipeClientStream(
                    ".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous);

                await client.ConnectAsync(2000); // 2s timeout
                using var writer = new StreamWriter(client, Encoding.UTF8) { AutoFlush = true };
                await writer.WriteLineAsync(message);
            }
            catch (Exception ex)
            {
                MessageReceived?.Invoke($"[Ошибка отправки: {ex.Message}]");
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
