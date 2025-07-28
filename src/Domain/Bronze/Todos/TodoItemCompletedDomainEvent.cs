using SharedKernel;

namespace Domain.Bronze.Todos;

public sealed record TodoItemCompletedDomainEvent(Guid TodoItemId) : IDomainEvent;
