using SharedKernel;

namespace Domain.Bronze.Todos;

public sealed record TodoItemDeletedDomainEvent(Guid TodoItemId) : IDomainEvent;
