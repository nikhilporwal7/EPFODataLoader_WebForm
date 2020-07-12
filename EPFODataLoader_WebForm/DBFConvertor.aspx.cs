using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPFODataLoader_WebForm.Models;

namespace EPFODataLoader_WebForm
{
    public partial class DBFConvertor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DDL_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            Array itemValues = System.Enum.GetValues(typeof(MonthOption));
            Array itemNames = System.Enum.GetNames(typeof(MonthOption));
           // DDL_Month.Items.Add(new ListItem(Enum.GetName(typeof(MonthOption), value), value.ToString()));

            var responseTypes = Enum.GetNames(typeof(MonthOption)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(MonthOption), x) });
            DDL_Month.DataSource = responseTypes;
            DDL_Month.DataTextField = "text";
            DDL_Month.DataValueField = "value";
            DDL_Month.DataBind();
        }
    }
}