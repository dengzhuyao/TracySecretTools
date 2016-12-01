using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TracySecretTool.EF;

namespace TracySecretTool.Server
{
    public class TMsg_CodeBLL
    {
        public int Add(TMsg_Code entity)
        {
            using (TracySecretToolEntities db = new TracySecretToolEntities())
            {
                db.TMsg_Code.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }
        public string GetNum(string list)
        {
            using (TracySecretToolEntities db = new TracySecretToolEntities())
            {
                string temp = list.Length > 6 ? list.Substring(0, 6) : list;
                var entity = db.TMsg_Code.FirstOrDefault(p => p.List.Contains(temp));
                if (entity != null)
                {
                    return entity.Val;
                }
                else
                {
                    return "-1";
                }
            }
        }
    }
}
