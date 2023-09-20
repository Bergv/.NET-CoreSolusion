using System;
using System.IO;

namespace FileWatchForm.FileChangeMonitors;

public class FileChangeMonitor
{
    private FileSystemWatcher _fsw;
    DateTime _lastEventTime;

    public event FileSystemEventHandler Changed;

    public FileChangeMonitor(string path, string filter)
    {
        _fsw = new FileSystemWatcher(path, filter);
        _fsw.Changed += _fsw_Changed;
        _fsw.EnableRaisingEvents = true;
        _fsw.IncludeSubdirectories = false;
        _fsw.NotifyFilter = NotifyFilters.LastWrite;
    }
    
    public FileChangeMonitor(string path, string filter, NotifyFilters filters):this(path,filter)
    {
        _fsw.NotifyFilter = filters; //NotifyFilters.LastWrite;
    }

    private void _fsw_Changed(object sender, FileSystemEventArgs e)
    {
        // Fix the FindFirstChangeNotification() double-call
        if (DateTime.Now.Subtract(_lastEventTime).TotalMilliseconds > 100)
        {
            _lastEventTime = DateTime.Now;
            if (this.Changed != null)
                this.Changed(sender, e);  // Bubble the event
        }
    }
}