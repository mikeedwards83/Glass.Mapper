/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-



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
        //ME - Disabled for NCrunch for now

        [SetUp]
        public void DeployTestItems()
        {

            try
            {
                Sitecore.Context.IsUnitTesting = true;
            }
            catch
            {
            }

#if NCRUNCH
#else

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

#endif
        }
    }
}



