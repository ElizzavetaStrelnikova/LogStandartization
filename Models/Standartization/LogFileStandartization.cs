using LogStandartization.Models.LogFileModel;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace LogStandartization.Models
{
    public  class LogFileStandartization
    {
        private readonly IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        private static readonly Encoding RussianEncoding = Encoding.GetEncoding(1251);

        public void GetLogFile()
        {
            string logFilePath = config["Logging:LogFilePath"];
            string fullPath = Path.GetFullPath(logFilePath);

            string rightlogFilePath = config["Logging:RightLogFilePath"];
            string rightfullPath = Path.GetFullPath(rightlogFilePath);

            string problemlogFilePath = config["Logging:ProblemLogFilePath"];
            string problemfullPath = Path.GetFullPath(problemlogFilePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("The log file was not found.");
            }

            string[] data = File.ReadAllLines(fullPath, RussianEncoding);

            if (data.Length == 0 || data.All(string.IsNullOrWhiteSpace))
            {
                Console.WriteLine("The log file is empty.");
                return;
            }

            foreach (string line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    LogFile logEntry = LogFile.Parse(line);

                    (string standardizedLog, bool isValid) = ValidateLogFile(logEntry);

                    if (isValid)
                    {
                        File.AppendAllText(rightfullPath, standardizedLog + Environment.NewLine, new UTF8Encoding(true));
                    }
                    else
                    {
                        File.AppendAllText(problemfullPath, line + Environment.NewLine, new UTF8Encoding(true));
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(problemfullPath, $"{line} [Error: {ex.Message}]" + Environment.NewLine, new UTF8Encoding(true));
                }
            }
        }

        public (string standardizedLog, bool isValid) ValidateLogFile(LogFile logEntry)
        {
            bool isValid = logEntry != null &&
                 logEntry.Date != DateTime.MinValue &&
                 !string.IsNullOrWhiteSpace(logEntry.Message);

            string standardizedLog = isValid ? logEntry.ToOutputFormat() : string.Empty;

            return (standardizedLog, isValid);
        }
    }
}
