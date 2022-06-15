<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteIndicadorPorServicio.aspx.cs" Inherits="GB.SUTEL.UI.Paginas.Reportes.ReporteIndicadorPorServicio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte  Indicador Por Servicio</title>
   
    <link href="../../Content/Site.css" rel="stylesheet" />
<style>
    h4, .h4 {
  font-size: 18px;
}
h4, h5, h6 {
  margin-top: 10px;
  margin-bottom: 10px;
}
h1, h2, h3, h4, h5, h6, .h1, .h2, .h3, .h4, .h5, .h6 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 500;
  line-height: 1.1;
}
body{
    margin-top: 0px;
}

.form-horizontal .control-label, .form-horizontal .radio, .form-horizontal .checkbox, .form-horizontal .radio-inline, .form-horizontal .checkbox-inline {
  padding-top: 7px;
  margin-top: 0;
  margin-bottom: 0;
}
.form-horizontal .control-label {
  text-align: right;
}

.form-group {
  margin-bottom: 15px;
}

.form-control {
  display: block;
  width: 100%;
  height: 34px;
  padding: 6px 12px;
  font-size: 14px;
  line-height: 1.428571429;
  color: #555555;
  vertical-align: middle;
  background-color: #ffffff;
  border: 1px solid #cccccc;
  border-radius: 4px;
  -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
  box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
  -webkit-transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
  transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
}

label {
  display: inline-block;
  margin-bottom: 5px;
  font-weight: bold;
}
.col-md-2 {
  width: 16.666666666666664%;
}

.col-md-10 {
  width: 83.33333333333334%;
}

.col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11 {
  float:left;  
}

 .col-sm-offset-3 {
     margin-top:5%;
    margin-left: 17%;
  }

.alert-danger {
  color: #b94a48;
  background-color: #f2dede;
  border-color: #eed3d7;
}

.alert {
  padding: 15px;
  margin-bottom: 20px;
  border: 1px solid transparent;
  border-radius: 4px;
}

.btn-success {
  color: #ffffff;
  background-color: #5cb85c;
  border-color: #4cae4c;
}
.btn {
  display: inline-block;
  padding: 6px 12px;
  margin-bottom: 0;
  font-size: 14px;
  font-weight: normal;
  line-height: 1.428571429;
  text-align: center;
  white-space: nowrap;
  vertical-align: middle;
  cursor: pointer;
  border: 1px solid transparent;
  border-radius: 4px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  -o-user-select: none;
  user-select: none;
}
</style>
      
</head>
<body>
    <h4>Reporte de Indicador por Servicio</h4>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label ID="lblServicio" runat="server" Text="Servicios" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-10">
                         <asp:DropDownList ID="ddlServicios" runat="server" AutoPostBack="true" 
                             OnSelectedIndexChanged="ddlServicios_SelectedIndexChanged" CssClass="form-control"
                             DataTextField="DesServicio" DataValueField="IdServicio" OnDataBound="ddlServicios_DataBound" ></asp:DropDownList>
                       
                    </div>
                </div>
                <br />
                 <br />
                 <br />
                <div class="form-group">
                    <asp:Label ID="lblIndicador" runat="server" Text="Indicador" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-10">

                         <asp:DropDownList ID="ddlIndicador" runat="server" AutoPostBack="true" CssClass="form-control"
                             DataTextField="NombreIndicador" DataValueField="IdIndicador" OnDataBound="ddlIndicador_DataBound" ></asp:DropDownList>

                        
                    </div>
                </div>

               
            </div>
            

            <div class="form-group">
                
                <div class="col-sm-offset-3 col-sm-9">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-success" Width="72px" />
                </div>
            </div>
             <asp:Panel runat="server"   id="divMensajeError" CssClass="alert alert-danger" Visible="false">
                    <strong>Error!</strong>
                    <div id="idMensajeErrorCuerpo"><asp:Label ID="lblTextoError" runat="server" Text=""></asp:Label></div>
            </asp:Panel>
        </div>
        <asp:Panel ID="pnlResultado" runat="server" Visible="false" Height="500px">
                <div style="height: 95%; overflow-y: scroll;">
                    <rsweb:ReportViewer ID="rptReporte" Visible="true" runat="server" Width="100%" ShowParameterPrompts="false" AsyncRendering="true" Height="90%"></rsweb:ReportViewer>

                </div>
        </asp:Panel>
    </form>
</body>
</html>