using System;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Glass.Mapper.Sc.Razor.Model;

namespace Glass.Mapper.Sc.Razor.Shell.Wizards
{
    public class TypedWizard : AbtractFileCreateWizard<GlassTypedRazor>
    {
        protected Edit TypeName { get; set; }
        protected Edit AssemblyName { get; set; }

        protected const string TemplateCsHtml = "/sitecore modules/shell/glass/mapper/razor/templates/TypedRazorTemplate.cshtml.temp";


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
