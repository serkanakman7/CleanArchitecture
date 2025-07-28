namespace App.Domain.Exceptions
{
    public class CriticalException : Exception
    {
        public CriticalException()
        {
        }

        public CriticalException(string? message) : base(message)
        {
        }
    }
}
