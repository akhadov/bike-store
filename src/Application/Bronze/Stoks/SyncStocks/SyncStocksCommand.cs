using Application.Abstractions.Messaging;

namespace Application.Bronze.Stoks.SyncStocks;

public sealed record SyncStocksCommand(string FilePath) : ICommand;
