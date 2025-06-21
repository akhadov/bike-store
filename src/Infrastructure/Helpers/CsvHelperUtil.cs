using System.Data;
using System.Globalization;
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
