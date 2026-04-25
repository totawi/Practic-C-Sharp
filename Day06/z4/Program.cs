using System;

public class DownloadEventArgs : EventArgs
{
    public int Percent { get; set; }
}

class FileDownloader
{
    public event EventHandler<DownloadEventArgs> DownloadProgressChanged;

    public void StartDownload()
    {
        for (int i = 0; i <= 100; i += 50)
        {
            DownloadProgressChanged?.Invoke(this, new DownloadEventArgs { Percent = i });
        }
    }
}

class DownloadMonitor
{
    public void Setup(FileDownloader downloader)
    {
        downloader.DownloadProgressChanged += (s, e) => Console.WriteLine($"Шкала: {e.Percent}%");
        downloader.DownloadProgressChanged += (s, e) => Console.WriteLine($"Лог: Загружено {e.Percent}%");
    }
}

class Program
{
    static void Main()
    {
        FileDownloader loader = new FileDownloader();
        DownloadMonitor monitor = new DownloadMonitor();

        monitor.Setup(loader); 
        loader.StartDownload();
    }
}