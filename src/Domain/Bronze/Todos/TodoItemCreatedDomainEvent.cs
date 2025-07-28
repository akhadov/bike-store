using SharedKernel;

namespace Domain.Bronze.Todos;

public sealed record TodoItemCreatedDomainEvent(Guid TodoItemId) : IDomainEvent;
