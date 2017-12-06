using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsKeepAlive.Service;

// ReSharper disable InconsistentNaming

namespace WindowsKeepAlive
{
    public partial class WindowsKeepAlive : Form
    {
        private Timer _timer;
        private readonly SleepService _sleepService;
        private bool _acceptEvents = true;

        public WindowsKeepAlive(SleepService sleepService)
        {
            _sleepService = sleepService;
            _sleepService.StateChanged += StateChanged;
            _sleepService.ModeChanged += ModeChanged;
            _sleepService.AutomaticStartTimeChanged += AutomaticStartTimeChanged;
            _sleepService.AutomaticStopTimeChanged += AutomaticStopTimeChanged;

            InitializeComponent();

            dateTimePickerAutomaticStartTime.Format = DateTimePickerFormat.Time;
            dateTimePickerAutomaticStartTime.ShowUpDown = true;
            dateTimePickerAutomaticStopTime.Format = DateTimePickerFormat.Time;
            dateTimePickerAutomaticStopTime.ShowUpDown = true;
        }

        private void AutomaticStopTimeChanged(TimeSpan automaticStopTime)
        {
            dateTimePickerAutomaticStopTime.Value = new DateTime(2000, 1, 1,
                automaticStopTime.Hours,
                automaticStopTime.Minutes,
                automaticStopTime.Seconds);
        }

        private void AutomaticStartTimeChanged(TimeSpan automaticStartTime)
        {
            dateTimePickerAutomaticStartTime.Value = new DateTime(2000, 1, 1,
                automaticStartTime.Hours,
                automaticStartTime.Minutes,
                automaticStartTime.Seconds);
        }

        private void ModeChanged(SleepServiceMode mode)
        {      
            comboBoxSelectMode.SelectedIndex = (int) mode;
            if (mode == SleepServiceMode.Automatic)
            {
                groupBoxAutomaticSettings.Enabled = true;                
            }
            else
            {
                groupBoxAutomaticSettings.Enabled = false;
            }
        }

        private void StateChanged(SleepServiceState sleepServiceState, bool automaticMode)
        {
            switch (sleepServiceState)
            {
                case SleepServiceState.SleepAllowed:
                    labelCurrentMode.Text = automaticMode ? "Sleep allowed (automatic mode)" : "Sleep allowed";                
                    break;

                case SleepServiceState.SleepDenied:
                    labelCurrentMode.Text = automaticMode ? "Sleep prevented (automatic mode)" : "Sleep prevented";
                    break;
            }
        }

        private void ComboBoxSelectMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_acceptEvents)
            {
                _acceptEvents = false;
                var selectedMode = (SleepServiceMode) comboBoxSelectMode.SelectedIndex;
                _sleepService.SetModeConfiguration(selectedMode);
                _acceptEvents = true;
            }
        }
       
        private void DateTimePickerAutomaticStartTime_ValueChanged(object sender, EventArgs e)
        {
            if (_acceptEvents)
            {
                _acceptEvents = false;
                _sleepService.SetAutomaticStartTimeConfiguration(dateTimePickerAutomaticStartTime.Value.TimeOfDay);
                _acceptEvents = true;
            }
        }

        private void DateTimePickerAutomaticStopTime_ValueChanged(object sender, EventArgs e)
        {
            if (_acceptEvents)
            {
                _acceptEvents = false;
                _sleepService.SetAutomaticStopTimeConfiguration(dateTimePickerAutomaticStopTime.Value.TimeOfDay);
                _acceptEvents = true;
            }
        }

        public void EnableEvents()
        {
            _acceptEvents = true;
        }

        public void DisableEvents()
        {
            _acceptEvents = false;
        }
    }

   
}
