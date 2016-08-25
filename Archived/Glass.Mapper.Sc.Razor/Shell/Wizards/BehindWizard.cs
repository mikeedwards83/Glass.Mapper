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
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Glass.Mapper.Sc.Razor.Model;
using Sitecore.IO;

namespace Glass.Mapper.Sc.Razor.Shell.Wizards
{
    /// <summary>
    /// Class BehindWizard
    /// </summary>
    public class BehindWizard : AbtractFileCreateWizard<GlassBehindRazor>
    {
        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        protected Edit Namespace { get; set; }
        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        protected Edit AssemblyName { get; set; }

        /// <summary>
        /// The template cs HTML
        /// </summary>
        protected const string TemplateCsHtml = "/sitecore modules/shell/glass/mapper/razor/templates/BehindRazorTemplate.cshtml.temp";
        /// <summary>
        /// The template cs HTML cs
        /// </summary>
        protected const string TemplateCsHtmlCs = "/sitecore modules/shell/glass/mapper/razor/templates/BehindRazorTemplate.cshtml.cs.temp";


        /// <summary>
        /// Called when the active page is changing.
        /// </summary>
        /// <param name="page">The page that is being left.</param>
        /// <param name="newpage">The new page that is being entered.</param>
        /// <returns>True, if the change is allowed, otherwise false.</returns>
        /// <contract>
        ///   <requires name="page" condition="not null" />
        ///   <requires name="newpage" condition="not null" />
        ///   </contract>
        /// <remarks>Set the newpage parameter to another page ID to control the
        /// path through the wizard pages.</remarks>
        protected override bool ActivePageChanging(string page, ref string newpage)
        {
            BackButton.Enabled = true;
            BackButton.Disabled = false;

            if (page == "fileNamePage" && newpage == "fileLocationPage")
            {
                if (FileName.Value.IsNullOrEmpty())
                {
                    SheerResponse.Alert("You must enter a file name");
                    return false;
                }
            }

            if (page == "fileLocationPage" && newpage == "itemLocationPage")
            {
                if (FileExists()) return false;
            }

            if (page == "itemLocationPage" && newpage == "finalPage")
            {
                if (!ItemCanWrite()) return false;
            }

            return base.ActivePageChanging(page, ref newpage);
        }

        /// <summary>
        /// Called when the active page has been changed.
        /// </summary>
        /// <param name="page">The page that has been entered.</param>
        /// <param name="oldPage">The page that was left.</param>
        /// <contract>
        ///   <requires name="page" condition="not null" />
        ///   <requires name="oldPage" condition="not null" />
        ///   </contract>
        protected override void ActivePageChanged(string page, string oldPage)
        {
            if (page == "finalPage")
            {
                BackButton.Disabled = true;

                var parameters = new System.Collections.Specialized.NameValueCollection();

                parameters.Add("className",FileName.Value);
                parameters.Add("namespace",Namespace.Value);
                parameters.Add("assemblyName",AssemblyName.Value);

                CreateCsHtmlFile(parameters, TemplateCsHtml);
                string csPath = FileUtil.MapPath(GetRelativeFilePath()) + ".cs";
                CreateFile(parameters, TemplateCsHtmlCs, csPath);



                GlassBehindRazor item = new GlassBehindRazor();
                item.Name = FileName.Value;
                item.View = GetRelativeFilePath();
                item.Type = "{0}.{1}".Formatted(Namespace.Value, FileName.Value );
                item.Assembly = AssemblyName.Value;
                CreateItem(item);

                Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren");
            }

            base.ActivePageChanged(page, oldPage);

        }
    }
}

