

using AzureSchoolFunctionsApp.Core.Interfaces;
using AzureSchoolFunctionsApp.Infrastructure;
using Microsoft.Graph;

namespace AzureSchoolFunctionsApp.Application.Services
{
    public class SharePointService : ISharePointService
    {
        private readonly GraphServiceClient _graphClient;

        public SharePointService(string tenantId, string clientId, string clientSecret)
        {
            _graphClient = GraphApiClient.GetClient(tenantId, clientId, clientSecret);
        }

        public async Task<List<(string studentName, string courseName)>> GetStudentRegistrationsAsync()
        {
            var registrations = new List<(string, string)>();

            var siteId = Environment.GetEnvironmentVariable("SITE_ID");

            var studentsListId = "252f5e79-d14a-4797-bc75-1234d3a495c8";
            var coursesListId = "a772df97-e3bb-4f95-be01-ca71380bf52c";
            var registrationsListId = "e2fd3d0a-06d7-487f-9229-9808cdc22bd1";


            var students = await _graphClient.Sites[siteId].Lists[studentsListId].Items.GetAsync(requestConfig =>
            {
                requestConfig.QueryParameters.Expand = new[] { "fields" };
            });

            var courses = await _graphClient.Sites[siteId].Lists[coursesListId].Items.GetAsync(requestConfig =>
            {
                requestConfig.QueryParameters.Expand = new[] { "fields" };
            });

            var registrationsList = await _graphClient.Sites[siteId].Lists[registrationsListId].Items.GetAsync(requestConfig =>
            {
                requestConfig.QueryParameters.Expand = new[] { "fields" };
            });


            foreach (var reg in registrationsList.Value)
            {
                var studentId = reg.Fields.AdditionalData["StudentIDLookupId"].ToString();
                var courseId = reg.Fields.AdditionalData["CourseIDLookupId"].ToString();

                var student = students.Value.Find(s => s.Id == studentId)?.Fields.AdditionalData["Name"].ToString();
                var course = courses.Value.Find(c => c.Id == courseId)?.Fields.AdditionalData["Name"].ToString();

                if (!string.IsNullOrEmpty(student) && !string.IsNullOrEmpty(course))
                {
                    registrations.Add((student, course));
                }
            }

            return registrations;
        }
    }
}
