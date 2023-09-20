using System;
using System.IO;

namespace FileWatchForm.FileChangeMonitors;

public class FileWatcher
{
    private FileSystemWatcher watcher = new FileSystemWatcher();
    private System.Timers.Timer t = new System.Timers.Timer();
    private object _sender;
    private FileSystemEventArgs _e;

    public event FileSystemEventHandler FileChanged; //FileSystemEventHandler

    public FileWatcher()
    {
        t.Elapsed += t_Elapsed;
        t.Interval = 500;
    }

    public void Start(string path, string filter)
    {
        watcher.Path = Path.GetDirectoryName(path);
        watcher.Filter = Path.GetFileName(filter);
        watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
        watcher.EnableRaisingEvents = true;
        watcher.Changed += watcher_Changed;
    }

    private void watcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (!t.Enabled)
        {
            _sender = sender;
            _e = e;
            t.Start();
        }
    }
    
    private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        t.Stop();
        if (FileChanged != null)
            FileChanged(_sender, _e);
    }
}