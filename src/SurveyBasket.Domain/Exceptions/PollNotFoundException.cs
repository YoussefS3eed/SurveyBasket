namespace SurveyBasket.Domain.Exceptions;

public sealed class PollNotFoundException(int id) : NotFoundException($"Poll with ID {id} was not found.")
{
}