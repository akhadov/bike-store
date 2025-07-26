
using Application.Abstractions.Messaging;

namespace Application.Customers.SyncCustomers;

public sealed record SyncCustomersCommand(string FilePath) : ICommand;
