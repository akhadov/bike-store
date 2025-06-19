using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Infrastructure.Helpers;
public static class CsvHelperUtil
{
    public static List<T> ReadCsv<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecords<T>().ToList();
    }
}
