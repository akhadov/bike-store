using SharedKernel;

namespace Domain.Bronze.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
