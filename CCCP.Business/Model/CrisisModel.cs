using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;
using CCCP.Business.Service;

namespace CCCP.Business.Model
{
    public class CrisisModel
    {
        #region Constructor

        public CrisisModel()
        {
            init();
        }
        public CrisisModel(Crisis viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {

        }

        #endregion

        #region Declaration

        public Crisis Entity = new Crisis();
        public ChatRoomModel Chatroom { get; set; }
        public List<ChecklistModel> Checklists { get; set; }
        public List<ChecklistExtend> ChecklistEntities
        {
            set
            {
                Checklists = new List<ChecklistModel>();
                foreach (ChecklistExtend checklistEntity in value) Checklists.Add(new ChecklistModel(checklistEntity));
            }
        }

        #endregion

        #region Public Method

        public bool IsReadyForClose()
        {
            foreach (ChecklistModel checklist in this.Checklists) if (!checklist.IsAllCompulsoryCompleted()) return false;
            return true;
        }

        public void PrepareSave(PrepareSaveMode saveMode = PrepareSaveMode.Last_Updated)
        {
            DateTime now = DateTime.Now;
            Entity.History = AuditTrailService.GetHistory(Entity.History, AccessControlService.CurrentUser.GetLastUpdatedBy(), now, saveMode.ToEnumString());

            switch (saveMode)
            {
                case PrepareSaveMode.Created:
                    Entity.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.CreatedDateTime = now;
                    if (Entity.IssueById == null || Entity.IssueById == 0)
                    {
                        Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                        Entity.IssueDateTime = now;
                    }

                    Entity.CrisisNo = IncidentService.GetNewIncidentNo(SequenceType.Crisis, DateTime.Now.Year);
                    Entity.Status = CrisisStatus.Pending_For_Approval.ToEnumString();
                    break;
                case PrepareSaveMode.Last_Updated:
                    if (IsReadyForClose())
                        Entity.Status = CrisisStatus.Ready_For_Close.ToEnumString();
                    else
                        Entity.Status = CrisisStatus.In_Progress.ToEnumString();
                    break;
                case PrepareSaveMode.Approved:
                    Entity.Status = CrisisStatus.Pending.ToEnumString();
                    break;
                case PrepareSaveMode.Rejected:
                    Entity.Status = CrisisStatus.Rejected.ToEnumString();
                    break;
                case PrepareSaveMode.Closed:
                    Entity.CloseById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.CloseDateTime = now;
                    Entity.Status = CrisisStatus.Closed.ToEnumString();
                    break;
            }

            Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            Entity.LastUpdatedDateTime = now;
        }

        #endregion

        #region Private Method

        #endregion
    }
}
