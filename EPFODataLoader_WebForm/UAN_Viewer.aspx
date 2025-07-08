<%@ Page Title="UAN Excluded List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UAN_Viewer.aspx.cs" Inherits="EPFODataLoader_WebForm.UAN_Viewer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Your UAN Page for EPS wage exclusion. UAN should be part of this list if you're planning to use the age 58 and above checkbox<i>age 58 and above checkbox</i> 
        for fixing the wages</h3>
   
    <div class="jumbotron" style="overflow: scroll">
        <asp:Literal ID="literalMsg" runat="server"></asp:Literal> <br />
    </div>

   <div class="jumbotron" style="overflow: scroll">
        <h1>Add more UANs to List</h1>
        <asp:TextBox ID="txtBoxInput" runat="server" Width="200px"></asp:TextBox>
        <asp:Button ID="btnAddUANs" runat="server" Text="Add UAN's" OnClick="AddUANs" Width="200px"/>
    </div>


</asp:Content>