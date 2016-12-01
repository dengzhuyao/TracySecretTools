using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TracySecretTool.EF;

namespace TracySecretTool.Server
{
    public class TTotal_ConfigBLL
    {
        public int Add(TTotal_Config entity)
        {
            using (TracySecretToolEntities db = new TracySecretToolEntities())
            {
                db.TTotal_Config.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }
        public string GetConfigByName(string name)
        {
            using (TracySecretToolEntities db = new TracySecretToolEntities())
            {
                var entity = db.TTotal_Config.SingleOrDefault(p => p.Name == name);
                return entity == null ? null : entity.Value;
            }
        }
    }
}
