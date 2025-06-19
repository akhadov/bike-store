using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.SyncCategoriesFromCsv;
public sealed record SyncCategoriesResponse
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; }
}
