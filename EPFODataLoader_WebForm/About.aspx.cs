﻿using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;

namespace EPFODataLoader_WebForm
{
    public partial class About : Page
    {
        string fileName = string.Empty;
        string result = string.Empty;
        bool isHigherWageLimited = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetInitialRow();
            }
            isHigherWageLimited = chkToggleHigherWages.Checked;
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            dt.Columns.Add(new DataColumn("NCP_Days", typeof(string)));

            dr = dt.NewRow();

            dr["Column1"] = string.Empty;
            dr["Column2"] = string.Empty;
            dr["Column3"] = string.Empty;
            dr["NCP_Days"] = string.Empty;
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

                        TextBox box4 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[3].FindControl("txtNCP_Days");

                        drCurrentRow = dtCurrentTable.NewRow();

                        //drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                        dtCurrentTable.Rows[i - 1]["NCP_Days"] = box4.Text;
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
                        TextBox box4 = (TextBox)EmployeeEntryGrid.Rows[rowIndex].Cells[3].FindControl("txtNCP_Days");

                        box1.Text = dtCurrentTable.Rows[i]["Column1"].ToString();
                        box2.Text = dtCurrentTable.Rows[i]["Column2"].ToString();
                        box3.Text = dtCurrentTable.Rows[i]["Column3"].ToString();
                        box4.Text = dtCurrentTable.Rows[i]["NCP_Days"].ToString();
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

            //Validate the data
            bool validGridData = ValidateGridData(dtCurrentTable);

            if (!validGridData)
            {
                Response.Write("Please enter data to generate text file.");
            }
            else
            {
                result = DumpDataTableToTxt(dtCurrentTable, isHigherWageLimited).ToString();

                //remove the ending newline character
                if (result.EndsWith(Environment.NewLine))
                {
                    result = result.Substring(0, result.Length - Environment.NewLine.Length);
                }

                fileName = GetFileName(dtCurrentTable.Rows.Count);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.AddHeader("content-type", "text/plain");

                using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                {
                    writer.Write(result);
                }
                Response.End();
            }
        }

        //calculate the 3 amounts  
        //calculate PF -12%
        //calculate 8.33%


        #region PrivateMethods
        /// <summary>
        /// src: https://stackoverflow.com/questions/4959722/c-sharp-datatable-to-csv
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="csvFile"></param>
        private static StringBuilder DumpDataTableToTxt(DataTable dt, bool isHigherWageLimited)
        {
            var result = new StringBuilder();

            dt.Rows[dt.Rows.Count - 1].Delete();

            dt.Columns.Add("Wages2");
            dt.Columns.Add("Wages3");
            dt.Columns.Add("Wages4");
            dt.Columns.Add("12_Per");
            dt.Columns.Add("8_33");
            dt.Columns.Add("3_67");
            //dt.Columns.Add("NCPDays"); //shifted to UI
            dt.Columns.Add("REFUND_OF_ADVANCES");

            foreach (DataRow row in dt.Rows)
            {
                int Wages = 0;
                int EDLIWages = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string temp = string.Empty;
                    
                    if (i < 3)   //i == 2 //Gross Wages Wages1
                    {
                        temp = row[i].ToString();
                    }
                    else if (i == 3 || i == 4) //wages2, wages3
                    {
                        if (isHigherWageLimited)
                        {
                            temp = row[2].ToString();      //rename this to row["Column3"]
                            Wages = Convert.ToInt32(temp) > 15000 ? 15000 : Convert.ToInt32(temp);
                            temp = Wages.ToString();
                        }
                        else
                        {
                            temp = row[2].ToString();
                            Wages = Convert.ToInt32(temp);
                        }
                       
                        //future implementations
                        //i=4 EPS is zero if pension is not there or age > 60
                    }
                    else if (i == 5) //EDLI Wages  //i=5 max EDLI amount is 15k
                    {
                        EDLIWages = Wages > 15000 ? 15000 : Wages;
                        temp = EDLIWages.ToString();
                    }
                    else if (i == 6) //12%
                    {
                        var calc = (int)Math.Round((double)Wages * 12 / 100, 0);
                        temp = calc.ToString();
                    }
                    else if (i == 7) //8.33%
                    {
                        var calc = (int)Math.Round(EDLIWages * 8.33 / 100, 0);
                        temp = calc.ToString();
                    }
                    else if (i == 8) //3.67%
                    {
                        var calc = (int)Math.Round((double)Wages * 12 / 100, 0) - (int)Math.Round((double)EDLIWages * 8.33 / 100, 0);
                        temp = calc.ToString();
                    }
                    else if (i == 9)   //NCP days
                    {
                        temp = row["NCP_Days"].ToString().IsNullOrWhiteSpace() ? "0" : row["NCP_Days"].ToString();
                    }
                    else if (i == 10)
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

        private string GetFileName(int countofEmp)
        {
            return countofEmp + "Emp" + "_" + DateTime.Now.Date.ToShortDateString() + ".txt";
        }

        private bool ValidateGridData(DataTable dtCurrentTable)
        {
            if (dtCurrentTable.Rows[0].ItemArray[0].Equals(result))
                return false;

            if (dtCurrentTable.Rows.Count == 1)        //sending empty form data //to be changed once model-view is binded
                return false;

            return true;
        }
        #endregion
    }

    //Add client Validation that NCP days are never more than 27 days
}