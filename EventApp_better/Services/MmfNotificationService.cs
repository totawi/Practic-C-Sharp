using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventApp.Services
{
    /// <summary>
    /// Уведомления об изменениях расписания через Memory-Mapped Files.
    /// Структура блока: [4 байта длина][данные UTF-8][1 байт флаг «прочитано»]
    /// </summary>
    public class MmfNotificationService : IDisposable
    {
        private const string MmfName    = "EventAppNotifications";
        private const string MutexName  = "EventAppNotificationsMutex";
        private const string EventName  = "EventAppNotificationsEvent";
        private const int    MmfSize    = 4096;

        private MemoryMappedFile _mmf;
        private Mutex _mutex;
        private EventWaitHandle _newDataEvent;
        private CancellationTokenSource _cts = new();

        public event Action<string> NotificationReceived;

        public void Initialize()
        {
            _mmf = MemoryMappedFile.CreateOrOpen(MmfName, MmfSize);
            _mutex = new Mutex(false, MutexName);
            _newDataEvent = new EventWaitHandle(false,
                EventResetMode.AutoReset, EventName);

            StartListener();
        }

        /// <summary>Записать уведомление об изменении расписания.</summary>
        public void PublishScheduleChange(string eventName, string changeDescription)
        {
            var message = $"📅 [{DateTime.Now:HH:mm}] «{eventName}»: {changeDescription}";
            WriteNotification(message);
        }

        private void WriteNotification(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            if (bytes.Length + 5 > MmfSize) return;

            _mutex.WaitOne();
            try
            {
                using var accessor = _mmf.CreateViewAccessor(0, MmfSize);
                accessor.Write(0, (int)bytes.Length);
                accessor.WriteArray(4, bytes, 0, bytes.Length);
                accessor.Write(4 + bytes.Length, (byte)0); // не прочитано
            }
            finally
            {
                _mutex.ReleaseMutex();
                _newDataEvent.Set();
            }
        }

        private void StartListener()
        {
            Task.Run(() =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    // Ждём сигнала о новых данных
                    _newDataEvent.WaitOne(500);
                    if (_cts.Token.IsCancellationRequested) break;

                    string msg = ReadPendingNotification();
                    if (msg != null)
                        NotificationReceived?.Invoke(msg);
                }
            }, _cts.Token);
        }

        private string ReadPendingNotification()
        {
            _mutex.WaitOne();
            try
            {
                using var accessor = _mmf.CreateViewAccessor(0, MmfSize);
                var length = accessor.ReadInt32(0);
                if (length <= 0 || length > MmfSize - 5) return null;

                var readFlag = accessor.ReadByte(4 + length);
                if (readFlag == 1) return null; // уже прочитано

                var bytes = new byte[length];
                accessor.ReadArray(4, bytes, 0, length);
                accessor.Write(4 + length, (byte)1); // отметить прочитанным
                return Encoding.UTF8.GetString(bytes);
            }
            finally { _mutex.ReleaseMutex(); }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _mmf?.Dispose();
            _mutex?.Dispose();
            _newDataEvent?.Dispose();
            _cts.Dispose();
        }
    }
}
