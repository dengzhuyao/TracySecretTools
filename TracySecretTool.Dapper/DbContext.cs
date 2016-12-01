using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;

namespace TracySecretTool.Dapper
{
    public class DbContext : IDisposable
    {
        /// <summary>
        /// Connection对象
        /// </summary>
        private SqlConnection Connection { get; set; }
        /// <summary>
        /// 事务对象
        /// </summary>
        private SqlTransaction Tran { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string ConnString { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="useTransaction">是否使用事务</param>
        public DbContext(bool useTransaction=false)
            : this(null, useTransaction)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <param name="useTransaction">是否使用事务</param>
        public DbContext(string connString, bool useTransaction)
        {
            this.ConnString = connString ?? staticConnString;
            this.Connection = new SqlConnection(this.ConnString);
            this.Connection.Open();
            this.Tran = useTransaction ? this.Connection.BeginTransaction() : null;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public bool CommitTransaction()
        {
            if (this.Tran.Connection != null)
            {
                this.Tran.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Tran = null;
            this.Connection.Close();
            this.Connection.Dispose();
        }
        #region Execute
        public bool Execute(string sql, object param = null)
        {
            if (this.Tran != null)
            {
                try
                {
                    return this.Connection.Execute(sql, param, this.Tran) > 0;
                }
                catch (Exception ex)
                {
                    this.Tran.Rollback();
                    this.Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    return false;
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(this.ConnString))
                {
                    conn.Open();
                    return conn.Execute(sql, param) > 0;
                }
            }
        }
        #endregion

        #region ExecuteScalar
        public T ExecuteScalar<T>(string sql, object param = null)
        {
            if (this.Tran != null)
            {
                try
                {
                    return this.Connection.ExecuteScalar<T>(sql, param, this.Tran);
                }
                catch (Exception ex)
                {
                    this.Tran.Rollback();
                    this.Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    return default(T);
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(this.ConnString))
                {
                    conn.Open();
                    return conn.ExecuteScalar<T>(sql, param);
                }
            }
        }
        #endregion

        #region GetList
        public List<T> GetList<T>(string sql, object param = null)
        {
            if (this.Tran != null)
            {
                try
                {
                    return this.Connection.Query<T>(sql, param, this.Tran).ToList();
                }
                catch (Exception ex)
                {
                    this.Tran.Rollback();
                    this.Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    return default(List<T>);
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(this.ConnString))
                {
                    conn.Open();
                    return conn.Query<T>(sql, param).ToList();
                }
            }
        }
        #endregion

        #region Query
        public T Query<T>(string sql, object param = null)
        {
            if (this.Tran != null)
            {
                try
                {
                    return this.Connection.QuerySingleOrDefault<T>(sql, param, this.Tran);
                }
                catch (Exception ex)
                {
                    this.Tran.Rollback();
                    this.Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    return default(T);
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(this.ConnString))
                {
                    conn.Open();
                    return conn.QuerySingleOrDefault<T>(sql, param);
                }
            }
        }
        #endregion

        #region GetDataTable
        public DataTable GetDataTable(string sql, SqlParameter[] paras = null)
        {
            if (this.Tran != null)
            {
                try
                {
                    DataSet ds = new DataSet("ds1");
                    using (SqlDataAdapter ada = new SqlDataAdapter(sql, this.Connection))
                    {
                        ada.SelectCommand.Transaction = this.Tran;
                        ada.SelectCommand.Parameters.AddRange(paras);//传参
                        ada.Fill(ds);
                    }
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    this.Tran.Rollback();
                    this.Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    return default(DataTable);
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(this.ConnString))
                {
                    conn.Open();
                    DataSet ds = new DataSet("ds1");
                    using (SqlDataAdapter ada = new SqlDataAdapter(sql, this.Connection))
                    {
                        ada.SelectCommand.Parameters.AddRange(paras);//传参
                        ada.Fill(ds);
                    }
                    return ds.Tables[0];
                }
            }
        }
        #endregion






        #region 静态
        /// <summary>
        /// 静态连接字符串
        /// </summary>
        private static string staticConnString { get; set; }
        /// <summary>
        /// 初始化静态的连接字符串
        /// </summary>
        /// <param name="connString"></param>
        public static void RegisterConnString(string connString)
        {
            staticConnString = connString;
        }
        #endregion
    }
}
