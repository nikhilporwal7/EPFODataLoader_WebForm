<%@ Page Title="EPFO Challan" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="EPFODataLoader_WebForm.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Enter the data here to Generate Adhoc Challans</h3>
    <%-- Add a checkbox here to handle 15k above scenario --%>
    <asp:CheckBox ID="chkToggleHigherWages" Text="Limit Wages to 15K"  runat="server"/>
    <asp:GridView ID="EmployeeEntryGrid" runat="server" AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" ShowFooter="true">
        <Columns>
            <asp:TemplateField HeaderText="UAN Number">
                <ItemTemplate>
                    <asp:TextBox ID="txtUAN_No" Style="width: 100%" TextMode="Number" MaxLength="10" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:TextBox ID="txtEmp_Name" Style="width: 100%" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="EPF Wages">
                <ItemTemplate>
                    <asp:TextBox ID="txtEmp_EPFWage" Style="width: 100%" TextMode="Number" runat="server"></asp:TextBox>
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Right" />
                <FooterTemplate>
                    <asp:Button ID="btnAdd" runat="server" Text="Add Row" OnClick="btnAddNewRow_Click" width="100%"/>
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Challan" OnClick="btnGenerate_Click" Width="100%" />
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
</asp:Content>
