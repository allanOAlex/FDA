namespace TB.Shared.Exceptions.ModelExceptions
{
    public class CreatingDuplicateException : Exception
    {
        public CreatingDuplicateException(string message = null!) : base(message: message)
        {
        }
    }
}
