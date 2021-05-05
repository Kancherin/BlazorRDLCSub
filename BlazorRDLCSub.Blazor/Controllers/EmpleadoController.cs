using BlazorRDLCSub.Blazor.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRDLCSub.Blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmpleadoService empleadoService = new EmpleadoService();

        public EmpleadoController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("PdfReport")]
        public IActionResult PDFReport()
        {
            return EmpleadosReporte("PDF", "pdf", "application/pdf");
        }

        private IActionResult EmpleadosReporte(string renderFormat, string extension, string mimeType)
        {
            using var report = new LocalReport();
            var data = new DataTable();
            data = empleadoService.GetEmpleado();

            report.DataSources.Add(new ReportDataSource("dsEmpleado", data));
            string imageParam = "";
            var imagePath = $"{_webHostEnvironment.WebRootPath}\\Images\\reporte.png";
            using(var b = new Bitmap(imagePath))
            {
                using(var ms = new MemoryStream())
                {
                    b.Save(ms, ImageFormat.Bmp);
                    imageParam = Convert.ToBase64String(ms.ToArray());
                }
            }
            string imageParam2 = "";
            var imagePath2 = $"{_webHostEnvironment.WebRootPath}\\Images\\netCore.png";
            using (var b = new Bitmap(imagePath2))
            {
                using (var ms = new MemoryStream())
                {
                    b.Save(ms, ImageFormat.Bmp);
                    imageParam2 = Convert.ToBase64String(ms.ToArray());
                }
            }

            var parameters = new[]
            {
                new ReportParameter("param", "RDLC Sub Report in Blazor With Image"),
                new ReportParameter("image", imageParam),
                new ReportParameter("image2", imageParam2)
            };
            
            report.ReportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\rpEmpleados.rdlc";
            report.SetParameters(parameters);

            //Sub-Report
            report.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessing);
                        
            var pdf = report.Render(renderFormat);
            return File(pdf, mimeType, "report." + extension);
        }

        void SubReportProcessing(Object sender, SubreportProcessingEventArgs args)
        {
            int Id = int.Parse(args.Parameters["Id"].Values[0].ToString());
            var detailData = new DataTable();
            detailData = empleadoService.GetEmpleadoDetalles().Select("Id=" + Id).CopyToDataTable();

            ReportDataSource ds = new ReportDataSource("dsEmpleadoDetalle", detailData);
            args.DataSources.Add(ds);
        }
    }
}
