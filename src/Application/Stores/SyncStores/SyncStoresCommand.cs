using Application.Abstractions.Messaging;

namespace Application.Stores.SyncStores;

public sealed record SyncStoresCommand(string FilePath) : ICommand;
