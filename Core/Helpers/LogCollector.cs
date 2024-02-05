namespace Core.Helpers
{
    public static class LogCollector
    {
        public static async Task WriteAsync(string mess, string logPath, int attemptsCount = 10)
        {
            while (attemptsCount > 0)
                try
                {
                    File.AppendAllText(logPath, $"{mess}\r\n");
                    break;
                }
                catch (Exception)
                {
                    attemptsCount--;
                    await Task.Delay(100);
                    continue;
                }
        }
        public static void Write(string mess, string logPath, int attemptsCount = 10)
        {
            while (attemptsCount > 0)
                try
                {
                    File.AppendAllText(logPath, $"{mess}\r\n");
                    break;
                }
                catch (Exception)
                {
                    attemptsCount--;
                    Thread.Sleep(100);
                    continue;
                }
        }
    }
}
