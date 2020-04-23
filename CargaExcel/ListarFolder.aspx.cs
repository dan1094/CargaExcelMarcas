using Entities;
using Rule;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CargaExcel
{
    public partial class ListarFolder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                using (FolderRule rule = new FolderRule())
                {
                    try
                    {
                        grvFolder.DataSource = rule.ConsultarFolder();
                        grvFolder.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }                                        
                }
            }
        }
                
        protected void btnCrearFiltrar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow item in grvFolder.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("cbxSeleccionar");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        using (FiltroRule rule = new FiltroRule() )
                        {
                            rule.InsertarFiltro(new Filtro()
                            {
                                FolderName = item.Cells[1].Text,
                                Fecha = DateTime.Now
                            });
                        }
                        // process selected record
                        //Response.Write(item.Cells[1].Text + "<br>");
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), 
                "script", "alert('Filtro creado exitosamente')", true);

        }
    }
}