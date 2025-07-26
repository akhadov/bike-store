using System.Data;

namespace Application.Abstractions.Services;
public interface ICsvService
{
    List<T> ReadCsv<T>(string filePath);
    DataTable ReadAsDataTableAsync(string path);
}
