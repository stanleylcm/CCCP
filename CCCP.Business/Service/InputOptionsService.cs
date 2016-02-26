﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;

namespace CCCP.Business.Service
{
    public class InputOptionsService
    {
        public static List<string> GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey key)
        {
            string keyStr = key.ToString();
            List<string> result = GetInputOptions(keyStr);

            return result;
        }

        public static List<string> GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey key)
        {
            string keyStr = key.ToString();
            List<string> result = GetInputOptions(keyStr);  

            return result;
        }

        public static List<string> GetInputOptions(string key)
        {
            List<string> result = new List<string>();

            // get options from DB table InputOption
            CCCPDbContext db = new CCCPDbContext();
            result = (from io in db.InputOption
                      where io.InputKey.Equals(key)
                      orderby io.SortingOrder
                      select io.Value).ToList();

            return result;
        }
    }
}
