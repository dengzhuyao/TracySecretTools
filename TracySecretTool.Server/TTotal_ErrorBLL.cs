using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TracySecretTool.EF;

namespace TracySecretTool.Server
{
   public class TTotal_ErrorBLL
    {
       public int Add(TTotal_Error entity)
       {
           using (TracySecretToolEntities db = new TracySecretToolEntities())
           {
               db.TTotal_Error.Add(entity);
               db.SaveChanges();
               return entity.ID;
           }
       }
    }
}
