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

            //var dt2 = new DataTable();

            //converting index value to string for some column to get month in spell
            MonthOption month = (MonthOption)monthIndex;
            string pf = "PF" + monthIndex;
            string monthValue = month.ToString();
            string eePercent = monthValue.Substring(0, 3) + "EE";
            string erPercent = monthValue.Substring(0, 3) + "ER";
            //SHOULD  I make a concrete class for this
            var res = from myRow in dt.AsEnumerable()
                      where myRow.Field<string>("WAGE" + monthIndex.ToString()) != null
                      select new Anon
                      {
                          EMP_UAN = myRow.Field<string>("EMP_UAN"),
                          EMP_NAME = myRow.Field<string>("EMP_NAME"),
                          EMP_WAGE = Convert.ToInt32(myRow.Field<string>("WAGE" + monthIndex.ToString())),
                          EMP_WAGE2 = Convert.ToInt32(myRow.Field<string>("WAGE" + monthIndex.ToString())),
                          EMP_WAGE3 = Convert.ToInt32(myRow.Field<string>("WAGE" + monthIndex.ToString())),
                          EMP_WAGE4 = Convert.ToInt32(myRow.Field<string>("WAGE" + monthIndex.ToString())),
                          EE_Percent = Convert.ToInt32(myRow.Field<string>(eePercent)),
                          PF = Convert.ToInt32(myRow.Field<string>(pf)),
                          ER_Percent = Convert.ToInt32(myRow.Field<string>(erPercent))
                      };
            dt = new DataTable();
            dt.Columns.Add("EMP_UAN");
            dt.Columns.Add("EMP_NAME");
            dt.Columns.Add("WAGE" + monthIndex.ToString());
            dt.Columns.Add("WAGE" + monthIndex.ToString() + 1);
            dt.Columns.Add("WAGE" + monthIndex.ToString() + 2);
            dt.Columns.Add("WAGE" + monthIndex.ToString() + 3);
            dt.Columns.Add("EEPer" + monthIndex.ToString());
            dt.Columns.Add(pf);
            dt.Columns.Add("ERPer" + monthIndex.ToString());
            dt.Columns.Add("NCPDays");
            dt.Columns.Add("Totaldays");

            foreach (var item in res)
            {
                DataRow row = dt.NewRow();

                foreach (var col in dt.Columns)
                {
                    row[columnName: "EMP_UAN"] = item.EMP_UAN;
                    row[columnName: "EMP_NAME"] = item.EMP_NAME;
                    row[columnName: "WAGE" + monthIndex.ToString()] = item.EMP_WAGE;
                    row[columnName: "WAGE" + monthIndex.ToString() + 1] = item.EMP_WAGE2;
                    row[columnName: "WAGE" + monthIndex.ToString() + 2] = item.EMP_WAGE3;
                    row[columnName: "WAGE" + monthIndex.ToString() + 3] = item.EMP_WAGE4;
                    row[columnName: "EEPer" + monthIndex.ToString()] = item.EE_Percent;
                    row[columnName: pf] = item.PF;
                    row[columnName: "ERPer" + monthIndex.ToString()] = item.ER_Percent;
                    row[columnName: "NCPdays"] = 0;
                    row[columnName: "Totaldays"] = 0;
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
            //result.Append(Environment.NewLine);

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string temp = row[i].ToString().Trim();
                    temp = temp.Contains(".") ? temp.TrimEnd('0').TrimEnd('.') : temp;
                    result.Append(temp);

                    result.Append(i == dt.Columns.Count - 1 ? Environment.NewLine : "#~#");        //https://stackoverflow.com/questions/2617752/newline-character-in-stringbuilder
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


        public static StringBuilder ReadCSVFile(DataTable dt, bool isHigherWageLimited = false)
        {
            var result = new StringBuilder();

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
                        temp = row[i].ToString().Trim();
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
                        temp = row[3].ToString() == null ? "0" : row[3].ToString();
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
            // }
        }
    }

}