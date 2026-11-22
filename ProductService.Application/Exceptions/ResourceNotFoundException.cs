namespace ProductService.Application.Exceptions;

public class ResourceNotFoundException(string message) : System.Exception(message);