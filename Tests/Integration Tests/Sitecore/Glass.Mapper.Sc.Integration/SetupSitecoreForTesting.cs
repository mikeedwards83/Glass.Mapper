using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Integration
{
    [SetUpFixture]
    public class SetupSitecoreForTesting
    {
        [SetUp]
        public void DeployTestItems()
        {
            //We need to locate the TDS project that contains our test configuration. We are assuming its folder is a sibling of the project folder and we are in the /bin/[config] folder
            string currentPath = Environment.CurrentDirectory;
            string tdsProjectPath = Path.GetFullPath(Path.Combine(currentPath, "..\\..\\..\\Glass.Mapper.Sc.Integration.Tds"));

            string msBuildPath = ConfigurationManager.AppSettings["MSBuildPath"];

            ProcessStartInfo psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = Path.Combine(msBuildPath, "MSBuild.exe"),
                Arguments = "/t:Deploy Glass.Mapper.Sc.Integration.Tds.scproj",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = tdsProjectPath
            };

            //Start the TDS deploy
            using (Process buildProc = Process.Start(psi))
            {
                //Show the output in the console for debugging purposes
                while (!buildProc.HasExited)
                {
                    string output = buildProc.StandardOutput.ReadLine();
                    Debug.WriteLine(output);
                    Console.WriteLine(output);
                }

                //If there are any failues, show the standard error contents
                if (buildProc.ExitCode != 0)
                {
                    Console.WriteLine("\n\nStandard Error:");

                    while (!buildProc.StandardError.EndOfStream)
                    {
                        Console.WriteLine(buildProc.StandardError.ReadLine());
                    }
                }
            }
        }
    }
}
