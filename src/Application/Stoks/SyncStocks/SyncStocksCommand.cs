using Application.Abstractions.Messaging;

namespace Application.Stoks.SyncStocks;

public sealed record SyncStocksCommand(string FilePath) : ICommand;
