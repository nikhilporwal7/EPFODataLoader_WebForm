using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
namespace EPFODataLoader_WebForm
{
    public partial class _Default : Page
    {
        public static int monthIndex;
        protected void Page_Load(object sender, EventArgs e)
        {
            HelloWorldLabel.Text = "Hello ";

            foreach (BAL.MonthOption mo in Enum.GetValues(typeof(BAL.MonthOption)))
            {
                ListItem item = new ListItem(Enum.GetName(typeof(BAL.MonthOption), mo), mo.ToString());
                Month.Items.Add(item);
            }
        }

        protected void GreetBotton_Click(object sender, EventArgs e)
        {
            HelloWorldLabel.Text = $"Hello {TextInput.Text}" ;
            TextInput.Text = string.Empty;

            if (FileUpload1.HasFile)
            {
                var ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                //getting the path of the file   
                FileUpload1.SaveAs(Server.MapPath("~/MyFolder/" + FileUpload1.FileName));
                string path = Server.MapPath("~/MyFolder/" + FileUpload1.FileName);
                DataTable dt = new DataTable();
                dt = ExcelUtilities.ReadDBFfile(path, monthIndex);

                //return something to UI
                gvExcelFile.DataSource = dt;
                gvExcelFile.DataBind();

                Label1.Text = $"File Uploaded: {FileUpload1.FileName}" ;

                //ExcelUtilities.DumpDBFToTxt(dt, txtFilePath);
            }
            else
            {
                Label1.Text = "No File Uploaded.";
            }
        }

        protected void GreetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HelloWorldLabel.Text = $"Hello { Month.SelectedValue }";
            monthIndex = Month.SelectedIndex;
        }
    }
}
