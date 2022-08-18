using System;
using VSSystem.Extensions.Hosting.Controllers;

namespace VSSystem.Extensions.Hosting.Controllers
{
    public static partial class LoggerExtension
    {

        #region Log Common
        public static void LogInfo(this AController sender, string contents)
        {
            sender?.Logger?.LogInfo(sender.Name, contents, sender.ServiceName);
        }
        public static void LogDebug(this AController sender, string contents)
        {
            sender?.Logger?.LogDebug(sender.Name, contents, sender.ServiceName);
        }
        public static void LogWarning(this AController sender, string contents)
        {
            sender?.Logger?.LogWarning(sender.Name, contents, sender.ServiceName);
        }
        public static void LogError(this AController sender, string contents)
        {
            sender?.Logger?.LogError(sender.Name, contents, sender.ServiceName);
        }
        public static void LogError(this AController sender, Exception ex)
        {
            sender?.Logger?.LogError(sender.Name, ex, sender.ServiceName);
        }

        #endregion

        #region Log with Tag

        public static void LogInfoWithTag(this AController sender, string tagName, string contents)
        {
            sender?.Logger?.LogInfoWithTag(sender.Name, tagName, contents, sender.ServiceName);
        }
        public static void LogDebugWithTag(this AController sender, string tagName, string contents)
        {
            sender?.Logger?.LogDebugWithTag(sender.Name, tagName, contents, sender.ServiceName);
        }
        public static void LogWarningWithTag(this AController sender, string tagName, string contents)
        {
            sender?.Logger?.LogWarningWithTag(sender.Name, tagName, contents, sender.ServiceName);
        }
        public static void LogErrorWithTag(this AController sender, string tagName, string contents)
        {
            sender?.Logger?.LogErrorWithTag(sender.Name, tagName, contents, sender.ServiceName);
        }
        public static void LogErrorWithTag(this AController sender, string tagName, Exception ex)
        {
            sender?.Logger?.LogErrorWithTag(sender.Name, tagName, ex, sender.ServiceName);
        }
        #endregion

    }
}