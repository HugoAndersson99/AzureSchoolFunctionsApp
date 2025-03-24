using AzureSchoolFunctionsApp.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AzureSchoolFunctionsApp.Presentation.Functions
{
    public class SharePointToBlobFunction
    {
        private readonly ILogger<SharePointToBlobFunction> _logger;
        private readonly ISharePointService _sharePointService;
        private readonly IBlobStorageService _blobStorageService;

        public SharePointToBlobFunction(ILogger<SharePointToBlobFunction> logger, ISharePointService sharePointService, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _sharePointService = sharePointService;
            _blobStorageService = blobStorageService;
        }

        [Function("SharePointToBlobFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Processing SharePoint data...");

            var registrations = await _sharePointService.GetStudentRegistrationsAsync();

            var csvContent = GenerateCsvContent(registrations);

            string fileName = $"students-courses-{DateTime.UtcNow:yyyyMMdd-HHmmss}.csv";
            await _blobStorageService.UploadFileAsync("csv-files", fileName, csvContent);

            _logger.LogInformation("File uploaded successfully.");

            return new OkObjectResult("File uploaded to Blob Storage.");
        }

        private string GenerateCsvContent(IList<(string StudentName, string CourseName)> registrations)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("StudentName,CourseName");

            foreach (var registration in registrations)
            {
                csvContent.AppendLine($"{registration.StudentName},{registration.CourseName}");
            }

            return csvContent.ToString();
        }
    }
}
