namespace Application.Configurations
{
    public class ExceptionNotFound : Exception
    {

        public ExceptionNotFound(string message) : base(message)
        {
        }

        public ExceptionNotFound(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
