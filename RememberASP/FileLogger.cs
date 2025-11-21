namespace RememberASP;

public class FileLoggerProvider : ILoggerProvider
{
    private ILogger logger;

    public FileLoggerProvider()
    {
        logger = new FileLogger("app.log");
    }

    public ILogger CreateLogger(string categoryName)
    {
        return logger;
    }

    public void Dispose()
    {
        
    }
}

public class FileLogger : ILogger
{
    private readonly string logFilePath;
    public FileLogger(string logFilePath)
    {
        this.logFilePath = logFilePath;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new Scope(null);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"[{DateTime.Now}][{logLevel}]: \t{formatter.Invoke(state, exception)}\n";

        File.AppendAllTextAsync(logFilePath, message);
    }

    private class Scope : IDisposable
    {
        private readonly Action _onDispose;

        public Scope(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke();
        }
    }
}