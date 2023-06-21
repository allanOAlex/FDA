using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public sealed class ValueDoesNotBelongToObjectException : BadRequestException
    {
        public ValueDoesNotBelongToObjectException(Guid objectId, Guid valueId) : base($"The value with the identifier {valueId} does not belong to the object with the identifier {objectId}")
        {
        }
    }
}
