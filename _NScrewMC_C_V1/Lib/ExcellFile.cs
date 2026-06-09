using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRJigUnload
{
    class ExcellFile
    {
        #region //Variable 1.header file 2.with columns
        public static string[] strInputHeader = { "STT", "Input Time", "Output Intend Time", "Aging Intend Time", "Temp", "Model Name", "Lot.No", "Length (M)", "Location" };
        public static string[] strOutputHeader = { "STT", "Input Time", "Output Time", "Aging Time", "Temp", "Model Name", "Lot.No", "Length (M)", "Location"};
        public static string[] strListRunHeader = { "STT", "Input Time", "Output Intend Time", "ReAging Time", "Temp", "Model Name", "Lot.No", "Length (M)", "Location" };
        public static string[] strListModelHeader = { "STT", "Model Name", "PSA Type", "Tape Type", "Aging Temp", "Aging Time" };                                     
        public static string[] strAlarmHeader = { "STT", "Error Name", "Time Happened" };

        // Report Output
        public static string[] strReportCurentList  ={ "STT", "Model","Lot.No", "Length", "Location", "Input Time","Hours Time" ,"Day Time", "Output Plan"};
        public static string[] strReportOutputList = { "STT", "Model", "Lot.No", "Length", "Location", "Input Time", "Hours Time", "Day Time", "Output Plan","Output Time","Delta Time"};
        //
        //Range
        public static int[] WithColumnDgvLog = { 20, 80, 80, 70, 30, 80, 70, 50, 160 };
        public static int[] WithColumnModel = { 40, 150, 110, 110, 100, 100 };
        public static int[] WithError = { 35, 1000, 200 };

        protected static string[] strindex = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N",
                                        "O", "P", "Q","R","S","T","U","V","W","X","Y","Z" };
        
        #endregion

        #region //Read file Excel don't use tool 
        public DataTable ReadExcelFile(string sheetName, string path)
        {
            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();
                string fileExtension = Path.GetExtension(path);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data S0ource=" + path + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$]";
                    comm.Connection = conn;
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        #endregion

        #region // Process File Excel
        public static void CreateExcelFile(string path, string[] header, int cols)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            for (int i = 0; i < cols; i++)
            {
                string temp = strindex[i] + "1";
                worksheet.Range[temp].Text = header[i];
            }
            workbook.SaveToFile(path, ExcelVersion.Version2010);
        }
        public static void SaveExcelFile(DataTable dtb, string path) {

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.InsertDataTable(dtb, true, 1, 1);

            worksheet.AllocatedRange.AutoFitColumns();
            worksheet.AllocatedRange.AutoFitColumns();

            try {
                workbook.SaveToFile(path, ExcelVersion.Version2010);
            }
            catch {
            }

        }
        public void InsertIndexTable(DataTable dataTable) {
            int i = dataTable.Rows.Count;
            for (int j = 0; j < i; j++)
                dataTable.Rows[j][0] = j + 1;
        }
        public static DataTable LoadTableFromExcel(string path) {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(path, ExcelVersion.Version2010);
            Worksheet sheet = workbook.Worksheets[0];
            return sheet.ExportDataTable();
        }
        public static DataTable LoadTableFromExcel(string[] path) {
            DataTable TableAll = new DataTable();
            DataTable TableTemp = new DataTable();
            int i = 0;
            foreach (string str in path) {
                if (i == 0) {
                    TableAll = LoadTableFromExcel(str);
                }
                else {
                    TableTemp = LoadTableFromExcel(str);
                    TableAll.Merge(TableTemp);
                }
                i++;
            }
            return TableAll;
        }
        public static void InsertRowsToExcel(string path, string[] content) {
            try
            {
                if (!File.Exists(path)) return;
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(path);
                Worksheet worksheet = workbook.Worksheets[0];
                DataTable tempDatatable = worksheet.ExportDataTable();
                tempDatatable.Rows.Add(content);
                worksheet.InsertDataTable(tempDatatable, true, 1, 1);
                worksheet.AllocatedRange.AutoFitColumns();
                worksheet.AllocatedRange.AutoFitColumns();
                //Save file
                workbook.SaveToFile(path);
            }
            catch { }
        }
        public string[] GetAllFileFolder(string path)
        {
            if (!Directory.Exists(path)){
                Directory.CreateDirectory(path);
                return null;
            }
            DirectoryInfo foderInfor = new DirectoryInfo(path);
            FileInfo[] Dataname = foderInfor.GetFiles("*.xlsx");
            if (Dataname.Length == 0) return null;
            string[] strname = new string[Dataname.Length];
            for (int i = 0; i < Dataname.Length; i++)
            {
                strname[i] = Dataname[i].Name;
            }
            return strname;
        }
        public static void InitIndexTable(DataTable Tb, string filepath)
        {
            int i = 1;
            foreach (DataRow dr in Tb.Rows) {
                dr[0] = i;
                i++;
            }
            try {
                DataTable TBClone = Tb.Copy();
                ExcellFile.SaveExcelFile(TBClone, filepath);
            }
            catch {

            }
            finally{ 
            
            }

        }
        #endregion

        #region //Process Gridview
        public static void InitialGridview(System.Windows.Forms.DataGridView dgv, int[] dgvwith)
        {
            dgv.SuspendLayout();
            dgv.ColumnHeadersVisible = false;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //dgv.RowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft; // Aligment header row
            //dgv.RowHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);//Format Font
            // dgv.setro
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (dgv.Columns.Count<=16)
              dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            int i = 0;
            foreach (System.Windows.Forms.DataGridViewColumn col in dgv.Columns){
                col.Width = dgvwith[i];
                col.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Arial", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                //Set Read Only for cell
                col.ReadOnly = true;
                //disable sort DataGridview
                col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                i++;
            }
            dgv.RowHeadersWidth = 30;
            dgv.RowHeadersVisible = false;
            //Show header text for row
            dgv.Rows[0].Selected = true;
            dgv.Columns[0].DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgv.Columns[1].DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgv.Columns[2].DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgv.ColumnHeadersVisible = true;
            dgv.ResumeLayout(false);
        }

        public static void DisableEditValueGridview(System.Windows.Forms.DataGridView dgv, bool value)
        {
            dgv.Columns[1].ReadOnly = value;
            dgv.Columns[2].ReadOnly = value;
        }
        #endregion
      
        #region // Tool creat Randoom String
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public int RandomInt(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public double RandomFloat(double min, double max)
        {
            Random rd = new Random();
            return rd.NextDouble() * (max - min) + min;
        }
        #endregion

        #region//Check File IsOpen
        public static bool IsFileClose(string filePath)
        {
            try{
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)){
                    if (fileStream != null) fileStream.Close();  
                }
            }
            catch{
                return false;
            }
            return true;
        }
        #endregion
    }
}
