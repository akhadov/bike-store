using Application.Abstractions.Messaging;

namespace Application.Products.SyncProducts;

public sealed record SyncProductsCommand(string FilePath) : ICommand;
