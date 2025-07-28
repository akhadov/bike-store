using Application.Abstractions.Messaging;

namespace Application.Bronze.Todos.Delete;

public sealed record DeleteTodoCommand(Guid TodoItemId) : ICommand;
