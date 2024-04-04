using System;
using System.Data;
using System.IO;
using System.Web.UI;
using BAL;

namespace EPFODataLoader_WebForm
{
    public partial class Contact : Page
    {
        bool isHigherWageLimited = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            isHigherWageLimited = chkToggleHigherWages.Checked;
        }

        protected void DownloadTxtData(System.Object sender, System.EventArgs e)
        {
            string serverStorageLocatn = "~/MyFolder/";

            if (CSVFileUpload_Id.HasFile)
            {
                string ext = Path.GetExtension(CSVFileUpload_Id.FileName).ToLower();
                var fileName = CSVFileUpload_Id.FileName;

                //getting the path of the file   
                CSVFileUpload_Id.SaveAs(Server.MapPath(serverStorageLocatn + CSVFileUpload_Id.FileName));
                string path = Server.MapPath(serverStorageLocatn + CSVFileUpload_Id.FileName);

                DataTable dt = new DataTable();
                dt = GetDT(path);

                //set data in dataTable before passing in Utilities
                var result2 = ExcelUtilities.ReadCSVFile(dt, isHigherWageLimited);

                //DownloadTxt(fileName);
                //var result = ExcelUtilities.DumpDBFToTxt(dt).ToString();
                string txtFilePath = "_" + fileName.Replace("csv", "txt");
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + txtFilePath);
                Response.AddHeader("content-type", "text/plain");

                using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                {
                    writer.Write(result2);
                }
                Response.End();
            }
            else
            {
                //Label1.Text = "No File Uploaded.";
            }
        }

        private DataTable GetDT(string fileName)
        {
            string[] Lines = File.ReadAllLines(fileName);
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);
            DataTable dt = new DataTable();
            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
                dt.Columns.Add(Fields[i].ToLower(), typeof(string));
            DataRow Row;
            for (int i = 1; i < Lines.GetLength(0); i++)
            {
                Fields = Lines[i].Split(new char[] { ',' });
                Row = dt.NewRow();
                for (int f = 0; f < Cols; f++)
                    Row[f] = Fields[f];
                dt.Rows.Add(Row);
            }
            return dt;
        }
    }
}