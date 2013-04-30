using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using NUnit.Framework;

namespace ResearchLinks.SpecTests {
    /// <summary>
    /// Summary description for Startup
    /// </summary>
    [SetUpFixture]
    public class Startup {

        private static Process _iisProcess;

        [SetUp]
        public static void Initialize(){
            var thread = new Thread(StartIisExpress) { IsBackground = true };

            thread.Start();
        }

        [TearDown]
        public static void TestCleanup() {
            if (_iisProcess.Handle !=null && !_iisProcess.HasExited) {
                _iisProcess.CloseMainWindow();
                _iisProcess.Dispose();
            }
        }

        private static void StartIisExpress() {
            var startInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Minimized,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments = string.Format("/site:\"{0}\"", "ResearchLinks")
            };

            var programfiles = string.IsNullOrEmpty(startInfo.EnvironmentVariables["programfiles(x86)"])
                                ? startInfo.EnvironmentVariables["programfiles"]
                                : startInfo.EnvironmentVariables["programfiles(x86)"];

            startInfo.FileName = programfiles + "\\IIS Express\\iisexpress.exe";


            try {
                _iisProcess = new Process { StartInfo = startInfo };

                _iisProcess.Start();
                _iisProcess.WaitForExit();
            } catch (Exception exc){
                //_iisProcess.CloseMainWindow();
                //_iisProcess.Dispose();
            }
        }
    }
}
