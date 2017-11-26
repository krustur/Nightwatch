using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
// ReSharper disable InconsistentNaming

namespace WindowsKeepAlive
{


    public partial class WindowsKeepAlive : Form
    {
        private Timer _timer;

        public WindowsKeepAlive()
        {
            InitializeComponent();
        }       

        private void SetAllowSleepMode()
        {
            DeleteTimer();            
            AllowSleep(false);
            DisableAutomaticSettings();
        }

        private void SetPreventSleepMode()
        {
            DeleteTimer();
            PreventSleep(false);
            DisableAutomaticSettings();
        }

        private void SetAutomaticMode()
        {
            DeleteTimer();
            CreateTimer();
            EvaluateAutomaticMode();
            EnableAutomaticSettings();
        }

        private void CreateTimer()
        {
            _timer = new Timer
            {
                Interval = 1000
            };
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void DeleteTimer()
        {
            if (_timer != null)
            {
                //if (_timer.Enabled == true)
                //{
                _timer.Stop();
                //}
                _timer = null;
            }
        }

        private void EvaluateAutomaticMode()
        {
            var now = DateTime.Now.TimeOfDay;
            if (now >= dateTimePickerAutomaticStartTime.Value.TimeOfDay &&
                now <= dateTimePickerAutomaticStopTime.Value.TimeOfDay)
            {
                PreventSleep(true);
            }
            else
            {
                AllowSleep(true);
            }
        }

        public void AllowSleep(bool automaticMode)
        {
            SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            labelCurrentMode.Text = automaticMode ? "Sleep allowed (automatic mode)" : "Sleep allowed";
        }

        public void PreventSleep(bool automaticMode)
        {
            if (SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                                                  | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                                                  | EXECUTION_STATE.ES_SYSTEM_REQUIRED
                                                  | EXECUTION_STATE.ES_AWAYMODE_REQUIRED) == 0) //Away mode for Windows >= Vista
                SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                                                  | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                                                  | EXECUTION_STATE.ES_SYSTEM_REQUIRED); //Windows < Vista, forget away mode
            labelCurrentMode.Text = automaticMode ? "Sleep prevented (automatic mode)" : "Sleep prevented";
        }

        private void DisableAutomaticSettings()
        {
            groupBoxAutomaticSettings.Enabled = false;
        }

        private void EnableAutomaticSettings()
        {
            groupBoxAutomaticSettings.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePickerAutomaticStartTime.Format = DateTimePickerFormat.Time;
            dateTimePickerAutomaticStartTime.ShowUpDown = true;
            dateTimePickerAutomaticStopTime.Format = DateTimePickerFormat.Time;
            dateTimePickerAutomaticStopTime.ShowUpDown = true;

            LoadConfiguration();

            
        }

        private void LoadConfiguration()
        {
            var automaticStartTime = (TimeSpan)Properties.Settings.Default["AutomaticStartTime"];
            var automaticStopTime = (TimeSpan)Properties.Settings.Default["AutomaticStopTime"];
            var mode = (int)Properties.Settings.Default["Mode"];

            dateTimePickerAutomaticStartTime.Value = new DateTime(2000, 1, 1,
                automaticStartTime.Hours,
                automaticStartTime.Minutes,
                automaticStartTime.Seconds);
            dateTimePickerAutomaticStopTime.Value = new DateTime(2000, 1, 1,
                automaticStopTime.Hours,
                automaticStopTime.Minutes,
                automaticStopTime.Seconds);
            comboBoxSelectMode.SelectedIndex = mode;

        }

        private void SaveModeConfiguration()
        {
            Properties.Settings.Default["Mode"] = comboBoxSelectMode.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void SaveAutomaticStartTimeConfiguration()
        {
            Properties.Settings.Default["AutomaticStartTime"] = dateTimePickerAutomaticStartTime.Value.TimeOfDay;            
            Properties.Settings.Default.Save();
        }

        private void SaveAutomaticStopTimeConfiguration()
        {
            Properties.Settings.Default["AutomaticStopTime"] = dateTimePickerAutomaticStopTime.Value.TimeOfDay;
            Properties.Settings.Default.Save();
        }

        private void ComboBoxSelectMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveModeConfiguration();
            switch (comboBoxSelectMode.SelectedIndex)
            {
                case 0:
                    SetAllowSleepMode();
                    break;
                case 1:
                    SetPreventSleepMode();
                    break;
                case 2:
                    SetAutomaticMode();
                    break;

            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            EvaluateAutomaticMode();
        }

        private void DateTimePickerAutomaticStartTime_ValueChanged(object sender, EventArgs e)
        {
            SaveAutomaticStartTimeConfiguration();
            EvaluateAutomaticMode();
        }

        private void DateTimePickerAutomaticStopTime_ValueChanged(object sender, EventArgs e)
        {
            SaveAutomaticStopTimeConfiguration();
            EvaluateAutomaticMode();
        }
    }

    [Flags]
    public enum EXECUTION_STATE : uint
    {
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
        // Legacy flag, should not be used.
        // ES_USER_PRESENT   = 0x00000004,
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
    }

    public static class SleepUtil
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}
