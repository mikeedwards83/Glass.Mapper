using System;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Glass.Mapper.Sc.Razor.Model;

namespace Glass.Mapper.Sc.Razor.Shell.Wizards
{
    /// <summary>
    /// Class TypedWizard
    /// </summary>
    public class TypedWizard : AbtractFileCreateWizard<GlassTypedRazor>
    {
        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        protected Edit TypeName { get; set; }
        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        protected Edit AssemblyName { get; set; }

        /// <summary>
        /// The template cs HTML
        /// </summary>
        protected const string TemplateCsHtml = "/sitecore modules/shell/glass/mapper/razor/templates/TypedRazorTemplate.cshtml.temp";


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
            this.BackButton.Enabled = true;
            this.BackButton.Disabled = false;

            if (page == "typeNamePage" && newpage == "fileNamePage")
            {
                string fullType = "{0}, {1}".Formatted(TypeName.Value, AssemblyName.Value);
                Type type = Type.GetType(fullType);

                if (type == null)
                {
                    SheerResponse.Alert("The type you specified could not be found. You may need to compile your application first.");
                    return false;
                }
            }

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
                this.BackButton.Disabled = true;

                var parameters = new System.Collections.Specialized.NameValueCollection();
                parameters["typeName"] = TypeName.Value;

                CreateCsHtmlFile(parameters, TemplateCsHtml);

                GlassTypedRazor item = new GlassTypedRazor();
                item.Name = FileName.Value;
                item.View = GetRelativeFilePath();
                item.Type = TypeName.Value;
                item.Assembly = AssemblyName.Value;

                CreateItem(item);
                
                

                global::Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren");
            }

            base.ActivePageChanged(page, oldPage);
        }
    }
}
