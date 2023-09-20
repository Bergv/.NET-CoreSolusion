using System;
using System.IO;

namespace FileWatchConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        string path = @"D:\Data";
        path = AppContext.BaseDirectory;
        MonitorDirectory(path,"task.txt");
        Console.ReadKey();
        Console.WriteLine("按q退出！");
        while (Console.Read() != 'q') ;
    }
    
    private static void MonitorDirectory(string path, string filter)
    {
      FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
      fileSystemWatcher.Path = path;
      fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess 
                                     | NotifyFilters.LastWrite 
      								 | NotifyFilters.FileName 		   
      								 |NotifyFilters.DirectoryName;
       //文件类型，支持通配符，“*.txt”只监视文本文件
      fileSystemWatcher.Filter = filter;    // 监控的文件格式
      fileSystemWatcher.IncludeSubdirectories = true;  // 监控子目录
      fileSystemWatcher.Changed += new FileSystemEventHandler(OnProcess);
      fileSystemWatcher.Created += new FileSystemEventHandler(OnProcess);
      fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);
      fileSystemWatcher.Deleted += new FileSystemEventHandler(OnProcess);
      //表示当前的路径正式开始被监控，一旦监控的路径出现变更，FileSystemWatcher 中的指定事件将会被触发。
      fileSystemWatcher.EnableRaisingEvents = true;
    }
	private static void OnProcess(object source, FileSystemEventArgs e)
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

	private static void OnCreated(object source, FileSystemEventArgs e)
    {
      Console.WriteLine("File created: {0} {1} {2}", e.ChangeType, e.FullPath, e.Name);
    }

    private static void OnChanged(object source, FileSystemEventArgs e)
    {
      Console.WriteLine("File changed: {0} {1} {2}", e.ChangeType, e.FullPath, e.Name);
    }

    private static void OnDeleted(object source, FileSystemEventArgs e)
    {
      Console.WriteLine("File deleted: {0} {1} {2}", e.ChangeType, e.FullPath, e.Name);
    }

	private static void OnRenamed(object source, FileSystemEventArgs e)
    {
      Console.WriteLine("File renamed: {0} {1} {2}", e.ChangeType, e.FullPath, e.Name);
    }
}
