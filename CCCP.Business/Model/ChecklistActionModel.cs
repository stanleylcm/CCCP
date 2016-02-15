﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;
using CCCP.Business.Service;

namespace CCCP.Business.Model
{
    public class ChecklistActionModel
    {
        public ChecklistActionModel()
        { 
        }
        public ChecklistActionModel(ChecklistAction entity)
        {
            this.Entity = entity;
        }

        public ChecklistAction Entity = new ChecklistAction();
        public CheckListActionStatus ActionStatus
        {
            get
            {
                return Entity.ActionStatus.ToEnum<CheckListActionStatus>();
            }
            set
            {
                Entity.ActionStatus = value.ToEnumString();
            }
        }
        public bool IsCompulsory
        {
            get
            {
                return Entity.IsCompulsory.Value;
            }
        }
        public bool HasUpdate = false;

        public void ToggleActionStatus()
        {
            switch (ActionStatus)
            {
                case CheckListActionStatus.Pending: { ActionStatus = CheckListActionStatus.In_Progress; break; }
                case CheckListActionStatus.In_Progress: { ActionStatus = CheckListActionStatus.Completed; break; }
                case CheckListActionStatus.Completed: { ActionStatus = CheckListActionStatus.Pending; break; }
            }

            HasUpdate = true;

        }
        public void PrepareSave(DateTime updateDateTime)
        {
            Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            Entity.LastUpdatedDateTime = updateDateTime;
        }
    }
}
