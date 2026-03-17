using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Roles.Queries.GetAllRoles;
using SurveyBasket.Application.Features.Users.Dtos;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace SurveyBasket.Application.Features.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    ISender sender,
    IBackgroundJobService backgroundJobService,
    IApplicationUrlService urlService,
    ILogger<CreateUserCommandHandler> logger)
    : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.IsEmailExistsAsync(request.Email, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail(request.Email));

        if (await userRepository.IsUsernameExistsAsync(request.UserName, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedUsername(request.UserName));

        if (request.Roles != null)
        {
            var rolesResult = await sender.Send(new GetAllRolesQuery(), cancellationToken);
            if (rolesResult.IsFailure)
                return Result.Failure<UserResponse>(rolesResult.Error);

            if (request.Roles.Except([.. rolesResult.Value.Select(r => r.Name)]).Any())
                return Result.Failure<UserResponse>(UserErrors.InvalidRoles);
        }

        var user = ApplicationUser.Create(
            request.FirstName,
            request.LastName,
            request.UserName,
            request.Email);

        var tempPassword = GenerateTemporaryPassword();
        var createResult = await userRepository.CreateAsync(user, tempPassword);

        if (createResult.IsFailure)
            return Result.Failure<UserResponse>(createResult.Error);

        var addRolesResult = await userRepository.AddToRolesAsync(user.Id, request.Roles ?? [], cancellationToken);
        if (addRolesResult.IsFailure)
            return Result.Failure<UserResponse>(addRolesResult.Error);

        var code = await userRepository.GenerateEmailConfirmationTokenAsync(user);
        code = code.ToBase64UrlEncoded();

        logger.LogInformation(
            "User created: {Email}. Confirmation code generated.", request.Email);

        var origin = urlService.GetOrigin();
        var confirmationLink =
            $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}";

        backgroundJobService.Enqueue<IEmailService>(emailService =>
            emailService.SendUserCreatedEmailAsync(user.Email!, user.FullName, user.UserName!, tempPassword, confirmationLink, cancellationToken));

        var response = new UserResponse(
            user.Id,
            user.UserName!,
            user.FirstName,
            user.LastName,
            user.Email!,
            user.IsDisabled,
            request.Roles ?? []);

        return Result.Success(response);
    }

    // Helpers Function
    private static string GenerateTemporaryPassword()
    {
        // Password must match ValidationPatterns.Password:
        // - At least 8 characters
        // - At least 1 lowercase letter
        // - At least 1 uppercase letter
        // - At least 1 digit
        // - At least 1 special character (!@#$%^&*()[]{}\-+=~`|:;"'<>,./?)
        
        var lowercase = "abcdefghijklmnopqrstuvwxyz";
        var uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var digits = "0123456789";
        var special = "!@#$%^&*()[]{}-+=~`|:;\"'<>,./?";
        var all = lowercase + uppercase + digits + special;

        var password = new char[12];
        
        // Ensure at least one of each required type
        password[0] = lowercase[RandomNumberGenerator.GetInt32(0, lowercase.Length)];
        password[1] = uppercase[RandomNumberGenerator.GetInt32(0, uppercase.Length)];
        password[2] = digits[RandomNumberGenerator.GetInt32(0, digits.Length)];
        password[3] = special[RandomNumberGenerator.GetInt32(0, special.Length)];
        
        for (int i = 4; i < password.Length; i++)
        {
            password[i] = all[RandomNumberGenerator.GetInt32(0, all.Length)];
        }

        // Shuffle the password to avoid predictable positions
        return new string(password.OrderBy(_ => RandomNumberGenerator.GetInt32(0, int.MaxValue)).ToArray());
    }
}
