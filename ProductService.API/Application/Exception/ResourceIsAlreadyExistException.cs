namespace ProductService.Api.Application.Exception;

public class ResourceIsAlreadyExistException(string massage) : System.Exception(massage);