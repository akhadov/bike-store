using SharedKernel;

namespace Domain.Categories;

public sealed class Category : Entity
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
