using System;
using System.Text;
using Sitecore.Web.UI.Pages;
using System.Collections.Specialized;
using System.IO;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore;
using Sitecore.Web.UI.Sheer;
using Sitecore.IO;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework;
using Glass.Mapper.Sc.Razor.Model;
using Sitecore.Web;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Razor.Shell
{
    public abstract class AbtractFileCreateWizard<T> : WizardForm where T:class
    {

        public string Extension { get; set; }
        public string DefaultFileLocation { get; set; }
        public string Database { get; set; }
        public string ItemRoot { get; set; }

        protected Edit FileName;
        protected TreeviewEx FileLocationTreeview;
        protected DataContext FileDataContext;
        protected DataContext ItemDataContext;
        protected TreeviewEx ItemTreeView;

        public AbtractFileCreateWizard()
        {
            Extension = "cshtml";
            DefaultFileLocation = "/layouts/razor";
            ItemRoot = "/sitecore/layout/Glass Razor";
            Database = "master";
        }

        protected override void OnLoad(EventArgs e)
        {
            FileDataContext.Folder = DefaultFileLocation;

            ItemDataContext.Root = ItemRoot;
            ItemDataContext.Filter = "@@templateId = '{A4F60160-BD14-4471-B362-CB56905E9564}'";

            string locationId = WebUtil.GetQueryString("locationId");
            var master = global::Sitecore.Configuration.Factory.GetDatabase("master");
            Item location = master.GetItem(ID.Parse(locationId));
            ItemUri folderUri = new ItemUri(location);
            ItemDataContext.SetFolder(folderUri);

            base.OnLoad(e);
        }

        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);

            Item item = null ;
            if (!(message.Name == "newfile:refresh"))
            {
                if (!string.IsNullOrEmpty(message["id"]))
                {
                    item = ItemDataContext.GetItem(message["id"]);
                }
                else
                {
                    item = ItemDataContext.GetFolder();
                }
                Dispatcher.Dispatch(message, item);
                return;
            }

        }

        public string GetRelativeFilePath()
        {
            Item fileLocationItem = this.FileLocationTreeview.GetSelectionItem();
            if (fileLocationItem != null)
            {

                string directory = StringUtil.GetString(new string[]
						{
							fileLocationItem["Path"]
						});



                string fileName = FileName.Value;
                string ext = fileName.EndsWith(".cshtml") ? "" : Extension;

                return FileUtil.MakePath(directory, "{0}.{1}".Formatted(fileName, ext));

            }
            else throw new NullReferenceException("File location item was null");
            
        }

        protected bool FileExists()
        {
            string fullPath =  FileUtil.MapPath(GetRelativeFilePath());

            if (File.Exists(fullPath))
            {
                SheerResponse.Alert("A file already exists at {0}. Please choose another name.".Formatted(fullPath));
                return true;
            }
            return false;
        }

        protected bool ItemCanWrite()
        {
            Item item = this.ItemTreeView.GetSelectionItem();
            if (item != null && !item.Access.CanCreate())
            {
                SheerResponse.Alert("You don't have permissions to write to {0}".Formatted(item.Paths.FullPath));
                return false;
            }
            return true;
        }

        public void CreateCsHtmlFile(NameValueCollection collection, string templatePath)
        {

            string path = FileUtil.MapPath(GetRelativeFilePath());
            CreateFile(collection, templatePath, path);

        }
        public void CreateFile(NameValueCollection collection, string templatePath, string targetPath){

            string fullTemplatePath = FileUtil.MapPath(templatePath);


            string template = File.ReadAllText(fullTemplatePath);

            StringBuilder sb = new StringBuilder(template);
            foreach(var key in collection.AllKeys){
                sb.Replace("{{"+key+"}}", collection[key]);
            }

            File.WriteAllText(targetPath, sb.ToString());
        }


        public void CreateItem(T item)
        {
           Item parent =  ItemTreeView.GetSelectionItem();
           ISitecoreService service = new SitecoreService(Database);

            GlassRazorFolder folder = service.CreateType<GlassRazorFolder>(parent, false, false);

            service.Create(folder, item);
        }

        

    }
}

