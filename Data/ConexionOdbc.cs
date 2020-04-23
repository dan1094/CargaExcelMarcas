
using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;


namespace Data
{
    public class ConexionOdbc : IDisposable
    {
        private OdbcConnection OdbcConnnection = null;

        virtual protected string ConnectionString()
        {
            return ConfigurationManager.AppSettings["appConnectionStringOdbc"];
        }

        protected OdbcConnection Conn
        {
            get
            {
                return OdbcConnnection;
            }
        }

        protected void open_Conn()
        {
            if (OdbcConnnection == null)
            {
                OdbcConnnection = new OdbcConnection(ConnectionString());
                OdbcConnnection.Open();
            }
            int i = 0;
            while (OdbcConnnection.State != ConnectionState.Open)
            {
                if (i < 5)
                {
                    try
                    {
                        OdbcConnnection.Open();
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
                OdbcConnnection.Close();
                OdbcConnnection.Dispose();
                OdbcConnnection = null;
            }
            catch
            { }
        }


        /// <summary>
        /// Open Connection command
        /// </summary>
        /// <param name="strSPName"></param>
        /// <returns></returns>
        protected OdbcCommand GetOdbcCommandInstance(string comando)
        {
            open_Conn();
            OdbcCommand odbccommand = new OdbcCommand(comando, Conn);
            odbccommand.CommandType = CommandType.Text;
            odbccommand.CommandTimeout = 1800;
            return odbccommand;
        }

        protected void DisposeCommand(OdbcCommand odbccommand)
        {
            odbccommand.Dispose();
            Close_Conn();
        }
        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }

    }
}
