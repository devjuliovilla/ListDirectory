using ListDirectory.Authentication;
using ListDirectory.Extensions;
using ListDirectory.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListDirectory.Controllers
{
    [AppAuthentication]
    [Route("/Files")]
    public class FilesController : Controller
    {
        private readonly ILogger<FilesController> logger;
        private readonly string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "files");
        private string currentFolder;

        public FilesController(ILogger<FilesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("/files/{folder?}")]
        public IActionResult Files(string folder = "")
        {
            try
            {
                var sessionCurrentFolder = HttpContext.Session.Get<string>("CurrentFolder");
                if (string.IsNullOrEmpty(sessionCurrentFolder))
                {
                    currentFolder = String.IsNullOrEmpty(folder) ? rootFolder : Path.Combine(rootFolder, folder.Replace(Path.DirectorySeparatorChar.ToString(), ""));
                    HttpContext.Session.Set("CurrentFolder", currentFolder);
                }
                else
                {
                    currentFolder = Path.Combine(sessionCurrentFolder, folder);
                    HttpContext.Session.Set("CurrentFolder", currentFolder);
                }

                var breadcrumbs = GetBreadcrumbs(currentFolder);
                HttpContext.Session.Set("Breadcrumbs", breadcrumbs);
                ViewData["Breadcrumbs"] = breadcrumbs;

                var isRoot = string.IsNullOrEmpty(folder);
                ViewData["IsRoot"] = isRoot;

                var entries = Directory.GetFileSystemEntries(currentFolder)
                    .Select(entry => new FileViewModel
                    {
                        FileName = Path.GetFileName(entry),
                        FileType = System.IO.File.GetAttributes(entry).HasFlag(FileAttributes.Directory) ? "Folder" : "File",
                        FileSizeBytes = System.IO.File.GetAttributes(entry).HasFlag(FileAttributes.Directory) ? 0 : new FileInfo(entry).Length,
                        CreationDate = System.IO.File.GetCreationTime(entry)
                    });

                return View(entries);
            }
            catch (Exception ex)
            {
                logger.LogError("Error getting files");
                logger.LogError(ex, ex.Message);
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("/downloadFile/{fileName}")]
        public IActionResult Download(string fileName)
        {
            try
            {
                currentFolder = HttpContext.Session.Get<string>("CurrentFolder");
                var filePath = Path.Combine(currentFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    return PhysicalFile(filePath, "application/octet-stream", fileName, enableRangeProcessing: true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error downloading file");
                logger.LogError(ex, ex.Message);
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("/files/up")]
        public IActionResult GoUp()
        {
            try
            {
                HttpContext.Session.Set("CurrentFolder", "");
                var breadcrumbs = HttpContext.Session.Get<List<BreadcrumbViewModel>>("Breadcrumbs");

                if (breadcrumbs != null && breadcrumbs.Count > 0)
                {
                    var previousPath = breadcrumbs[breadcrumbs.Count - 2].Path;
                    return RedirectToAction("Files", new { folder = previousPath });
                }

                return RedirectToAction("");
            }
            catch (Exception ex)
            {
                logger.LogError("Error going up");
                logger.LogError(ex, ex.Message);
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private List<BreadcrumbViewModel> GetBreadcrumbs(string folder)
        {
            var breadcrumbs = new List<BreadcrumbViewModel>();

            var folders = folder.Replace(rootFolder, "").Split(Path.DirectorySeparatorChar);

            foreach (var item in folders)
            {
                breadcrumbs.Add(new BreadcrumbViewModel
                {
                    Name = item,
                    Path = string.Join(Path.DirectorySeparatorChar, folders.Take(Array.IndexOf(folders, item) + 1))
                });
            }

            return breadcrumbs;
        }


    }
}