﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCCP.Common
{
    public enum IncidentLevel
    {
        None = 0,
        Level_1 = 1,
        Level_2 = 2,
        Level_3 = 3
    }

    public enum IncidentLevelWithCrisis
    {
        Level_1 = 1,
        Level_2 = 2,
        Level_3 = 3,
        Crisis = 4
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
        //Security
    }

    public enum IncidentTypeSubType
    {
        EnvironmentLeakage,
        EnvironmentAirEmission,
        QualityGeneration,
        OHS,
        QualityCorporateImage,
        QualityNetwork,
        SystemInvoicing,
        SystemBilling,
        SystemCallCentre,
        SystemNetworkConnectivity,
        SystemITSystem,
        SystemOTSystem
        //SystemSecurity
    }

    public enum SequenceType
    {
        Incident,
        Crisis
    }


    public enum OMSStatus 
    {
        Pending,
        In_Progress,
        Closed,
        Cancelled
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
        Pending_For_Approval,
        Rejected,
        Pending,
        In_Progress,
        Ready_For_Close,
        Closed
    }

    public enum MenuItem
    {
        RoleMaintenance = 0,
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
        BusinessDataMaintenance,
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

    public enum PrepareSaveMode
    {
        Created,
        Last_Updated,
        Closed,
        Approved,
        Rejected,
        Cancelled
    }

    public enum SystemFunctionCode
    {
        None,
        SM_Role,
        EC_Role,
        CreateUpdate,
        ChatRoom,
        Notification,
        ActionChecklist
    }

    public enum NotificationMessageType
    {
        GeneralEnquiry,
        Incident,
        Crisis,
        Escalate_Crisis
    }

    #region Enum for Incident - System (Billing)
    public enum IncidentSystemBillingProblemArea
    {
        System_Bug,
        Imperfect_Business_Logic
    }

    public enum IncidentSystemBillingPossibleCause
    {
        Undefined_Scenario,
        Not_Yet_Confirmed
    }

    public enum IncidentSystemBillingBillingErrorSeriousness
    {
        Comfort_Zone,
        Manageable_Zone,
        Danger_Zone
    }

    public enum IncidentSystemBillingContactedBy
    {
        None,
        Customer,
        Consumer_Council,
        Government,
        Media
    }

    public enum IncidentSystemBillingStatusUpdate
    {
        Under_Investigation,
        Defining_Mitigating_Actions,
        Defining_Resolution,
        Implementing_Mitigating_Actions,
        Implementing_Resolution,
        Closing_Communication,
        Case_Close
    }

    public enum IncidentSystemBillingRequireMitigatingAction
    {
        Yes,
        No,
        Not_Yet_Confirmed
    }

    public enum IncidentSystemBillingMitigatingAction
    {
        Create_Temp_Tools_for_Manual_Control_and_Correction,
        Suspend_Bill_Generation,
        Others_9please_specify0
    }

    public enum IncidentSystemBillingInputKey
    {
        IncidentSystemBilling_ContactedBy,
        IncidentSystemBilling_MitigatingAction,
        IncidentSystemBilling_PossibleCause,
        IncidentSystemBilling_ProblemArea,
        IncidentSystemBilling_RequireMitigatingAction,
        IncidentSystemBilling_StatusUpdate,
        IncidentSystemBilling_BillingErrorSeriousness
    }
    #endregion

    #region Enum for Incident - System (Invoicing)
    public enum IncidentSystemInvoicingMitigatingAction
    {
        Trigger_Emergency_Backup,
        Others_9please_specify0
    }

    public enum IncidentSystemInvoicingInputKey
    {
        IncidentSystemInvoicing_ContactedBy,
        IncidentSystemInvoicing_MitigatingAction,
        IncidentSystemInvoicing_PossibleCause,
        IncidentSystemInvoicing_ProblemArea,
        IncidentSystemInvoicing_RequireMitigatingAction,
        IncidentSystemInvoicing_StatusUpdate,
        IncidentSystemInvoicing_Impact
    }
    #endregion

    #region Enum for Incident - System (Call Centre)
    public enum IncidentSystemCallCentreStatusUpdate
    {
        Under_Investigation,
        Problem_Identified,
        Implementing_mitigating_actions,
        Partial_service_resumed,
        Full_service_restoration,
        Others_9please_specify0
    }

    public enum IncidentSystemCallCentreImpact
    {
        _5_workstation_failure,
        Unavailability_of_self_service,
        Absence_of_voice_record,
        Absence_of_report,
        No_call_routing_function,
        Absence_of_E1_Line,
        Suspension_of_Call_Centre,
        Others_9please_specify0
    }

    public enum IncidentSystemCallCentrePossibleCause
    {
        Network_Failure
    }

    public enum IncidentSystemCallCentreInputKey
    {
        IncidentSystemCallCentre_MitigatingAction,
        IncidentSystemCallCentre_PossibleCause,
        IncidentSystemCallCentre_RequireMitigatingAction,
        IncidentSystemCallCentre_StatusUpdate,
        IncidentSystemCallCentre_Impact
    }
    #endregion

    #region Enum for Incident - System (IT System)
    public enum IncidentSystemITSystemInputKey
    {
        IncidentSystemITSystem_AffectedSystem,
        IncidentSystemITSystem_AffectedArea
    }
    #endregion

    #region Enum for Incident - System (Network Connectivity)
    public enum IncidentSystemNetworkConnectivityInputKey
    {
        IncidentSystemNetworkConnectivity_AffectedArea
    }
    #endregion

    #region Enum for Incident - System (OT System)
    public enum IncidentSystemOTSystemInputKey
    {
        IncidentSystemOTSystem_AffectedSystem
    }
    #endregion

    #region Enum for Incident - Environment (Air Emission)
    public enum IncidentEnvironmentAirEmissionInputKey
    {
        IncidentEnvironmentAirEmission_Location,
        IncidentEnvironmentAirEmission_SourceOfInformation
    }
    #endregion

    #region Enum for Incident - Environment (Leakage)
    public enum IncidentEnvironmentLeakageTypeOfLeakage
    {
        Fuel,
        Lube_Oil,
        Ammonia,
        Chlorine,
        Acid,
        Alkaline,
        Others_9please_specify0
    }

    public enum IncidentEnvironmentLeakageAffectedArea
    {
        Within_CEM_premise,
        Outside_CEM_premise
    }

    public enum IncidentEnvironmentLeakageInputKey
    {
        IncidentEnvironmentLeakage_SourceOfInformation,
        IncidentEnvironmentLeakage_TypeOfLeakage,
        IncidentEnvironmentLeakage_AffectedArea
    }
    #endregion

    #region Enum for Incident - Quality (Corporate Image)
    public enum IncidentQualityCorporateImagePossibleCause
    {
        Others_9please_specify0
    }

    public enum IncidentQualityCorporateImageImpact
    {
        Others_9please_specify0
    }

    public enum IncidentQualityCorporateImageInputKey
    {
        IncidentQualityCorporateImage_PossibleCause,
        IncidentQualityCorporateImage_Impact,
        IncidentQualityCorporateImage_StatusUpdate
    }
    #endregion

    #region Enum for Incident - Quality (Generation)
    public enum IncidentQualityGenerationInputKey
    {
        IncidentQualityGeneration_NameOfPowerGenerator,
        IncidentQualityGeneration_IsCEMNetworkBeingAffected
    }
    #endregion

    #region Enum for Incident - OHS
    public enum IncidentOHSInputKey
    {
        IncidentOHS_PossibleCause,
        IncidentOHS_OHSType,
        IncidentOHS_NatureOfInjury,
        IncidentOHS_Treatment
    }

    public enum IncidentOHSPossibleCause
    {
        Others_9please_specify0
    }

    public enum IncidentOHSNatureOfInjury
    {
        Others_9please_specify0
    }

    public enum IncidentOHSTreatment
    {
        Others_9please_specify0
    }
    #endregion

    #region Enum for Incident - Quality Network
    public enum IncidentQualityNetworkInputKey
    {
        IncidentQualityNetwork_LossInterconnection,
        IncidentQualityNetwork_LossTransmission,
        IncidentQualityNetwork_OutageLevel
    }
    #endregion
}