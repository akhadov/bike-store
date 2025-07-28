using Application.Abstractions.Messaging;

namespace Application.Bronze.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;
