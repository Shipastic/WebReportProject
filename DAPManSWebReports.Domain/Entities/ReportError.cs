﻿using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class ReportError
    {
        public string Description { get; set; }
        public string Email { get; set; }
        public IFormFile File { get; set; }
        public string Url { get; set; }
    }
}
