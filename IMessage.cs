namespace Lotaria.Logging
{
    public interface IMessage
    {
        void Debug(string message);
        void Warning(string message);
        void Error(string message);
    }
}
