using LogStandartization.Enums;

namespace LogStandartization.Models
{
    public abstract class LogFile
    {
        DateTime Date { get; set; }
        LoggingLevel LoggingLevel { get; set; }

        string CallingMethod { get; set; } = "DEFAULT";
        string Message { get; set; }

        public void Standartize(string path)
        {
            Get(path);
            Validate();
            Save();
        }

        public abstract void Get(string path);
        public abstract void Validate();
        public virtual void Save()
        {
            throw new NotImplementedException();
        }
    }
}
