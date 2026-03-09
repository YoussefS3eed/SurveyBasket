namespace SurveyBasket.Domain.Errors;

public static class NotificationErrors
{
    public static readonly Error NoPollsFound =
        Error.NotFound("Notification.NoPolls", "No polls found to notify about");

    public static readonly Error NoUsersFound =
        Error.NotFound("Notification.NoUsers", "No users found to notify");
}
