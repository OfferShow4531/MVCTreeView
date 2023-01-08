using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace MVCTreeView.Controllers
{
    public class TreeViewController : Controller
    {
        private SqlConnection conn;
        private SqlDataAdapter da;
        private DataTable dt;

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.DOM_TreeViewMenu = PopulateMenuDataTable();
            return View();
        }
        private string PopulateMenuDataTable()
        {
            string DOM = "";

            string sql = @"SELECT MenuNumber, ParentId, Name, Description FROM locations";
            conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; 
	                                Initial Catalog = Categories; 
	                                User ID = Judgment_Developer; Password = 1963");
            conn.Open();

            da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.CommandTimeout = 10000;

            dt = new DataTable();
            da.Fill(dt);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Dispose();

            DOM = GetDOMTreeView(dt);

            return DOM;
        }

        private string GetDOMTreeView(DataTable dt)
        {
            string DOMTreeView = "";

            DOMTreeView += CreateTreeViewOuterParent(dt);
            DOMTreeView += CreateTreeViewMenu(dt, "0");

            DOMTreeView += "</ul>";

            return DOMTreeView;
        }

        private string CreateTreeViewOuterParent(DataTable dt)
        {
            string DOMDataList = "";

            DataRow[] drs = dt.Select("MenuNumber = 0");

            foreach (DataRow row in drs)
            {
                //row[2], 2 is column number start with 0, which is the MenuName
                DOMDataList = "<ul><li class='header'>" + row[2].ToString() + "</li>";
            }

            return DOMDataList;
        }

        private string CreateTreeViewMenu(DataTable dt, string ParentId)
        {
            string DOMDataList = "";

            string menuNumber = "";
            string menuName = "";
            

            DataRow[] drs = dt.Select("ParentId = " + ParentId);

            foreach (DataRow row in drs)
            {
                menuNumber = row[0].ToString();
                menuName = row[2].ToString();
                

                DOMDataList += "<li class='treeview'>";
                DOMDataList += "<a href=' '><i class=''></i><span>  "
                                + menuName + "</span></a>";

                DataRow[] drschild = dt.Select("ParentId = " + menuNumber);
                if (drschild.Count() != 0)
                {
                    DOMDataList += "<ul class='treeview-menu'>";
                    DOMDataList += CreateTreeViewMenu(dt, menuNumber).Replace
                                   ("<li class='treeview'>", "<li>");
                    DOMDataList += "</ul></li>";
                }
                else
                {
                    DOMDataList += "</li>";
                }
            }
            return DOMDataList;
        }


    }
}
