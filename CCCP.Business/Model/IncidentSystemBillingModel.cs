using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;

namespace CCCP.Business.Model
{
    public class IncidentSystemBillingModel
    {
        public IncidentSystemBillingModel(IncidentSystemBilling viewModel)
        {
            this.Entity = viewModel;
        }

        private void init()
        {
            this.IncidentStatus = Entity.IncidentStatus.ToEnum<IncidentStatus>();
        }

        public IncidentSystemBilling Entity = new IncidentSystemBilling();
        public IncidentStatus IncidentStatus = IncidentStatus.Pending;
        public List<Checklist> Checklists = new List<Checklist>();


    }
}
