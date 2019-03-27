using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Application.ViewModels.Common;
using NetCoreApp.Data.Enums;
using NetCoreApp.Utilities.Extensions;
using NetCoreApp.Utilities.Helpers;
using OfficeOpenXml;

namespace NetCoreApp.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public BillController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = ServiceRegistration.BillService.GetDetail(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult UpdateStatus(int billId, BillStatus status)
        {
            ServiceRegistration.BillService.UpdateStatus(billId, status);

            return new OkResult();
        }

        [HttpGet]
        public IActionResult GetAllPaging(string startDate, string endDate, string keyword, int page, int pageSize)
        {
            var model = ServiceRegistration.BillService.GetAllPaging(startDate, endDate, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(BillViewModel billVm)
        {
            // fields required don't nhap thi se bao loi. Bao loi cac truong bat buoc trong viewmodel
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (billVm.Id == 0)
            {
                ServiceRegistration.BillService.Create(billVm);
            }
            else
            {
                ServiceRegistration.BillService.Update(billVm);
            }

            return new OkObjectResult(billVm);
        }

        [HttpGet]
        public IActionResult GetPaymentMethod()
        {
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetBillStatus()
        {
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = ServiceRegistration.BillService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = ServiceRegistration.BillService.GetSizes();
            return new OkObjectResult(sizes);
        }

        [HttpPost]
        public IActionResult ExportExcel(int billId)
        {
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            var sFileName = $"Bill_{billId}.xlsx";

            // Template File
            var templateDocument = Path.Combine(sWebRootFolder, "template", "BillTemplate.xlsx");

            var url = $"{Request.Scheme}://{Request.Host}/{"export-files"}/{sFileName}";
            var file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Bills"];
                    // Data Access, load order header data.
                    var billDetail = ServiceRegistration.BillService.GetDetail(billId);

                    // Insert customer data into template
                    worksheet.Cells[4, 1].Value = "Customer Name: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Address: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "Phone: " + billDetail.CustomerMobile;
                    // Start Row for Detail Rows
                    int rowIndex = 9;

                    // load order details
                    var orderDetails = ServiceRegistration.BillService.GetBillDetails(billId);
                    var count = 1;
                    foreach (var orderDetail in orderDetails)
                    {
                        // Cell 1, Carton Count
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // Cell 2, Order Number (Outline around columns 2-7 make it look like 1 column)
                        worksheet.Cells[rowIndex, 2].Value = orderDetail.Product.Name;
                        // Cell 8, Weight in LBS (convert KG to LBS, and rounding to whole number)
                        worksheet.Cells[rowIndex, 3].Value = orderDetail.Quantity.ToString();

                        worksheet.Cells[rowIndex, 4].Value = orderDetail.Price.ToString("N0");
                        worksheet.Cells[rowIndex, 5].Value = (orderDetail.Price * orderDetail.Quantity).ToString("N0");
                        // Increment Row Counter
                        rowIndex++;
                        count++;
                    }
                    var total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    worksheet.Cells[24, 5].Value = total.ToString("N0");

                    var numberWord = "Total amount (by word): " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    var billDate = billDetail.DateCreated;
                    worksheet.Cells[28, 3].Value = billDate.Day + ", " + billDate.Month + ", " + billDate.Year;


                    package.SaveAs(file); //Save the workbook.
                }
            }
            return new OkObjectResult(url);
        }
    }
}