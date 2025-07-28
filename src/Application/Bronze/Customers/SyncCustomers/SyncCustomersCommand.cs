using Application.Abstractions.Messaging;

namespace Application.Bronze.Customers.SyncCustomers;

public sealed record SyncCustomersCommand(string FilePath) : ICommand;
