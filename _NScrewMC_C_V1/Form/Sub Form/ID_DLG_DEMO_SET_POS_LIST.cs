using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class DemoSetPosList : Form
    {
        public DemoSetPosList()
        {
            InitializeComponent();

            LIST_DEMO_SET.View = View.Details;
            LIST_DEMO_SET.FullRowSelect = true;
            LIST_DEMO_SET.OwnerDraw = false;
            LIST_DEMO_SET.Columns.Add("NO", 60, HorizontalAlignment.Center);
            LIST_DEMO_SET.Columns.Add("Demo X", 100, HorizontalAlignment.Center);
            LIST_DEMO_SET.Columns.Add("Demo Y", 100, HorizontalAlignment.Center);
            LIST_DEMO_SET.Columns.Add("Set X", 100, HorizontalAlignment.Center);
            LIST_DEMO_SET.Columns.Add("Set Y", 100, HorizontalAlignment.Center);
            LIST_DEMO_SET.Columns.Add("Diff X", 100, HorizontalAlignment.Center);
            LIST_DEMO_SET.Columns.Add("Diff Y", 100, HorizontalAlignment.Center);
            ListInit();
        }

        private void ListInit()
        {
            ListViewItem LvItem;
            IniFile ini;
            string path = Config.SystemSavePath + "DemoSetPosition.ini";
            ini = new IniFile(path);
            int index = 1;
            while (true)
            {
                string key = index.ToString();
                string value = ini.Read("DEMO", key, "");
                if (value == "") break;
                string[] split = value.Split(',');
                string item = $"P{index}";
                LvItem = new ListViewItem(item);
                LvItem.SubItems.Add(split[0]);
                LvItem.SubItems.Add(split[1]);

                double x1 = double.Parse(split[0]);
                double y1 = double.Parse(split[1]);
                value = ini.Read("SET", key, "");
                if (value != "")
                {
                    split = value.Split(',');
                    LvItem.SubItems.Add(split[0]);
                    LvItem.SubItems.Add(split[1]);

                    double x2 = double.Parse(split[0]);
                    double y2 = double.Parse(split[1]);

                    string disX = (x1 - x2).ToString("F3");
                    string disY = (y1 - y2).ToString("F3");
                    LvItem.SubItems.Add(disX);
                    LvItem.SubItems.Add(disY);
                }
                
                LIST_DEMO_SET.Items.Add(LvItem);
                index++;
            }
        }

        private void BT_FILE_SAVE_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (*.csv)|*.csv";
            dialog.FileName = "ListViewData.csv";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            StringBuilder sb = new StringBuilder();

            // Column Header 저장
            for (int i = 0; i < LIST_DEMO_SET.Columns.Count; i++)
            {
                sb.Append(LIST_DEMO_SET.Columns[i].Text);
                if (i < LIST_DEMO_SET.Columns.Count - 1)
                    sb.Append(",");
            }
            sb.AppendLine();

            // Item 데이터 저장
            foreach (ListViewItem item in LIST_DEMO_SET.Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    sb.Append(item.SubItems[i].Text);
                    if (i < item.SubItems.Count - 1)
                        sb.Append(",");
                }
                sb.AppendLine();
            }

            File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);

            MessageBox.Show("CSV 저장 완료");
        }

        private void BT_CLOSE_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
