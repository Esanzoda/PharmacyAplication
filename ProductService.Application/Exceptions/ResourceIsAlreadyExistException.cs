namespace ProductService.Application.Exceptions;

public class ResourceIsAlreadyExistException(string massage) : System.Exception(massage);