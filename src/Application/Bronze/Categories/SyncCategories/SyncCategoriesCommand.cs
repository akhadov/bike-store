using Application.Abstractions.Messaging;

namespace Application.Bronze.Categories.SyncCategories;

public sealed record SyncCategoriesCommand(string FilePath) : ICommand;