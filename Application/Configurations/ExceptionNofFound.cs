namespace Application.Configurations
{
    public class ExceptionNofFound : Exception
    {
        public ExceptionNofFound() : base()
        {
        }

        public ExceptionNofFound(string message) : base(message)
        {
        }

        public ExceptionNofFound(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
