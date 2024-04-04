<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="EPFODataLoader_WebForm.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your contact page.</h3>
   
    <div class="jumbotron" style="overflow: scroll">
        <asp:checkbox id="chkToggleHigherWages" type="checkbox" Text="Limit Wages to 15K" runat="server" />
        <h1>Upload CSV data</h1>
        
        <asp:FileUpload ID="CSVFileUpload_Id" runat="server" Width="138px" />

        <h1>Download processed data</h1>
        <asp:Button ID="Button1" runat="server" Text="Download Txt File" OnClick="DownloadTxtData" Width="200px"/>
    </div>

     <address>
     One Microsoft Way<br />
     Redmond, WA 98052-6399<br />
     <abbr title="Phone">P:</abbr>
     425.555.0100
 </address>
 <address>
     <strong>Support:</strong>   <a href="mailto:Support@example.com">porwalprofessionalconsultancy@gmail.com</a><br />
     <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>
 </address>
</asp:Content>
