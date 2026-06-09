using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class BaslerCameraControl : Form
    {
        const int GWL_STYLE = -16;
        const int WS_CHILD = 0x40000000;
        const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        const uint SWP_FRAMECHANGED = 0x0020;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);
        public BaslerCameraControl(int LeftRight)
        {
            InitializeComponent();
        }
        private void buttonVisionConnect_Click(object sender, EventArgs e)
        {
        }
        private void buttonGrapStart_Click(object sender, EventArgs e)
        {
            // 실행할 프로그램 경로
            //string programPath = @"C:\ScrewVision\ScrewVision_.exe";
            //Process process = Process.Start(programPath);
            //if (process == null)
            //{
            //    MessageBox.Show("프로그램 실행 실패.");
            //    return;
            //}
            // 외부 프로그램을 Panel에서 실행
             RunExternalProgramInPanel();
        }
        public void RunExternalProgramInPanel()
        {
            try
            {
                var processes = Process.GetProcessesByName("ScrewVision");
                if (processes.Length == 0)
                {
                    Console.WriteLine("ScrewVision.exe 프로세스를 찾을 수 없습니다.");
                    return;
                }

                IntPtr hWnd = processes[0].MainWindowHandle;

                if (hWnd == IntPtr.Zero)
                {
                    Console.WriteLine("창 핸들을 찾을 수 없습니다.");
                    return;
                }

                int style = GetWindowLong(hWnd, GWL_STYLE);

                // 자식 윈도우 스타일 제거 후 일반 윈도우 스타일 설정
                style = (style & ~WS_CHILD) | WS_OVERLAPPEDWINDOW;
                SetWindowLong(hWnd, GWL_STYLE, style);

                // 크기 및 위치 초기화
                SetWindowPos(hWnd, IntPtr.Zero, 100, 100, 1024, 768,
                    SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);

                Console.WriteLine("ScrewVision.exe 창 스타일과 크기를 초기화했습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"외부 프로그램 실행 중 오류 발생: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
