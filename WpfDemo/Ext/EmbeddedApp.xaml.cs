using AppContainers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo.Ext
{
    /// <summary>
    /// EmbeddedApp.xaml 的交互逻辑
    /// </summary>
    public partial class EmbeddedApp : UserControl
    {
        public EmbeddedApp()
        {
            InitializeComponent();
        }
        private WindowsFormsHost _winFormHost = null;
        private System.Windows.Forms.Panel _hostPanel = null;

        private ManualResetEvent _eventDone = new ManualResetEvent(false);
        private Process _process = null;

        private IntPtr _embededWindowHandle = (IntPtr)0;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _winFormHost = PART_Host;
           // _winFormHost = GetTemplateChild("PART_Host") as WindowsFormsHost;
            if (_winFormHost != null)
            {
                _hostPanel = new System.Windows.Forms.Panel();
                _winFormHost.Child = _hostPanel;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_process != null)
            {
                Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            }
            base.OnRender(drawingContext);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.InvalidateVisual();
            base.OnRenderSizeChanged(sizeInfo);
        }

        #region Private Methods

        /// <summary>
        /// 启动外进程
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        private Process StartApp(string appPath)
        {
            if (!File.Exists(appPath))
            {
                return null;
            }
            ProcessStartInfo info = new ProcessStartInfo(appPath)
            {
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Minimized
            };
            Process process = Process.Start(info);

            return process;
        }

        /// <summary>
        /// 关闭外进程
        /// </summary>
        /// <param name="process"></param>
        private void CloseApp(Process process)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }

        /// <summary>
        /// 将外进程嵌入到当前程序
        /// </summary>
        /// <param name="process"></param>
        private bool EmbedApp(Process process)
        {
            //是否嵌入成功标志，用作返回值
            bool isEmbedSuccess = false;
            //外进程句柄
            IntPtr processHwnd = process.MainWindowHandle;
            //容器句柄
            IntPtr panelHwnd = _hostPanel.Handle;

            if (processHwnd != (IntPtr)0 && panelHwnd != (IntPtr)0)
            {
                //把本窗口句柄与目标窗口句柄关联起来
                int setTime = 0;
                while (!isEmbedSuccess && setTime < 10)
                {
                    isEmbedSuccess = (Win32Api.SetParent(processHwnd, panelHwnd) != 0);
                    Thread.Sleep(100);
                    setTime++;
                }
                //设置初始尺寸和位置
                Win32Api.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            }

            if (isEmbedSuccess)
            {
                _embededWindowHandle = _process.MainWindowHandle;
            }

            return isEmbedSuccess;
        }

        #endregion

        #region Public Methods

        public bool StartAndEmbedProcess(string processPath)
        {
            bool isStartAndEmbedSuccess = false;
            _eventDone.Reset();

            //启动进程
            _process = StartApp(processPath);
            if (_process == null)
            {
                return false;
            }

            //确保可获取到句柄
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (_process.MainWindowHandle != (IntPtr)0)
                    {
                        _eventDone.Set();
                        break;
                    }
                    Thread.Sleep(10);
                }
            }));
            thread.Start();

            //嵌入进程
            if (_eventDone.WaitOne(10000))
            {
                isStartAndEmbedSuccess = EmbedApp(_process);
                if (!isStartAndEmbedSuccess)
                {
                    CloseApp(_process);
                }
            }
            return isStartAndEmbedSuccess;
        }

        public void CloseProcess()
        {
            CloseApp(_process);
        }

        public bool EmbedExistProcess(Process process)
        {
            _process = process;
            return EmbedApp(process);
        }

        #endregion
    }
}
