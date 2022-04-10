﻿using Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ExportSamplePDF
{
    public class ExportSamplePDF
    {
        public class Command : IRequest<byte[]>
        {

        }

        public class Handler : IRequestHandler<Command, byte[]>
        {
            private readonly IPDFOperations _pdfOperations;
            private readonly IWebHostEnvironment _env;

            public Handler(IPDFOperations pdfOperations, IWebHostEnvironment env)
            {
                _pdfOperations = pdfOperations;
                _env = env;
            }
            public async Task<byte[]> Handle(Command request, CancellationToken cancellationToken)
            {
                var users = new[] 
                {
                    new { Name = "Tarek", Email = "tarek.iraqi@gmail.com" },
                    new { Name = "Tarek", Email = "tarek.iraqi@gmail.com" }
                };

                var page = new StringBuilder();
                page.Append("<html>");
                page.Append("<head></head>");
                page.Append("<body>");
                page.Append("<div class='header'><h1>This is the generated PDF report!!!</h1></div>");
                page.Append("<table align='center'>");
                page.Append("<tr>");
                page.Append("<th>Name</th>");
                page.Append("<th>Email Address</th>");
                page.Append("</tr>");

                foreach (var user in users)
                {
                    page.Append("<tr>");
                    page.Append($"<td>{user.Name}</td>");
                    page.Append($"<td>{user.Email}</td>");
                    page.Append("</tr>");
                }

                page.Append("</table>");
                page.Append("</body>");
                page.Append("</html>");

                return _pdfOperations.ConvertHTMLToPDF(page.ToString(), Path.Combine(_env.WebRootPath, "css", "pdfStyles.css"));
            }
        }
    }
}
