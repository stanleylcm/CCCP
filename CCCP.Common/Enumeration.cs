﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCCP.Common
{
    public enum IncidentLevel
    {
        None = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    public enum IncidentType
    {
        Environment,
        OHS,
        Quality,
        System
    }

    public enum IncidentSubType
    {
        Air_Emission,
        Leakage,
        Corporate_Image,
        Generation,
        Network,
        Billing,
        Call_Centre,
        Invoicing,
        IT_System,
        OT_System,
        Network_Connectivity,
        Security
    }

    public enum IncidentStatus
    {
        Pending,
        In_Progress,
        Pending_For_Crisis_Approval,
        In_Crisis,
        Ready_For_Close,
        Closed,
        Cancelled
    }

    public enum CheckListActionStatus
    {
        Pending = 0,
        In_Progress = 1,
        Completed = 2
    }

    public enum EnquiryStatus
    {
        Pending,
        In_Progress,
        Closed, // without incident
        Completed // with incident
    }

    public enum CrisisStatus
    {
        Pending,
        In_Progress,
        Ready_For_Close,
        Closed
    }

    public enum MenuItem
    {
        RoleMaintainenance = 0,
        UserAccountMaintenance,
        Home_Index, // Home_Dashboard;
        GeneralEnquiry,
        IncidentManagement,
        IncidentQualityNetwork,
        IncidentQualityGeneration,
        IncidentQualityCorporateImage,
        IncidentEnvironmentLeakage,
        IncidentEnvironmentAirEmission,
        IncidentOHS,
        IncidentSystemInvoicing,
        IncidentSystemBilling,
        IncidentSystemCallCentre,
        IncidentSystemNetworkConnectivity,
        IncidentSystemITSystem,
        IncidentSystemOTSystem,
        IncidentSystemSecurity,
        OMSEvent,
        CrisisManagement,
        CrisisApproval,
        BusinessDataMainteneance,
        ChecklistMaintenance,
        AuditTrial,
        Report,
        Report_IncidentSummary,
        Report_IncidentDetails,
        Report_IncidentDetailsForCOT
    }

    public enum AuditLogEventType
    {
        Add,
        Delete,
        Modifiy
    }
}