namespace TB.Shared.Exceptions.ModelExceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(FluentValidation.Results.ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();
            foreach (var error in validationResult.Errors)
            {
                ValidationErrors.Add(error.ErrorMessage);
            }
        }

        public List<string>? ValidationErrors { get; set; }    
    }
}
