using Application.Abstractions.Messaging;

namespace Application.Orders.SyncOrderItems;

public sealed record SyncOrderItemsCommand(string FilePath) : ICommand;
