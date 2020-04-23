<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListarFolder.aspx.cs" Inherits="CargaExcel.ListarFolder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
        <h1>Listar Folder</h1>
&nbsp;
        <%--<asp:TextBox ID="txtFltro" runat="server"></asp:TextBox>--%>
        <br />
        <asp:Panel runat="server" ScrollBars="Auto">
            <asp:GridView ID="grvFolder" runat="server" AutoGenerateColumns="False" CssClass="mydatagrid"
                PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbxSeleccionar" runat="server"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="FolderName" HeaderText="Folder"></asp:BoundField>
                    <asp:BoundField DataField="FechaString" HeaderText="Fecha"></asp:BoundField>

                </Columns>
                <HeaderStyle CssClass="header" />
                <PagerStyle CssClass="pager" />
                <RowStyle CssClass="rows" />
            </asp:GridView>
        </asp:Panel>
         <br />
        <asp:Button ID="btnCrearFiltrar" class="btn btn-primary btn-lg" runat="server" Text="Crear Filtro" OnClick="btnCrearFiltrar_Click" />
        
    </div>
    <script type="text/javascript">

</script>

</asp:Content>
