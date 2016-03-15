using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCCP.Business.Model
{
    public interface IIncidentModel
    {
        List<ChecklistModel> Checklists { get; set; }
    }
}
