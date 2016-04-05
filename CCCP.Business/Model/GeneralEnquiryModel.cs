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
    public class GeneralEnquiryModel
    {
        #region Constructor

        public GeneralEnquiryModel()
        {
            init();
        }
        public GeneralEnquiryModel(GeneralEnquiry viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {

        }

        #endregion

        #region Declaration

        public GeneralEnquiry Entity = new GeneralEnquiry();
        public EnquiryStatus EnquiryStatus
        {
            get
            {
                return Entity.Status.ToEnum<EnquiryStatus>();
            }
        }
        public String IssueBy
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();
                User user = db.User.Find(Entity.IssueById);
                if (user == null) return "";

                UserModel userModel =  new UserModel(user);
                return userModel.GetLastUpdatedBy();
            }
        }
        public String EnquiryType
        {
            get
            {
                return MasterTableService.GetIncidentTypeSubType(Entity.IncidentTypeId).ToEnumString();
            }
        }

        #endregion

        #region Public Method

        public bool IsReadyForClose()
        {
            //return IncidentService.IsReadyForClose(this);
            return true;
        }
        public void PrepareSave(PrepareSaveMode saveMode = PrepareSaveMode.Last_Updated)
        {
            DateTime now = DateTime.Now;

            switch (saveMode)
            {
                case PrepareSaveMode.Created:
                    Entity.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.CreatedDateTime = now;
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.IssueDateTime = now;
                    break;
                case PrepareSaveMode.Last_Updated:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    break;
                case PrepareSaveMode.Closed:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.CloseById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.CloseDateTime = now;
                    break;
            }
        }

        #endregion

        #region Private Method

        #endregion
    }
}
