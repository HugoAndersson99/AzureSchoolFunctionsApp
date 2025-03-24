

namespace AzureSchoolFunctionsApp.Core.Interfaces
{
    public interface ISharePointService
    {
        Task<List<(string studentName, string courseName)>> GetStudentRegistrationsAsync();

    }
}
