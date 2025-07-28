using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Silver.DimBrands;

public sealed class DimBrand : Entity
{
    public int BrandId { get; set; }

    public string BrandName { get; set; }
}
