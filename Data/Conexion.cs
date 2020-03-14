using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Data
{
    public class Conexion : IDisposable
    {
        private SqlConnection _sqlconnnection = null;

        virtual protected string ConnectionString()
        {
            return ConfigurationManager.AppSettings["appConnectionString"];
        }
                
        protected SqlConnection Conn
        {
            get
            {
                return _sqlconnnection;
            }
        }

        protected void open_Conn()
        {
            if (_sqlconnnection == null)
            {
                _sqlconnnection = new SqlConnection(ConnectionString());
                _sqlconnnection.Open();
            }
            int i = 0;
            while (_sqlconnnection.State != ConnectionState.Open)
            {
                if (i < 5)
                {
                    try
                    {
                        _sqlconnnection.Open();
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
        }

        protected void Close_Conn()
        {
            try
            {
                _sqlconnnection.Close();
                _sqlconnnection.Dispose();
                _sqlconnnection = null;
            }
            catch
            { }
        }


        /// <summary>
        /// Open Connection and Get the Paramenters of the SP
        /// </summary>
        /// <param name="strSPName"></param>
        /// <returns></returns>
        protected SqlCommand GetSqlCommandInstance(string strSPName)
        {
            open_Conn();
            System.Data.SqlClient.SqlCommand sqlcommand = new SqlCommand(strSPName, Conn);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            sqlcommand.CommandTimeout = 1800;
            return sqlcommand;
        }

        protected object ScalarSQLCommand(SqlCommand sqlcommand)
        {
            object ret = sqlcommand.ExecuteScalar();
            DisposeCommand(sqlcommand);
            return ret;
        }

        protected int ReturnValueSQLCommand(SqlCommand sqlcommand)
        {
            int returnValue = sqlcommand.ExecuteNonQuery();

            DisposeCommand(sqlcommand);

            return returnValue;
        }

        protected void DisposeCommand(SqlCommand sqlcommand)
        {
            sqlcommand.Dispose();
            Close_Conn();
        }

        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }

    }
}
