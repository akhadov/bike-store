using Application.Abstractions.Messaging;

namespace Application.SyncBronzeFromCsv;
public sealed record SyncBronzeFromCsvCommand(string FilePath) : ICommand;
