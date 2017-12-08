using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Squirrel;

namespace Nightwatch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            //var squirrelFolder = @"F:\Google Drive\Projects\github\Nightwatch\Releases";
            //    using (var mgr = new UpdateManager(squirrelFolder))
            //    {
            //        await mgr.UpdateApp();
            //    }
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/krustur/Nightwatch"))
            {
                await mgr.Result.UpdateApp();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new NightwatchContext());
        }
    }
}
