namespace Hack.Common.Framework
{
    public interface ICommClient
    {
       bool SendMessage(string to, string message);
    }
}
