using System;
using System.Collections.Generic;
using Rule;
using Entities;
using System.Web.UI;
using System.IO;

namespace CargaExcel
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                using (MarcasRule rule = new MarcasRule())
                {
                    try
                    {
                        grvMarcas.DataSource = rule.Consultar(string.Empty);
                        grvMarcas.DataBind();
                    }
                    catch
                    {
                    }

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/App_Data/"));
                    foreach (string filePath in filePaths)
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch { }

                }
            }
            uplFile.Dispose();
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            if (uplFile.HasFile)
            {
                if (Path.GetExtension(uplFile.PostedFile.FileName).Equals(".xlsx"))
                {
                    string fileName = Path.GetFileName(uplFile.PostedFile.FileName);
                    string fileLocation = Server.MapPath("~/App_Data/" + fileName);
                    uplFile.SaveAs(fileLocation);

                    using (MarcasRule rule = new MarcasRule())
                    {
                        rule.LeerExcel(fileLocation);
                        grvMarcas.DataSource = rule.Consultar(string.Empty);
                        grvMarcas.DataBind();
                    }
                    //uplFile.Dispose();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Archivo Cargado exitosamente')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "Clear()", true);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + fileName + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('El archivo debe ser .xlsx')", true);
                }
            }
        }

        protected void btnLimpiarTabla_Click(object sender, EventArgs e)
        {
            using (MarcasRule rule = new MarcasRule())
            {
                rule.BorrarTablaMarcas();
                grvMarcas.DataSource = rule.Consultar(string.Empty);
                grvMarcas.DataBind();
            }
        }

        protected void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string name = string.Format("Reporte_{0}_{1}_{2}_{3}_{4}.pdf",
                dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

            string filename = Server.MapPath("~/App_Data/" + name);
            using (MarcasRule rule = new MarcasRule())
            {
                rule.ArmarPDF(rule.Consultar(txtFltro.Text), filename);
            }
            Descargar(filename, name);
        }

        protected void btnDescargarExcel_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string name = string.Format("Reporte_{0}_{1}_{2}_{3}_{4}.xlsx",
                dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

            string filename = Server.MapPath("~/App_Data/" + name);
            using (MarcasRule rule = new MarcasRule())
            {
                rule.ArmarExel(rule.Consultar(txtFltro.Text), filename);
            }
            Descargar(filename, name);
        }
        private void Descargar(string ruta, string nombre)
        {
            Response.AddHeader("Content-Type", "application/octet-stream");
            Response.AddHeader("Content-Transfer-Encoding", "Binary");
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", nombre));
            Response.WriteFile(ruta);
            Response.End();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            using (MarcasRule rule = new MarcasRule())
            {
                grvMarcas.DataSource = rule.Consultar(txtFltro.Text);
                grvMarcas.DataBind();
            }
        }
    }
}
