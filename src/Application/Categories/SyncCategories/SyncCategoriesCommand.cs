using Application.Abstractions.Messaging;

namespace Application.Categories.SyncCategories;

public sealed record SyncCategoriesCommand(string FilePath) : ICommand;