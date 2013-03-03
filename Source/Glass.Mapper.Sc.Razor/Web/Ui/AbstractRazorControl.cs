using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Sitecore.Web.UI;
using System.Web.UI;
using Sitecore.Data.Items;
using System.Web.Mvc;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public abstract class AbstractRazorControl<T> : WebControl, IRazorControl, global::Sitecore.Layouts.IExpandable
    {

      //  private static volatile FileSystemWatcher _fileWatcher = null;
        private static readonly object _fileWatcherKey = new object();

        private readonly object _key = new object();
        private static readonly object _viewKey = new object();

        private static volatile Dictionary<string, string> _viewCache;
        private static volatile FileSystemWatcher _fileSystemWatcher;


        private ISitecoreService _sitecoreService;

        /// <summary>
        /// A list of placeholders to render on the page.
        /// </summary>
        public IEnumerable<string> Placeholders
        {
            get;
            set;
        }

        /// <summary>
        /// View data
        /// </summary>
        public ViewDataDictionary ViewData { get; private set; }

        /// <summary>
        /// The path to the Razor view
        /// </summary>
        public string View
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the Glass Context to use
        /// </summary>
        public string ContextName
        {
            get;
            set;
        }

        /// <summary>
        /// The model to pass to the Razor view.
        /// </summary>
        public T Model
        {
            get;
            private set;
        }

        public ISitecoreService SitecoreService
        {
            get
            {
                if(_sitecoreService == null)
                    _sitecoreService = new SitecoreService(Sitecore.Context.Database, ContextName);
                
                return _sitecoreService;
            }
        }

        Func<string, string> ViewLoader = viewPath =>
        {
            try
            {    
                //TODO: more error catching
                return File.ReadAllText(viewPath);
            }
            catch (Exception ex)
            {
                global::Sitecore.Diagnostics.Log.Error("Failed to read Razor view", ex, typeof(IRazorControl));
            }
            return "";
        };


        public static string GetFullPath(string viewPath)
        {
             viewPath = viewPath.Replace("/",@"\");
            if (viewPath.StartsWith(@"\")) viewPath = viewPath.Substring(1);
            var path = HttpRuntime.AppDomainAppPath + viewPath; 
            return path;
        }

        public AbstractRazorControl()
        {
            if (_viewCache == null)
            {
                lock (_key)
                {
                    if (_viewCache == null)
                    {
                        _viewCache = new Dictionary<string, string>();
                    }
                }
            }
            if (_fileSystemWatcher == null)
            {
                lock (_fileWatcherKey)
                {
                    if (_fileSystemWatcher == null)
                    {
                        try
                        {
                            _fileSystemWatcher = new FileSystemWatcher(HttpRuntime.AppDomainAppPath, "*.cshtml");
                            _fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
                            _fileSystemWatcher.EnableRaisingEvents = true;
                            _fileSystemWatcher.IncludeSubdirectories = true;
                        }
                        catch (Exception ex)
                        {
                            global::Sitecore.Diagnostics.Log.Error("Failed to setup Razor file watcher.",ex);
                        }
                    }
                }
            }
  
            ViewData = new ViewDataDictionary();
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string path = e.FullPath.ToLower();
            if(_viewCache.ContainsKey(path))
                UpdateCache(path, ViewLoader);
        }

       

        /// <summary>
        /// Put your logic to create your model here
        /// </summary>
        /// <returns></returns>
        public abstract T GetModel();

        /// <summary>
        /// Returns either the data source item or if no data source is specified the context item
        /// </summary>
        /// <returns></returns>
        protected Item GetDataSourceOrContextItem()
        {
            return this.DataSource.IsNullOrEmpty() ? global::Sitecore.Context.Item :
                global::Sitecore.Context.Database.GetItem(this.DataSource);
        }
        
        protected override string GetCachingID()
        {
            return this.View;
        }

        protected override void DoRender(HtmlTextWriter output)
        {
            Model = GetModel();

            var viewContents = GetRazorView(View);

            try
            {
                TemplateModel<T> tModel = new TemplateModel<T>();
                tModel.Control = this;
                tModel.Model = Model;

                string content = global::RazorEngine.Razor.Parse<TemplateModel<T>>(viewContents, tModel, View);

                output.Write(content);
            }
            catch (RazorEngine.Templating.TemplateCompilationException ex)
            {
                StringBuilder errors = new StringBuilder();
                ex.Errors.ForEach(x =>
                                      {
                                          errors.AppendLine("File: {0}".Formatted(View));
                                          errors.AppendLine( x.ErrorText);
                                      });
                 
                 
                throw new RazorException(errors.ToString());
                 
                 
            }
        }
        
        public virtual string GetRazorView(string viewPath)
        {
            string finalview = null;

            viewPath = GetFullPath(viewPath);

            viewPath = viewPath.ToLower();

            if (!_viewCache.ContainsKey(viewPath))
            {
                UpdateCache(viewPath, ViewLoader);
            }
            finalview = _viewCache[viewPath];

            return finalview;
        }

        private static string UpdateCache(string viewPath, Func<string, string> viewLoader)
        {
            viewPath = viewPath.ToLower();

            string finalview = viewLoader(viewPath);
            if (finalview == null) throw new NullReferenceException("Could not find file {0}.".Formatted(viewPath));

            lock (_viewKey)
            {
                _viewCache[viewPath] = finalview;
            }

            return finalview;
        }

        public void Expand()
        {
            if (Placeholders != null)
            {
                foreach (var placeHolderName in Placeholders)
                {
                    global::Sitecore.Web.UI.WebControls.Placeholder holder = new global::Sitecore.Web.UI.WebControls.Placeholder();
                    holder.Key = placeHolderName.ToLower();
                    this.Controls.Add(holder);
                }
            }

            this.Controls.Cast<Control>().Where(x => x is global::Sitecore.Layouts.IExpandable)
                .Cast<global::Sitecore.Layouts.IExpandable>().ToList().ForEach(x => x.Expand());
        }
    }
   
}
