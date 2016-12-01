using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TracySecretTool.Dapper
{
    public static class DbHelper
    {
        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static bool Execute(string sql, object param = null, DbContext dbContext = null)
        {
            dbContext = dbContext ?? new DbContext();
            return dbContext.Execute(sql, param);
        }

        /// <summary>
        /// 执行并获取第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(string sql, object param = null, DbContext dbContext = null)
        {
            dbContext = dbContext ?? new DbContext();
            return dbContext.ExecuteScalar<T>(sql, param);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string sql, object param = null, DbContext dbContext = null)
        {
            dbContext = dbContext ?? new DbContext();
            return dbContext.GetList<T>(sql, param);
        }

        /// <summary>
        /// 查询唯一的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static T Query<T>(string sql, object param = null, DbContext dbContext = null)
        {
            dbContext = dbContext ?? new DbContext();
            return dbContext.Query<T>(sql, param);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, object param = null, DbContext dbContext = null)
        {
            dbContext = dbContext ?? new DbContext();
            return dbContext.GetDataTable(sql, GetParas(param));
        }

        /// <summary>
        /// 获取sql参数的辅助方法
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        private static SqlParameter[] GetParas(object paras)
        {
            if (paras != null)
            {
                List<SqlParameter> list = new List<SqlParameter>();
                Type t = paras.GetType();
                foreach (PropertyInfo pi in t.GetProperties())
                {

                    string name = pi.Name;
                    object val = pi.GetValue(paras, null);
                    list.Add(new SqlParameter() { ParameterName = name, Value = val });
                }
                return list.ToArray();
            }
            else
            {
                return new SqlParameter[0];
            }
        }
    }
}
