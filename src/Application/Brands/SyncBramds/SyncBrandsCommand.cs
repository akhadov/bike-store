using Application.Abstractions.Messaging;

namespace Application.Brands.SyncBramds;
public sealed record SyncBrandsCommand(string FilePath) : ICommand;
