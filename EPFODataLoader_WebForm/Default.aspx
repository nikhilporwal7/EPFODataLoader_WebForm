﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EPFODataLoader_WebForm._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="overflow: scroll">
        <h1>Display Uploaded data</h1>
        <asp:Label runat="server" ID="helloWorldLabel"></asp:Label>
        <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
        <asp:Button ID="greetButton" runat="server" OnClick="GreetBotton_Click" Text="Submit" class="btn btn-primary btn-lg" Width="123px" />
        <h2>Select DBF File</h2>
        <asp:FileUpload ID="FileUpload1" runat="server" Width="138px" />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <asp:GridView ID="gvExcelFile" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:Button ID="btn_Download" runat="server" BackColor="#0033CC" Font-Bold="True" ForeColor="White" Text="Download" OnClick="DownloadTxt" Width="112px" />
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Select Month</h2>
            <asp:DropDownList runat="server" ID="Month" AutoPostBack="true" OnSelectedIndexChanged="GreetList_SelectedIndexChanged">
                <%-- <asp:ListItem Value="no one">No one</asp:ListItem>
                <asp:ListItem Value="world">World</asp:ListItem>
                <asp:ListItem Value="universe">Universe</asp:ListItem>--%>
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
            <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>

    <asp:RegularExpressionValidator
        ID="FileUpLoadValidator" runat="server"
        ErrorMessage="Upload DBF file only."
        ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.xls|.csv|.xlsx|dbf|DBF)$"
        ControlToValidate="FileUpload1">  
    </asp:RegularExpressionValidator>
</asp:Content>
