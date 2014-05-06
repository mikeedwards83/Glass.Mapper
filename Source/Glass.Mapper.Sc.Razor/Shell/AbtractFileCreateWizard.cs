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
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Class AbtractFileCreateWizard
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbtractFileCreateWizard<T> : WizardForm where T:class
    {

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public string Extension { get; set; }
        /// <summary>
        /// Gets or sets the default file location.
        /// </summary>
        /// <value>The default file location.</value>
        public string DefaultFileLocation { get; set; }
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }
        /// <summary>
        /// Gets or sets the item root.
        /// </summary>
        /// <value>The item root.</value>
        public string ItemRoot { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        protected Edit FileName;
        /// <summary>
        /// The file location treeview
        /// </summary>
        protected TreeviewEx FileLocationTreeview;
        /// <summary>
        /// The file data context
        /// </summary>
        protected DataContext FileDataContext;
        /// <summary>
        /// The item data context
        /// </summary>
        protected DataContext ItemDataContext;
        /// <summary>
        /// The item tree view
        /// </summary>
        protected TreeviewEx ItemTreeView;
        /// <summary>
        /// The master
        /// </summary>
        protected ISitecoreService Master;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbtractFileCreateWizard{T}"/> class.
        /// </summary>
        /// <exception cref="Glass.Mapper.Sc.Razor.RazorException">Exception creating Sitecore Service, have you called the method GlassRazorModuleLoader.Load()?</exception>
        public AbtractFileCreateWizard()
        {
            Extension = "cshtml";
            DefaultFileLocation = "/layouts/razor";
            ItemRoot = "/sitecore/layout/Glass Razor";
            Database = "master";
            try
            {
                Master = new SitecoreService(Database, GlassRazorSettings.ContextName);
            }
            catch (KeyNotFoundException)
            {
                throw new RazorException(
                    "Exception creating Sitecore Service, have you called the method GlassRazorModuleLoader.Load()?");
            }

        }

        /// <summary>
        /// Raises the load event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        /// <remarks>This method notifies the server control that it should perform actions common to each HTTP
        /// request for the page it is associated with, such as setting up a database query. At this
        /// stage in the page lifecycle, server controls in the hierarchy are created and initialized,
        /// view state is restored, and form controls reflect client-side data. Use the IsPostBack
        /// property to determine whether the page is being loaded in response to a client postback,
        /// or if it is being loaded and accessed for the first time.</remarks>
        protected override void OnLoad(EventArgs e)
        {
            FileDataContext.Folder = DefaultFileLocation;

            ItemDataContext.Root = ItemRoot;
            ItemDataContext.Filter = "@@templateId = '{A4F60160-BD14-4471-B362-CB56905E9564}'";

            string locationId = WebUtil.GetQueryString("locationId");
            var master = Sitecore.Configuration.Factory.GetDatabase("master");
            Item location = master.GetItem(ID.Parse(locationId));
            ItemUri folderUri = new ItemUri(location);
            ItemDataContext.SetFolder(folderUri);

            base.OnLoad(e);
        }

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);

            if (!(message.Name == "newfile:refresh"))
            {
                Item item;
                if (!string.IsNullOrEmpty(message["id"]))
                {
                    item = ItemDataContext.GetItem(message["id"]);
                }
                else
                {
                    item = ItemDataContext.GetFolder();
                }
                Dispatcher.Dispatch(message, item);
            }

        }

        /// <summary>
        /// Gets the relative file path.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NullReferenceException">File location item was null</exception>
        public string GetRelativeFilePath()
        {
            Item fileLocationItem = FileLocationTreeview.GetSelectionItem();
            if (fileLocationItem != null)
            {

                string directory = StringUtil.GetString(new[]
						{
							fileLocationItem["Path"]
						});



                string fileName = FileName.Value;
                string ext = fileName.EndsWith(".cshtml") ? "" : Extension;

                return FileUtil.MakePath(directory, "{0}.{1}".Formatted(fileName, ext));

            }
            
            throw new NullReferenceException("File location item was null");
        }

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
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

        /// <summary>
        /// Items the can write.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        protected bool ItemCanWrite()
        {
            Item item = ItemTreeView.GetSelectionItem();
            if (item != null && !item.Access.CanCreate())
            {
                SheerResponse.Alert("You don't have permissions to write to {0}".Formatted(item.Paths.FullPath));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates the cs HTML file.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="templatePath">The template path.</param>
        public void CreateCsHtmlFile(NameValueCollection collection, string templatePath)
        {

            string path = FileUtil.MapPath(GetRelativeFilePath());
            CreateFile(collection, templatePath, path);

        }
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="templatePath">The template path.</param>
        /// <param name="targetPath">The target path.</param>
        public void CreateFile(NameValueCollection collection, string templatePath, string targetPath){

            string fullTemplatePath = FileUtil.MapPath(templatePath);


            string template = File.ReadAllText(fullTemplatePath);

            StringBuilder sb = new StringBuilder(template);
            foreach(var key in collection.AllKeys){
                sb.Replace("{{"+key+"}}", collection[key]);
            }

            File.WriteAllText(targetPath, sb.ToString());
        }


        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void CreateItem(T item)
        {
           Item parent =  ItemTreeView.GetSelectionItem();
           var folder = Master.CreateType<GlassRazorFolder>(parent);

            Master.Create(folder, item);
        }

        

    }
}


