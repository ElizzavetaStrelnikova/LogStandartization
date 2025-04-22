namespace LogStandartization.Models
{
    public abstract class LogFileStandartization
    {
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
