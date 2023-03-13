using System;
using System.Threading.Tasks;

namespace Job_Service.Services
{
    public interface ICVRepository
    {
        ValueTask<dynamic> GetCVData(string commandText);
        ValueTask<dynamic> AddEmployee(string title, string description, int locationId, int departmentId, DateTime closingDate);
        ValueTask<dynamic> UpdateEmployee(int id,string title, string description, int locationId, int departmentId, DateTime closingDate);

    }
}
