﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointAppProdDeployment_TESTWeb.Models
{
    public class AuditLogEntry
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}