using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointAppProdDeployment_TESTWeb.Models
{
    public static class Model1Extensions
    {

        public static void Log(this Model1 me, string message)
        {
            me.AuditLogEntries.Add(new AuditLogEntry
            {
                Date = DateTime.Now,
                Message = message
            });
            me.SaveChanges();
        }


        public static void Log(this Model1 me, String format, params object[] args)
        {
            Model1Extensions.Log(me, string.Format(format, args));
        }
    }
}