using System.Runtime.InteropServices;

namespace Nightwatch.Service
{
    public static class SleepUtil
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}