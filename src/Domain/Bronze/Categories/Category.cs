using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Bronze.Categories;

public sealed class Category : Entity
{
    [Name("category_id")]
    public int CategoryId { get; set; }

    [Name("category_name")]
    public string CategoryName { get; set; }
}
