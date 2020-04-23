<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CargaExcel._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .mydatagrid {
            width: 80%;
            border: solid 2px black;
            min-width: 80%;
        }

        .header {
            background-color: #000;
            font-family: Arial;
            color: White;
            height: 25px;
            text-align: center;
            font-size: 16px;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 14px;
            color: #000;
            min-height: 25px;
            text-align: left;
        }

            .rows:hover {
                background-color: #5badff;
                color: #fff;
            }

        .mydatagrid a /** FOR THE PAGING ICONS  **/ {
            background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: #fff;
            text-decoration: none;
            font-weight: bold;
        }

            .mydatagrid a:hover /** FOR THE PAGING ICONS  HOVER STYLES**/ {
                background-color: #000;
                color: #fff;
            }

        .mydatagrid span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            background-color: #fff;
            color: #000;
            padding: 5px 5px 5px 5px;
        }

        .pager {
            background-color: #5badff;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .mydatagrid td {
            padding: 5px;
        }

        .mydatagrid th {
            padding: 5px;
        }
    </style>
    <div class="jumbotron">
        <h1>Procesar Archivo</h1>
        <asp:FileUpload ID="uplFile" runat="server" BackColor="DodgerBlue" ForeColor="AliceBlue" AllowMultiple="false"
            BorderWidth="3px" accept="xlsx" />
        <br />
        <asp:Button ID="btnCargar" class="btn btn-primary btn-lg" runat="server" Text="Cargar Excel" OnClick="btnCargar_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnLimpiarTabla" class="btn btn-primary btn-lg" BackColor="Tomato" runat="server" Text="Limpiar Tabla" OnClick="btnLimpiarTabla_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnDescargarPDF" class="btn btn-primary btn-lg" runat="server" Text="Descargar PDF" OnClick="btnDescargarPDF_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnDescargarExcel" class="btn btn-primary btn-lg" runat="server" Text="Descargar Excel" OnClick="btnDescargarExcel_Click" />

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        &nbsp;&nbsp;
        <asp:Button ID="btnFiltrar" class="btn btn-primary btn-lg" runat="server" Text="Buscar" OnClick="btnFiltrar_Click" />
        &nbsp;
        <asp:TextBox ID="txtFltro" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btnComparar" class="btn btn-primary btn-lg" runat="server" Text="Comparar" OnClick="btnComparar_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnDescargarComparacion" class="btn btn-primary btn-lg" runat="server"
            Text="Descargar Comparacion" OnClick="btnDescargarComparacion_Click" />
        <br />
        <br />
        <br />
        <asp:Panel runat="server" ScrollBars="Auto">
            <asp:GridView ID="grvMarcas" runat="server" AutoGenerateColumns="False" CssClass="mydatagrid"
                PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Marca" HeaderText="Marca"></asp:BoundField>
                    <asp:BoundField DataField="Fonema" HeaderText="Fonema"></asp:BoundField>
                    <asp:BoundField DataField="Nogaceta" HeaderText="No gaceta"></asp:BoundField>
                    <asp:BoundField DataField="FgacetaString" HeaderText="Fecha Gaceta"></asp:BoundField>
                    <asp:BoundField DataField="Codigo_clase" HeaderText="Cod. Clase"></asp:BoundField>
                    <asp:BoundField DataField="FpresentaString" HeaderText="Fecha Presenta"></asp:BoundField>
                    <asp:BoundField DataField="Nopub" HeaderText="Nopub"></asp:BoundField>
                    <asp:BoundField DataField="Noexp" HeaderText="Noexp"></asp:BoundField>
                    <asp:BoundField DataField="Solicitant" HeaderText="Solicitant"></asp:BoundField>
                    <asp:BoundField DataField="Codigo_pais" HeaderText="Cod. País"></asp:BoundField>
                    <asp:BoundField DataField="Apoderado" HeaderText="Apoderado"></asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                    <asp:BoundField DataField="FdigitacioString" HeaderText="Fecha digitacion"></asp:BoundField>

                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function Clear() {
            //Reference the FileUpload and get its Id and Name.
            var fileUpload = document.getElementById("<%=uplFile.ClientID %>");
            var id = fileUpload.id;
            var name = fileUpload.name;

            //Create a new FileUpload element.
            var newFileUpload = document.createElement("INPUT");
            newFileUpload.type = "FILE";

            //Append it next to the original FileUpload.
            fileUpload.parentNode.insertBefore(newFileUpload, fileUpload.nextSibling);

            //Remove the original FileUpload.
            fileUpload.parentNode.removeChild(fileUpload);

            //Set the Id and Name to the new FileUpload.
            newFileUpload.id = id;
            newFileUpload.name = name;
            return false;
        }


    </script>

</asp:Content>
