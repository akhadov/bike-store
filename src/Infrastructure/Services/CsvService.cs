using System.Data;
using Application.Abstractions.Services;
using Infrastructure.Helpers;

namespace Infrastructure.Services;
public class CsvService : ICsvService
{
    public List<T> ReadCsv<T>(string filePath)
    {
        return CsvHelperUtil.ReadCsv<T>(filePath);
    }

    public DataTable ReadAsDataTableAsync(string path)
    {
        return CsvHelperUtil.ReadDataTable(path);
    }
}
