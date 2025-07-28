using Application.Abstractions.Messaging;

namespace Application.Bronze.Orders.SyncOrderItems;

public sealed record SyncOrderItemsCommand(string FilePath) : ICommand;
