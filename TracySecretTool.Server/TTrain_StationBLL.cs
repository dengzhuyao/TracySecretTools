using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TracySecretTool.EF;


namespace TracySecretTool.Server
{
    public class TTrain_StationBLL
    {
        public int Add(TTrain_Station entity)
        {
            using (TracySecretToolEntities db = new TracySecretToolEntities())
            {
                db.TTrain_Station.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }

        public TTrain_Station GetStationByName(string stationName)
        {
            using (TracySecretToolEntities db = new TracySecretToolEntities())
            {
                return db.TTrain_Station.SingleOrDefault(p => p.StationName == stationName);
            }
        }
    }
}
