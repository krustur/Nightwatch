using System;
using System.Windows.Forms;
using Nightwatch.Service;

namespace Nightwatch
{
    internal class NightwatchContext : ApplicationContext
    {
        readonly Nightwatch.NightwatchForm _nightwatchForm;
        private readonly NotifyIcon _notifyIcon;

        public NightwatchContext()
        {
            var sleepService = new SleepService();

            _nightwatchForm = new Nightwatch.NightwatchForm(sleepService);
            _nightwatchForm.DisableEvents();
            sleepService.StartService();
            _nightwatchForm.EnableEvents();

            var configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
            var exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            _notifyIcon = new NotifyIcon();
            
            _notifyIcon.Icon = Properties.Resources.icon;
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
            if (_nightwatchForm.Visible)
            {
                _nightwatchForm.Activate();
            }
            else
            {
                _nightwatchForm.ShowDialog();
            }
        }
    }
}