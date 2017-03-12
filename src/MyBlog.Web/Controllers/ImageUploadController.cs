using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    public class ImageUploadController : Controller
    {

        private IHostingEnvironment _environment;

        public ImageUploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(ICollection<IFormFile> files,string fileUrls)
        {
            List<String> urls = new List<string>();
            if (!string.IsNullOrWhiteSpace(fileUrls))
            {
                var fileArray = fileUrls.Split(',');
                foreach (var item in fileArray)
                {
                    urls.Add(item);
                }
            }
            

            var uploads = Path.Combine(_environment.WebRootPath, "img");
            foreach (var file in files)
            {
                if (file.Length > 0)
                { 
                    var name = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploads, name);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                          file.CopyTo(fileStream);
                    }
                   // _environment.WebRootPath.
                    urls.Add(Url.Content("~/img/"+name));
                   
                }
            }
            return View(urls);
             
        }
    }
}
 