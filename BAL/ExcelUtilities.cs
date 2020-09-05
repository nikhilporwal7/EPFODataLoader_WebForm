using System;
using System.Data;
using System.IO;
using System.Text;
using dBASE.NET;

namespace BAL
{
    public class ExcelUtilities
    {
        public static DataTable ReadDBFfile(string filePath, int monthIndex)
        {
            var dbf = new Dbf();
            dbf.Read(filePath);
            var dt = new DataTable();


            //return dt;

            // Cols
            for (int i = 0; i < dbf.Fields.Count; i++)
            {
                dt.Columns.Add(dbf.Fields[i].Name, dbf.Fields[i].GetType().GetProperties()[0].PropertyType);
            }

            // Rows
            foreach (DbfRecord record in dbf.Records)
            {
                var row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++) row[i] = record.Data[i];
                dt.Rows.Add(row);
            }
            // Clean
            dbf = null;

            //filter the datafrom row in myDTable.AsEnumerable()
            string EMP_UAN = string.Empty;
            string EMP_NAME = string.Empty;

            //var dt2 = new DataTable();
            
            //SHOULD  I make a concrete class for this
            var res = from myRow in dt.AsEnumerable()
                      where myRow.Field<string>("WAGE"+ monthIndex.ToString() ) != null
                      select new Anon
                      {
                          EMP_UAN = myRow.Field<string>("EMP_UAN"),
                          EMP_NAME = myRow.Field<string>("EMP_NAME"),
                          EMP_WAGE = Convert.ToInt32(myRow.Field<string>("WAGE" + monthIndex.ToString()))
                      };
            dt = new DataTable();
            dt.Columns.Add("EMP_UAN");
            dt.Columns.Add("EMP_NAME");
            dt.Columns.Add("WAGE" + monthIndex.ToString());
            foreach (var item in res)
            {
                DataRow row = dt.NewRow();

                foreach (var col in dt.Columns)
                {
                    row[columnName: "EMP_UAN"] = item.EMP_UAN;
                    row[columnName: "EMP_NAME"] = item.EMP_NAME;
                    row[columnName: "WAGE" + monthIndex.ToString()] = item.EMP_WAGE;
                }

                dt.Rows.Add(row);
            }
            
            return dt;
        }

        /// <summary>
        /// src: https://stackoverflow.com/questions/4959722/c-sharp-datatable-to-csv
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="csvFile"></param>
        public static StringBuilder DumpDBFToTxt(DataTable dt)
        {
            var result = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                result.Append(dt.Columns[i].ColumnName);
                result.Append(i == dt.Columns.Count - 1 ? "\n" : "#~#");
            }
            result.Append(Environment.NewLine);

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (row[3].ToString() == "0.00")
                    //{
                    //    //do nothing;
                    //}
                    //else
                    //{
                        string temp = row[i].ToString().Trim();
                        temp = temp.Contains(".") ? temp.TrimEnd('0').TrimEnd('.') : temp;
                        result.Append(temp);

                        result.Append(i == dt.Columns.Count - 1 ? Environment.NewLine : "#~#");        //https://stackoverflow.com/questions/2617752/newline-character-in-stringbuilder
                    //}
                }
                //result.Append(Environment.NewLine);
            }
            return result;
            //File.WriteAllText(csvFile, result.ToString());
        }

        private static string QueryBuilder(int monthIndex)
        {
            string finalQuery = string.Empty;
            string wages = "WAGE" + monthIndex;
            string pf = "PF" + monthIndex;
            //converting index value to string for some column to get month in spell
            MonthOption month = (MonthOption)monthIndex;
            string monthValue = month.ToString();
            string eePercent = monthValue.Substring(0, 3) + "EE";
            string erPercent = monthValue.Substring(0, 3) + "ER";

            return finalQuery = "SELECT EMP_UAN, EMP_NAME, " + wages + "," + wages + "," + wages + "," + wages + "," + eePercent + "," + pf + "," + erPercent + ", PF13 as NCPdays, PF13 as Totaldays";
        }
    }
}
