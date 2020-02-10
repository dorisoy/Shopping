namespace JacksonVeroneze.Shopping.Common
{
    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        void Trace(string message);
    }
}