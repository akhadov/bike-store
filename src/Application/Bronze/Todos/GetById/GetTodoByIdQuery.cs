using Application.Abstractions.Messaging;

namespace Application.Bronze.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
