using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Infrastructure.Helpers;
public static class CsvHelperUtil
{
    public static List<T> ReadCsv<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture);

        string[] nullValues = new[] { "NULL", "null", "N/A", "n/a", "NA", "na" };

        using var csv = new CsvReader(reader, config);

        foreach (string nullValue in nullValues)
        {
            csv.Context.TypeConverterOptionsCache.GetOptions<int?>().NullValues.Add(nullValue);
            csv.Context.TypeConverterOptionsCache.GetOptions<long?>().NullValues.Add(nullValue);
            csv.Context.TypeConverterOptionsCache.GetOptions<decimal?>().NullValues.Add(nullValue);
            csv.Context.TypeConverterOptionsCache.GetOptions<DateTime?>().NullValues.Add(nullValue);
            csv.Context.TypeConverterOptionsCache.GetOptions<bool?>().NullValues.Add(nullValue);
            csv.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(nullValue);
        }

        return csv.GetRecords<T>().ToList();
    }

    public static DataTable ReadDataTable(string filePath)
    {
        using var reader = new StreamReader(filePath);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        using var dataReader = new CsvDataReader(csv);

        var dataTable = new DataTable();

        dataTable.Load(dataReader);

        return dataTable;
    }
}
