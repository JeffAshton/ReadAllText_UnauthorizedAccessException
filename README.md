FileWatcherReader.exe:

```
System.UnauthorizedAccessException: Access to the path 'C:\test\previous.txt' is denied.
   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.StreamReader..ctor(String path, Encoding encoding, Boolean detectEncodingFromByteOrderMarks, Int32 bufferSize, Boolean checkHost)
   at System.IO.File.InternalReadAllText(String path, Encoding encoding, Boolean checkHost)
   at System.IO.File.ReadAllText(String path)
   at FileWatcherReader.FlagFileReader.ReadAllText(String path) in C:\Users\jashton\source\repos\FileWatcherReader\FileWatcherReader\FileReader.cs:line 15
   at FileWatcherReader.Program.ReadFile(IEnumerable`1 paths) in C:\Users\jashton\source\repos\FileWatcherReader\FileWatcherReader\Program.cs:line 51
```