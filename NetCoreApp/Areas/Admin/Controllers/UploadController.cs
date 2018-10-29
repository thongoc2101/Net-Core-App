using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// upload image for form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadImage()
        {
            //update in form
            DateTime now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }

            var file = files[0];
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var imageFolder = $@"\uploaded\images\{now:yyyyMMdd}";
            string folder = _hostingEnvironment.WebRootPath + imageFolder;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            // copy image
            string filePath = Path.Combine(folder, fileName);
            using (FileStream fs= System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            // return url file image
            return new OkObjectResult(Path.Combine(imageFolder, fileName).Replace(@"\", @"/"));
        }

        /// <summary>
        /// upload image in cKEditor
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="ckEditorFuncNum"></param>
        /// <param name="ckEditor"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task UploadImageForCkEditor(IList<IFormFile> upload, string ckEditorFuncNum, string ckEditor, string langCode)
        {
            DateTime now = DateTime.Now;
            if (upload.Count == 0)
            {
                await HttpContext.Response.WriteAsync("Please Input Image");
            }
            else
            {
                var file = upload[0];
                var filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');
                var imageFolder = $@"\uploaded\images\{now:yyyyMMdd}";
                string folder = _hostingEnvironment.WebRootPath + imageFolder;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                await HttpContext.Response.WriteAsync("<script>window.parent.CKEDITOR.tools.callFunction(" + ckEditorFuncNum + ", '" + Path.Combine(imageFolder, filename).Replace(@"\", @"/") + "');</script>");
            }
        }
    }
}