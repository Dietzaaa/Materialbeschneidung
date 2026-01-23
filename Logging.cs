using System.IO;
using System.Reflection;

namespace Materialbeschneidung
{
    public static class Logging
    {
        private static readonly string _pfad = "logs\\";
        private static void PersistantAppender(string inhalt)
        {
            Init();
            string pfadTmp = _pfad + "persistantLog.txt";
            if (File.Exists(pfadTmp))
            {
                File.AppendAllText(pfadTmp, inhalt);
            }
            else
            {
                File.WriteAllText(pfadTmp, inhalt);
            }
        }

        private static void RollingAppender(string inhalt)
        {
            Init();
            string pfadTmp = _pfad + "rollingLog.txt";
            if (File.Exists(pfadTmp))
            {
                FileInfo fileInfo = new FileInfo(pfadTmp);
                const long maxFileSize = 5 * 1024 * 1024; // 5 MB
                List<string> lines = File.ReadAllLines(pfadTmp).ToList();

                while (fileInfo.Length > maxFileSize)
                {
                    lines.RemoveAt(0);
                    File.WriteAllLines(pfadTmp, lines);
                    fileInfo.Refresh();
                }
                File.AppendAllText(pfadTmp, inhalt);
            }
            else
            {
                File.WriteAllText(pfadTmp, inhalt);
            }
        }
        private static string AddInfo(string inhalt, string methode)
        {
            inhalt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + methode + " " + inhalt + "\n";
            return inhalt;
        }

        private static void Init()
        {
            if (!Directory.Exists(_pfad))
            {
                Directory.CreateDirectory(_pfad);
            }
        }

        public static void Debug(string inhalt)
        {
            inhalt = AddInfo(inhalt, MethodBase.GetCurrentMethod().Name);
            RollingAppender(inhalt);
        }
        public static void Info(string inhalt)
        {
            inhalt = AddInfo(inhalt, MethodBase.GetCurrentMethod().Name);
            RollingAppender(inhalt);
        }
        public static void Warning(string inhalt)
        {
            inhalt = AddInfo(inhalt, MethodBase.GetCurrentMethod().Name);
            RollingAppender(inhalt);
            PersistantAppender(inhalt);
        }
        public static void Error(string inhalt)
        {
            inhalt = AddInfo(inhalt, MethodBase.GetCurrentMethod().Name);
            RollingAppender(inhalt);
            PersistantAppender(inhalt);
        }
        public static void Fatal(string inhalt)
        {
            inhalt = AddInfo(inhalt, MethodBase.GetCurrentMethod().Name);
            RollingAppender(inhalt);
            PersistantAppender(inhalt);
        }

        public static void Debug(Exception ex)
        {
            string msg = ExceptionToString(ex);
            Debug(msg);
        }

        public static void Info(Exception ex)
        {
            string msg = ExceptionToString(ex);
            Info(msg);
        }

        public static void Warning(Exception ex)
        {
            string msg = ExceptionToString(ex);
            Warning(msg);
        }

        public static void Error(Exception ex)
        {
            string msg = ExceptionToString(ex);
            Error(msg);
        }

        public static void Fatal(Exception ex)
        {
            string msg = ExceptionToString(ex);
            Fatal(msg);
        }

        private static string ExceptionToString(Exception ex)
        {
            return $"Exception: {ex.Message}\n" +
                   $"Type: {ex.GetType()}\n" +
                   $"StackTrace: {ex.StackTrace}\n" +
                   (ex.InnerException != null
                        ? $"InnerException: {ex.InnerException.Message}\n{ex.InnerException.StackTrace}\n"
                        : string.Empty);
        }
    }
}
