using Application.Abstractions.Messaging;

namespace Application.Bronze.Orders.SyncOrders;

public sealed record SyncOrdersCommand(string FilePath) : ICommand;
