
using Simple.Encryption;
using LightControl.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace LightControl.Controllers
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


        public string Save([FromBody] TextViewModel vm)
        {
            var file = Path.Combine(_webHostEnvironment.WebRootPath, "Data", vm.Filename);
            var encr = _encryption?.Encrypt(vm.Text);
            System.IO.File.WriteAllText(file, encr);
            return "OK";
        }


        public IActionResult DoPost(TextViewModel scheduleViewModel)
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}