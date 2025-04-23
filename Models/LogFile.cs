using LogStandartization.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LogStandartization.Models
{
    public  class LogFile
    {
        DateTime Date { get; set; }
        LoggingLevel LoggingLevel { get; set; }

        string CallingMethod { get; set; } = "DEFAULT";
        string Message { get; set; }

        /// <summary>
        /// Checks the format of the log line.
        /// </summary>+
        /// 
        /// <param name="logLine"></param>
        /// <returns>The log file entity</returns>
        /// <exception cref="FormatException"></exception>
        public static LogFile Parse(string logLine)
        {
            if (string.IsNullOrWhiteSpace(logLine))
                return null;

            if (TryParseFormat1(logLine, out var logFile))
                return logFile;

            if (TryParseFormat2(logLine, out logFile))
                return logFile;

            throw new FormatException("Log line doesn't match any known format");
        }

        /// <summary>
        /// Checks if the log line is of format 1.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="logFile"></param>
        /// <returns>true/false</returns>
        private static bool TryParseFormat1(string line, out LogFile logFile)
        {
            logFile = null;
            // Format 1: 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
            var format1Pattern = @"^(\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}\.\d{3})\s+(\w+)\s+(.*)$";
            var match = Regex.Match(line, format1Pattern);

            if (match.Success)
            {
                if (!TryParseLogLevel(match.Groups[2].Value, out var level))
                    return false;

                logFile = new LogFile
                {
                    Date = DateTime.ParseExact(match.Groups[1].Value, "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture),
                    LoggingLevel = level,
                    Message = match.Groups[3].Value
                };
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the log line is of format 2.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="logFile"></param>
        /// <returns>true/false</returns>
        private static bool TryParseFormat2(string line, out LogFile logFile)
        {
            logFile = null;
            // Format 2: 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'
            var format2Pattern = @"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4})\s*\|\s*(\w+)\s*\|\s*(\d+)\s*\|\s*([^\|]+)\s*\|\s*(.+)$";
            var match = Regex.Match(line, format2Pattern);

            if (match.Success)
            {
                if (!TryParseLogLevel(match.Groups[2].Value, out var level))
                    return false;

                logFile = new LogFile
                {
                    Date = DateTime.ParseExact(match.Groups[1].Value, "yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture),
                    LoggingLevel = level,
                    CallingMethod = match.Groups[4].Value,
                    Message = match.Groups[5].Value
                };
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the logging level.
        /// </summary>
        /// <param name="levelStr"></param>
        /// <param name="level"></param>
        /// <returns>true/false</returns>
        private static bool TryParseLogLevel(string levelStr, out LoggingLevel level)
        {
            levelStr = levelStr.Trim().ToUpper();

            switch (levelStr)
            {
                case "INFO":
                case "INFORMATION":
                    level = LoggingLevel.Information;
                    return true;
                case "WARN":
                case "WARNING":
                    level = LoggingLevel.Warning;
                    return true;
                case "ERR":
                case "ERROR":
                    level = LoggingLevel.Error;
                    return true;
                case "DBG":
                case "DEBUG":
                    level = LoggingLevel.Debug;
                    return true;
                default:
                    level = LoggingLevel.Information;
                    return false;
            }
        }
    }
}
