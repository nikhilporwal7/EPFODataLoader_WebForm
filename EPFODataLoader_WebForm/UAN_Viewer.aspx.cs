using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EPFODataLoader_WebForm
{
    public partial class UAN_Viewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string filePath = Server.MapPath("~/MyFolder/UAN_EPSExcluded"); // Or your file path
                                                                                //string UAN_EPS_Excluded_File_Name = "UAN_EPSExcluded";
                if (File.Exists(filePath))
                {
                    string fileContent = File.ReadAllText(filePath);

                    literalMsg.Text = fileContent.Replace(Environment.NewLine, "<br/>"); // Or Label1.Text
                }
                else
                {
                    // File does not exist, handle the case
                    Response.Write("File does not exist.");
                }
            }
        }

        protected void AddUANs(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/MyFolder/UAN_EPSExcluded");
            File.AppendAllText(filePath, txtBoxInput.Text + Environment.NewLine);
            string fileContent = File.ReadAllText(filePath);

            literalMsg.Text = fileContent.Replace(Environment.NewLine, "<br/>"); // Or Label1.Text
            txtBoxInput.Text = string.Empty;
        }
    }
}
