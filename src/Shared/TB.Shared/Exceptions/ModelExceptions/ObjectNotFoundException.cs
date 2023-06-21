using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public sealed class ObjectNotFoundException : NotFoundException
    {
        public ObjectNotFoundException(Guid objectId) : base($"The object with the identifier {objectId} was not found.")
        {
        }
    }
}
