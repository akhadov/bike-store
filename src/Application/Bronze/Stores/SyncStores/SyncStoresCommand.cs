using Application.Abstractions.Messaging;

namespace Application.Bronze.Stores.SyncStores;

public sealed record SyncStoresCommand(string FilePath) : ICommand;
