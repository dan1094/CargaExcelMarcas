using Rule;
using System;
using System.IO;
using System.Web.UI;

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
                        grvMarcas.DataSource = rule.ConsultarMarcas(string.Empty);
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
            try
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
                            grvMarcas.DataSource = rule.ConsultarMarcas(string.Empty);
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnLimpiarTabla_Click(object sender, EventArgs e)
        {
            try
            {
                using (MarcasRule rule = new MarcasRule())
                {
                    rule.BorrarTablaMarcas();
                    grvMarcas.DataSource = rule.ConsultarMarcas(string.Empty);
                    grvMarcas.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string name = string.Format("Reporte_{0}_{1}_{2}_{3}_{4}.pdf",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

                string filename = Server.MapPath("~/App_Data/" + name);
                using (MarcasRule rule = new MarcasRule())
                {
                    rule.ArmarPDFMarcas(rule.ConsultarMarcas(txtFltro.Text), filename);
                }
                Descargar(filename, name);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnDescargarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string name = string.Format("Reporte_{0}_{1}_{2}_{3}_{4}.xlsx",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

                string filename = Server.MapPath("~/App_Data/" + name);
                using (MarcasRule rule = new MarcasRule())
                {
                    rule.ArmarExel(rule.ConsultarMarcas(txtFltro.Text), filename);
                }
                Descargar(filename, name);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }
        private void Descargar(string ruta, string nombre)
        {
            try
            {
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.AddHeader("Content-Transfer-Encoding", "Binary");
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", nombre));
                Response.WriteFile(ruta);
                Response.End();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (MarcasRule rule = new MarcasRule())
                {
                    grvMarcas.DataSource = rule.ConsultarMarcas(txtFltro.Text);
                    grvMarcas.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnComparar_Click(object sender, EventArgs e)
        {
            try
            {
                using (MarcasRule rule = new MarcasRule())
                {
                    rule.RealizarComparacion();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script",
                    "alert('Comparacion realizada')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnDescargarComparacion_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string name = string.Format("ReporteComparacion_{0}_{1}_{2}_{3}_{4}.pdf",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

                string filename = Server.MapPath("~/App_Data/" + name);
                using (MarcasRule rule = new MarcasRule())
                {
                    rule.ArmarPDFComparacion(filename);
                }
                Descargar(filename, name);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", $"alert('{ex.Message}')", true);
            }
        }
    }
}
