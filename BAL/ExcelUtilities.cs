using System.Data;
using System.Data.OleDb;
using System.Web;
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
            return dt;
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
