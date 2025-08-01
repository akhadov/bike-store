﻿using FluentValidation;

namespace Application.Bronze.Todos.Complete;

internal sealed class CompleteTodoCommandValidator : AbstractValidator<CompleteTodoCommand>
{
    public CompleteTodoCommandValidator()
    {
        RuleFor(c => c.TodoItemId).NotEmpty();
    }
}
