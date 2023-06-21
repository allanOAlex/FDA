namespace TB.Application.Abstractions.IServices
{
    public interface IValidationService 
    {
        Task<bool> ValidateAsync(dynamic value);
    }
}
