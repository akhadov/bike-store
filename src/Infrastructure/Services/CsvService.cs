using System.Data;
using Application.Abstractions.Services;
using Infrastructure.Helpers;

namespace Infrastructure.Services;
public class CsvService : ICsvService
{
    public DataTable ReadAsDataTableAsync(string path)
    {
        return CsvHelperUtil.ReadDataTable(path);
    }
}
