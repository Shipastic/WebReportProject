using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class FolderDetail
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public IEnumerable<FolderModel> ChildFolders { get; set; }
        public IEnumerable<DataViewModel> Dataviews { get; set; }
    }
}
