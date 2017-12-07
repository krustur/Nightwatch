using System;
using System.Windows.Forms;

namespace WindowsKeepAlive.Service
{
    public enum SleepServiceMode
    {
        AllowSleep = 0,
        DenySleep = 1,
        Automatic = 2
    }

    public enum SleepServiceState
    {
        SleepAllowed = 0,
        SleepDenied = 1,       
    }

    public class SleepService
    {
        private Timer _timer;
        private TimeSpan _automaticStartTime;
        private TimeSpan _automaticStopTime;
        private SleepServiceMode _mode;

        public TimeSpan AutomaticStartTime
        {
            get => _automaticStartTime;
            set
            {
                _automaticStartTime = value;
                AutomaticStartTimeChanged?.Invoke(value);
            }
        }

        public TimeSpan AutomaticStopTime
        {
            get => _automaticStopTime;
            set
            {
                _automaticStopTime = value;
                AutomaticStopTimeChanged?.Invoke(value); 
            }
        }

        public SleepServiceMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                ModeChanged?.Invoke(value);
            }
        }

        public Action<SleepServiceState, bool> StateChanged { get; set; }
        public Action<SleepServiceMode> ModeChanged { get; set; }
        public Action<TimeSpan> AutomaticStartTimeChanged { get; set; }
        public Action<TimeSpan> AutomaticStopTimeChanged { get; set; }

        public void StartService()
        {
            LoadConfiguration();
            EvaluateState();
            CreateTimer();
        }

        public void SetModeConfiguration(SleepServiceMode mode)
        {
            Mode = mode;

            Properties.Settings.Default["Mode"] = (int)Mode;
            Properties.Settings.Default.Save();

            switch (Mode)
            {
                case SleepServiceMode.AllowSleep:
                    EvaluateState();
                    break;
                case SleepServiceMode.DenySleep:
                    EvaluateState();
                    break;
                case SleepServiceMode.Automatic:
                    EvaluateState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

        }

        public void SetAutomaticStartTimeConfiguration(TimeSpan timeOfDay)
        {
            AutomaticStartTime = timeOfDay;
            Properties.Settings.Default["AutomaticStartTime"] = AutomaticStartTime;
            Properties.Settings.Default.Save();
            EvaluateState();
        }

        public void SetAutomaticStopTimeConfiguration(TimeSpan timeOfDay)
        {
            AutomaticStopTime = timeOfDay;
            Properties.Settings.Default["AutomaticStopTime"] = AutomaticStopTime;
            Properties.Settings.Default.Save();
            EvaluateState();
        }

        private void AllowSleep(bool automaticMode)
        {
            SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            StateChanged?.Invoke(SleepServiceState.SleepAllowed, automaticMode);
        }

        private void DenySleep(bool automaticMode)
        {
            if (SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                                                  | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                                                  | EXECUTION_STATE.ES_SYSTEM_REQUIRED
                                                  | EXECUTION_STATE.ES_AWAYMODE_REQUIRED) == 0) //Away mode for Windows >= Vista
                SleepUtil.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                                                  | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                                                  | EXECUTION_STATE.ES_SYSTEM_REQUIRED); //Windows < Vista, forget away mode
            StateChanged?.Invoke(SleepServiceState.SleepDenied, automaticMode);
        }

        private void LoadConfiguration()
        {
            AutomaticStartTime = (TimeSpan)Properties.Settings.Default["AutomaticStartTime"];
            AutomaticStopTime = (TimeSpan)Properties.Settings.Default["AutomaticStopTime"];
            Mode = (SleepServiceMode)Properties.Settings.Default["Mode"];            
        }
    
        private void CreateTimer()
        {
            _timer = new Timer
            {
                Interval = 5*1000
            };
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            EvaluateState();
        }

        private void EvaluateState()
        {
            switch (Mode)
            {
                case SleepServiceMode.AllowSleep:
                    AllowSleep(false);
                    break;
                case SleepServiceMode.DenySleep:
                    DenySleep(false);
                    break;
                case SleepServiceMode.Automatic:
                    var now = DateTime.Now.TimeOfDay;
                    if (now >= AutomaticStartTime &&
                        now <= AutomaticStopTime)
                    {
                        DenySleep(true);
                    }
                    else
                    {
                        AllowSleep(true);
                    }
                    break;
            }

        }
    }
}