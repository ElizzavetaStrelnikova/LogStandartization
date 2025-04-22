using LogStandartization.Enums;

namespace LogStandartization.Models
{
    public abstract class LogFile
    {
        DateTime Date { get; set; }
        LoggingLevel LoggingLevel { get; set; }

        string CallingMethod { get; set; } = "DEFAULT";
        string Message { get; set; }
    }
}
