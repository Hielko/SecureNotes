using Microsoft.AspNetCore.Mvc;
using SecureNotes.Models;
using Simple.Encryption;
using System.Diagnostics;

namespace SecureNotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEncryption _encryption;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IEncryption encryption)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _encryption = encryption;
        }

        public IActionResult Index(string filename)
        {
            var vm = new TextViewModel();

            if (string.IsNullOrEmpty(filename))
            {
                filename = "TextFile.txt";
            }
            vm.Filename = filename;

            var file = Path.Combine(_webHostEnvironment.WebRootPath, "Data", filename);
            if (System.IO.File.Exists(file))
            {
                var text = System.IO.File.ReadAllText(file);
                vm.Text = _encryption?.Decrypt(text);
            }
            else
            {
                vm.Text = "New file";
            }

            return View(vm);
        }


        [Produces("application/json")]
        public IActionResult Save([FromBody] TextViewModel vm)
        {
         //   vm.Filename = string.Empty;

            if (!string.IsNullOrEmpty(vm.Filename))
            {
                var fileName = Path.Combine(_webHostEnvironment.WebRootPath, "Data", vm.Filename);

                if (System.IO.File.Exists(fileName))
                {
                    var backupFile = Path.ChangeExtension(fileName, ".bak");
                    System.IO.File.Delete(backupFile);
                    System.IO.File.Copy(fileName, backupFile);
                }
                
                var encryptedText = _encryption?.Encrypt(vm.Text);
                System.IO.File.WriteAllText(fileName, encryptedText);
            }
            else
            {
                return BadRequest($"Empty {nameof(vm.Filename)}");
            }
            return Ok("Saved");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}