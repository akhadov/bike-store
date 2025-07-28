using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Silver.DimCategories;

public sealed class DimCategory : Entity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }
}
