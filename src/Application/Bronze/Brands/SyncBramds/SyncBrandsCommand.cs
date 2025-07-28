using Application.Abstractions.Messaging;

namespace Application.Bronze.Brands.SyncBramds;
public sealed record SyncBrandsCommand(string FilePath) : ICommand;
