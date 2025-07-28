using Application.Abstractions.Messaging;

namespace Application.Bronze.Todos.Get;

public sealed record GetTodosQuery(Guid UserId) : IQuery<List<TodoResponse>>;
