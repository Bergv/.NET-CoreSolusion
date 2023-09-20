using System;
using System.IO;
using System.Windows.Forms;
using FileWatchForm.FileChangeMonitors;

namespace FileWatchForm;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        // InitWatch();
        // InitWatch2();
        InitWatch3();
    }

    private void InitWatch2()
    {
        FileChangeMonitor fcm = new FileChangeMonitor(AppContext.BaseDirectory, "task.txt", NotifyFilters.LastWrite | NotifyFilters.CreationTime);
        fcm.Changed += (o,e) => PrintLine(e);
    }
    
    private void InitWatch3()
    {
        FileWatcher watcher = new();
        watcher.Start(AppContext.BaseDirectory, "task.txt");
        watcher.FileChanged += (o,e) => PrintLine(e);
    }
    

    private void InitWatch()
    {
        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        fileSystemWatcher.Path = AppContext.BaseDirectory;
        fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
                                         | NotifyFilters.CreationTime
                                         | NotifyFilters.DirectoryName
                                         | NotifyFilters.FileName
                                         | NotifyFilters.LastAccess
                                         | NotifyFilters.LastWrite
                                         | NotifyFilters.Security
                                         | NotifyFilters.Size;
        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
        //文件类型，支持通配符，“*.txt”只监视文本文件
        fileSystemWatcher.Filter = "task.txt";    // 监控的文件格式
        fileSystemWatcher.IncludeSubdirectories = true;  // 监控子目录
        fileSystemWatcher.Changed += new FileSystemEventHandler(OnProcess);
        fileSystemWatcher.Created += new FileSystemEventHandler(OnProcess);
        fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);
        fileSystemWatcher.Deleted += new FileSystemEventHandler(OnProcess);
        //表示当前的路径正式开始被监控，一旦监控的路径出现变更，FileSystemWatcher 中的指定事件将会被触发。
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        richTextBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff | ") + Environment.NewLine);
        richTextBox1.AppendText(DateTime.Now + Environment.NewLine);
        richTextBox1.AppendText(DateTime.UtcNow + Environment.NewLine);
        richTextBox1.AppendText(DateTime.MinValue + Environment.NewLine);
        richTextBox1.AppendText(Environment.TickCount + Environment.NewLine);
    }
    
    private void OnProcess(object source, FileSystemEventArgs e)
    {
        if (e.ChangeType == WatcherChangeTypes.Created)
        {
            OnCreated(source, e);
        }
        else if (e.ChangeType == WatcherChangeTypes.Changed)
        {
            OnChanged(source, e);
        }
        else if (e.ChangeType == WatcherChangeTypes.Deleted)
        {
            OnDeleted(source, e);
        } 
    }
    
    private void OnCreated(object source, FileSystemEventArgs e)
    {
        PrintLine(e);
    }

    private bool isOnChangedExecuting = false; //不可行
    DateTime lastRead = DateTime.MinValue;
    private void OnChanged(object source, FileSystemEventArgs e)
    {
        // var watch = source as FileSystemWatcher;
        // watch!.EnableRaisingEvents = false;
        // watch.EnableRaisingEvents = true;
        DateTime _lastEventTime = default;
        if (DateTime.Now.Subtract(_lastEventTime).TotalMilliseconds > 100)
        {
            _lastEventTime = DateTime.Now;
            PrintLine(e);
            
        }
    }

    

    private void OnDeleted(object source, FileSystemEventArgs e)
    {
        PrintLine(e);
    }

    private void OnRenamed(object source, FileSystemEventArgs e)
    {
        PrintLine(e);
    }
    
    private void PrintLine(FileSystemEventArgs e)
    {
        var t = $"FileChangeType:{e.ChangeType} Path:{e.FullPath} Name:{e.Name}";
        Console.WriteLine(t);
        AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff | ") + $"FileChangeType:{e.ChangeType}");
    }
    
    private void AppendLine(string str)
    {
        if (richTextBox1.InvokeRequired)
        {
            richTextBox1.Invoke(AppendLine, str);
        }
        else
        {
            richTextBox1.AppendText(str + Environment.NewLine);
        }
    }
    
}

