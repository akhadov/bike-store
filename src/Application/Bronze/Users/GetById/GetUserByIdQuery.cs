using Application.Abstractions.Messaging;

namespace Application.Bronze.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
