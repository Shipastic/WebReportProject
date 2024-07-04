using DAPManSWebReports.Entities.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Models
{
    public class SpParameter : ISpParameter
    {
        public int Direction { get; set; }
        public int ParameterType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
