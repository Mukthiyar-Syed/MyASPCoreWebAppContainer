using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyASPCoreWebAppContainer.Models;
using Microsoft.AspNetCore.Mvc;
using MyASPCoreWebAppContainer.Services;

namespace MyASPCoreWebAppContainer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AzureBlobService _azureBlobService;

        public HomeController(ILogger<HomeController> logger, AzureBlobService azureBlobService)
        {
            _logger = logger;
            _azureBlobService = azureBlobService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UploadBlob()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await _azureBlobService.UploadFileAsync(file);
                ViewBag.Message = "File uploaded to blob successfully!";
            }
            else
            {
                ViewBag.Message = "Please select a file to upload.";
            }
            return View("UploadBlob");
        }

        public async Task<IActionResult> ViewDownload()
        {
            var files = await _azureBlobService.ListFilesAsync();
            return View(files);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadBlob(string fileName)
        {
            try
            {
                var downloadResult = await _azureBlobService.DownloadFileAsync(fileName);
                var blobContent = downloadResult.Content.ToStream();
                return File(blobContent, downloadResult.Details.ContentType, fileName);
            }
            catch (Exception)
            {
                return NotFound("File not found in blob storage.");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
