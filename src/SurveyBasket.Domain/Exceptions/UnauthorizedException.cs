namespace SurveyBasket.Domain.Exceptions;

public class UnauthorizedException(string message = "Invalid credentials") : Exception(message)
{
}
