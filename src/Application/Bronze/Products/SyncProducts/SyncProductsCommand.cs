using Application.Abstractions.Messaging;

namespace Application.Bronze.Products.SyncProducts;

public sealed record SyncProductsCommand(string FilePath) : ICommand;
