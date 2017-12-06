using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsKeepAlive.Properties;
using WindowsKeepAlive.Service;

namespace WindowsKeepAlive
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new WindowsKeepAliveContext());
        }
    }

    internal class WindowsKeepAliveContext : ApplicationContext
    {
        readonly WindowsKeepAlive _windowsKeepAlive;
        private readonly NotifyIcon _notifyIcon;

        public WindowsKeepAliveContext()
        {
            var sleepService = new SleepService();

            _windowsKeepAlive = new WindowsKeepAlive(sleepService);
            _windowsKeepAlive.DisableEvents();
            sleepService.StartService();
            _windowsKeepAlive.EnableEvents();

            var configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
            var exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            _notifyIcon = new NotifyIcon();
            
            _notifyIcon.Icon = global::WindowsKeepAlive.Properties.Resources.icon;
            _notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
                { configMenuItem, exitMenuItem });
            _notifyIcon.Visible = true;
            _notifyIcon.Click += ShowConfig;
        }

        void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            _notifyIcon.Visible = false;
            Application.Exit();
        }

       

        void ShowConfig(object sender, EventArgs e)
        {
            if (_windowsKeepAlive.Visible)
            {
                _windowsKeepAlive.Activate();
            }
            else
            {
                _windowsKeepAlive.ShowDialog();
            }
        }
    }
}
