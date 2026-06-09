using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace _NScrewMC_C_V1
{
    public partial class FormLog : Form
    {
        int _indexLog = 0;
        string[] strNameLog = { "No.Error", "Date", "Contents" };
        int[] _withNameLog = { 80, 100, 250 };
        //
        string[] strNameLogCount = { "Count", "No.Error", "Contents" };
        int[] _withNameLogCount = { 60, 80, 250 };

        public class MyMonthCalendar : MonthCalendar
        {
            [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
            static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
            protected override void OnHandleCreated(EventArgs e)
            {
                SetWindowTheme(Handle, string.Empty, string.Empty);
                base.OnHandleCreated(e);
            }
        }

        public static MyMonthCalendar monthCalendar = new MyMonthCalendar();

        public void Loadcontrol()
        {
            this.Controls.Add(monthCalendar);
            monthCalendar.Top  = 1;
            monthCalendar.Left = 1;
            monthCalendar.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
        }

        #region //Initial form
        public FormLog()
        {
            InitializeComponent();
            IDC_DATA_PICK_TIME_START.CustomFormat = "ddd,dd-MM-yyyy";
            IDC_DATA_PICK_TIME_END.CustomFormat = "ddd,dd-MM-yyyy";
            MSystem.m_plisView.InitListview(LV_LOG, strNameLog, _withNameLog, LV_LOG.Top, LV_LOG.Left, LV_LOG.Size, 12F);
            MSystem.m_plisView.InitListview(LV_LOG_COUNT, strNameLogCount, _withNameLogCount, LV_LOG_COUNT.Top, LV_LOG_COUNT.Left, LV_LOG_COUNT.Size, 12F);
            IDC_DATA_PICK_TIME_START.Value = DateTime.Now;//DateTime.Today.AddDays(-1);
            IDC_DATA_PICK_TIME_END.Value = DateTime.Now;
        }
        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadDataLog(_indexLog);
        }
        private void FormLog_Load(object sender, EventArgs e){
            Loadcontrol();

            monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(monthCalendar_DateChanged);
            LoadDataLog(0);
        }
        private void PreLoadEvent(object sender, EventArgs e) {
            switch ((sender as Button).Name.ToString()) {
                case "BtInput":
                    //LoadInput();
                    break;
                case "BtOutput":
                    //LoadOutput();
                    break;
                case "BtAllError":
                   // LoadError();
                    break;
                case "BtEdit":
                    //GetPass();
                    break;
                default: break;
            }
        
        }
        #endregion

        #region // Re Draw listview
       
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 0) // Or whatever the column in question is.
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, new SolidBrush(e.SubItem.ForeColor), e.Bounds, format);
                e.DrawDefault = false;
            }
            else
            {
                e.DrawDefault = true;
            }
        }
        #endregion
        void LoadDataLog(int _index)
        {

            string _fileName = "";
            string _Date = "", _Date2 = "";
            lock (MSystem.LockLog)
            {
                switch (_index)
                {
                    case 0:
                        MSystem.m_pDatabaseLog.CreateTB();
                        _Date = $"{monthCalendar.SelectionRange.Start.ToString("yyyyMMdd")}";
                        MSystem.m_plisView.TB_Error = MSystem.m_pDatabaseLog.GetDataSearchRange($"{_Date}_00:00:00", $"{_Date}_23:59:59");
                        MSystem.m_plisView.LoadListData(LV_LOG, MSystem.m_plisView.TB_Error);
                        MSystem.m_plisView.CoutErrorLog(LV_LOG_COUNT, MSystem.m_plisView.TB_Error);
                        break;

                    case 1:
                        if (!File.Exists(Config.LogDevice))
                        {
                            Directory.CreateDirectory(Config.LogDevice);
                        }
                        _fileName = $"{Config.LogDevice}{monthCalendar.SelectionRange.Start.ToString("yyyyMMdd")}_Devlog.txt";
                        break;

                    case 2:
                        if (!File.Exists(Config.LogDataSave))
                        {
                            Directory.CreateDirectory(Config.LogDataSave);
                        }
                        _fileName = $"{Config.LogDataSave}{monthCalendar.SelectionRange.Start.ToString("yyyyMMdd")}_DataSave.txt";
                        break;

                    case 3:
                        MSystem.m_pDatabaseLog.CreateTB();
                        _Date = $"{IDC_DATA_PICK_TIME_START.Value.ToString("yyyyMMdd")}";
                        _Date2 = $"{IDC_DATA_PICK_TIME_END.Value.ToString("yyyyMMdd")}";
                        LV_LOG_COUNT.Sorting = SortOrder.None;
                        MSystem.m_plisView.TB_Error = MSystem.m_pDatabaseLog.GetDataSearchRange($"{_Date}_00:00:00", $"{_Date2}_23:59:59");
                        MSystem.m_plisView.LoadListData(LV_LOG, MSystem.m_plisView.TB_Error);
                        MSystem.m_plisView.CoutErrorLog(LV_LOG_COUNT, MSystem.m_plisView.TB_Error);
                        break;

                    default:
                        return;
                }
            }
        }

        private void FormLog_VisibleChanged(object sender, EventArgs e)
        {
            LoadDataLog(_indexLog);
        }

        private void IDC_CLEAR_Click(object sender, EventArgs e)
        {
            lock (MSystem.Lock_DisplayLog)
            {
                LV_LOG.Items.Clear();
                LV_LOG_COUNT.Items.Clear();
            }
        }

        private void BT_LOG_ERROR_Click(object sender, EventArgs e)
        {
            _indexLog = 0;
            LoadDataLog(3);
        }
    }
}
