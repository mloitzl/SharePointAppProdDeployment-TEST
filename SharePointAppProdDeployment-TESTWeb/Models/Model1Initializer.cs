using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePointAppProdDeployment_TESTWeb.Models
{
    public class Model1Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Model1>
    {

        protected override void Seed(Model1 context)
        {
            context.AuditLogEntries.Add(new AuditLogEntry
            {
                Message = "Initializer",
                Date = DateTime.Now
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}