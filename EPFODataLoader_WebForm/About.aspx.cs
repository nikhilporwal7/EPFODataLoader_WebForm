using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EPFODataLoader_WebForm
{
    public partial class About : Page
    {
        string fileName = string.Empty;
        string result = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetInitialRow();
            }
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));

            dr = dt.NewRow();

            dr["Column1"] = string.Empty;
            dr["Column2"] = string.Empty;
            dr["Column3"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
            EmployeeEntryGrid.DataSource = dt;
            EmployeeEntryGrid.DataBind();
        }

        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values

                        TextBox box1 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[0].FindControl("txtUAN_No");

                        TextBox box2 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[1].FindControl("txtEmp_Name");

                        TextBox box3 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[2].FindControl("txtEmp_EPFWage");

                        drCurrentRow = dtCurrentTable.NewRow();

                        //drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;
                    EmployeeEntryGrid.DataSource = dtCurrentTable;
                    EmployeeEntryGrid.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks

            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values

                        TextBox box1 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[0].FindControl("txtUAN_No");
                        TextBox box2 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[1].FindControl("txtEmp_Name");
                        TextBox box3 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[2].FindControl("txtEmp_EPFWage");

                        box1.Text = dtCurrentTable.Rows[i]["Column1"].ToString();
                        box2.Text = dtCurrentTable.Rows[i]["Column2"].ToString();
                        box3.Text = dtCurrentTable.Rows[i]["Column3"].ToString();
                        rowIndex++;
                    }
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            if (dtCurrentTable.Rows.Count > 0)
            {
                result = DumpDataTableToTxt(dtCurrentTable).ToString();


                string txtFilePath = "abc.txt"; 
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + txtFilePath);
                Response.AddHeader("content-type", "text/plain");

                using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                {
                    writer.WriteLine(result);
                }
                Response.End();
            }
            else
            {
                Response.Write("Please enter data to generate text file.");
            }
        }

        /// <summary>
        /// src: https://stackoverflow.com/questions/4959722/c-sharp-datatable-to-csv
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="csvFile"></param>
        private static StringBuilder DumpDataTableToTxt(DataTable dt)
        {
            var result = new StringBuilder();

            dt.Rows[dt.Rows.Count - 1].Delete();

            dt.Columns.Add("Wages2");
            dt.Columns.Add("Wages3");
            dt.Columns.Add("Wages4");
            dt.Columns.Add("12_Per");
            dt.Columns.Add("8_33");
            dt.Columns.Add("3_67");
            dt.Columns.Add("NCPDays");
            dt.Columns.Add("Totaldays");

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string temp = string.Empty;
                    if (i < 3)
                    {
                        temp = row[i].ToString();
                    }
                    else if(i == 3 || i == 4 || i== 5) //wages2, wages3, wages4
                    {
                        temp = row[2].ToString();
                    }
                    else if(i == 6) //12%
                    {
                        var wages = Convert.ToInt32(row[2]);
                        var calc = (int)Math.Round((double)wages * 12 / 100, 0);
                        temp = calc.ToString();
                    }
                    else if (i == 7) //8.33%
                    {
                        var wages = Convert.ToInt32(row[2]);
                        var calc = (int)Math.Round((double)wages * 8.33 / 100, 0);
                        temp = calc.ToString();
                    }
                    else if (i == 8) //3.67%
                    {
                        var wages = Convert.ToInt32(row[2]);
                        var calc = (int)Math.Round((double)wages * 12 / 100, 0) - (int)Math.Round((double)wages * 8.33 / 100, 0);
                        temp = calc.ToString();
                    }
                    else if (i==9 || i == 10)
                    {
                        temp = "0";
                    }

                    result.Append(temp);
                    result.Append(i == dt.Columns.Count - 1 ? Environment.NewLine : "#~#");        //https://stackoverflow.com/questions/2617752/newline-character-in-stringbuilder
                }
                //result.Append(Environment.NewLine);
            }
            return result;
            //File.WriteAllText(csvFile, result.ToString());
        }

        //calculate the 3 amounts  
        //calculate PF -12%
        //calculate 8.33%
        //


    }
}