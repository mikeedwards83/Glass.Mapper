using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Glass.Mapper.Sc.Razor.Model;
using Sitecore.IO;

namespace Glass.Mapper.Sc.Razor.Shell.Wizards
{
    public class BehindWizard : AbtractFileCreateWizard<GlassBehindRazor>
    {
        protected Edit Namespace { get; set; }
        protected Edit AssemblyName { get; set; }

        protected const string TemplateCsHtml = "/sitecore modules/shell/glass/mapper/razor/templates/BehindRazorTemplate.cshtml.temp";
        protected const string TemplateCsHtmlCs = "/sitecore modules/shell/glass/mapper/razor/templates/BehindRazorTemplate.cshtml.cs.temp";


        protected override bool ActivePageChanging(string page, ref string newpage)
        {
            this.BackButton.Enabled = true;
            this.BackButton.Disabled = false;

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

        protected override void ActivePageChanged(string page, string oldPage)
        {
            if (page == "finalPage")
            {
                this.BackButton.Disabled = true;

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

                global::Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren");
            }

            base.ActivePageChanged(page, oldPage);

        }
    }
}
