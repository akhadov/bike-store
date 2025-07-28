using Application.Abstractions.Messaging;

namespace Application.Bronze.Users.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
