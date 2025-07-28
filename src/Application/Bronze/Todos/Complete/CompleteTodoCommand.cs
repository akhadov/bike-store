using Application.Abstractions.Messaging;

namespace Application.Bronze.Todos.Complete;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;
