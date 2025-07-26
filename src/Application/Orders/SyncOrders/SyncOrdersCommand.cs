using Application.Abstractions.Messaging;

namespace Application.Orders.SyncOrders;

public sealed record SyncOrdersCommand(string FilePath) : ICommand;
