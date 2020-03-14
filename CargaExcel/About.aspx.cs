using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CargaExcel
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string fileLocation = Server.MapPath("~/App_Data/" + "test.xls");
            //string connectionString = @"";
            //connectionString = "Provider =Microsoft.Jet.OLEDB.4.0;Data Source=" +
            //        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            ////connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            ////          fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            //OleDbConnection con = new OleDbConnection(connectionString);

            //con.Open();

            //con.Close();
        }

        protected void btnGuardarArchivo_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=CO1P8S\DEV_03;Initial Catalog=test;Persist Security Info=True;User ID=adm;Password=adm";
            if (uplFile.HasFile)
            {
                string fileName = Path.GetFileName(uplFile.PostedFile.FileName);
                string fileExtension = Path.GetExtension(uplFile.PostedFile.FileName);
                string fileLocation = Server.MapPath("~/App_Data/" + fileName);
                uplFile.SaveAs(fileLocation);
                if (fileExtension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                      fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                      fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                OleDbConnection con = new OleDbConnection(connectionString);
                con.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                DataTable dtExcelRecords = new DataTable();
                

                DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dtExcelRecords);
                grdTablaExcel.DataSource = dtExcelRecords;
                grdTablaExcel.DataBind();
            }
        }
    }
}
