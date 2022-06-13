namespace Geolocator.Services.Helpers;

public static class ExceptionHelper
{
    public static Exception WrongDataInputException(string description)
    {
        throw new WrongDataInputException(description);
    }

    public static Exception EntityNotFoundException(string entity, string id)
    {
        throw new EntityNotFoundException(entity, id);
    }

    public static Exception EntityAlreadyExistsException(string entity, string id)
    {
        throw new EntityAlreadyExistsException(entity, id);
    }
}

public class WrongDataInputException : Exception
{
    private const string DefaultMessage = "The wrong data was provided.";

    public WrongDataInputException(string detailedMessage, string message = DefaultMessage) :
        base($"{message} {detailedMessage}")
    {
    }
}

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, string id) :
        base($"The entity: {entityName} with the given id: ${id} was not found.")
    {
    }
}

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string entityName, string id) :
        base($"The entity: {entityName} with the given id: ${id} already exists.")
    {
    }
}