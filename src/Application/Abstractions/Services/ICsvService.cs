using System.Data;

namespace Application.Abstractions.Services;
public interface ICsvService
{
    DataTable ReadAsDataTableAsync(string path);
}
