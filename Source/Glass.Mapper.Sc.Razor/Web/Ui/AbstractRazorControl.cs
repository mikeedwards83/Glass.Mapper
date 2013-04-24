using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Glass.Mapper.Profilers;
using Glass.Mapper.Sc.Profilers;
using RazorEngine.Templating;
using Sitecore.Web.UI;
using System.Web.UI;
using Sitecore.Data.Items;
using System.Web.Mvc;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class AbstractRazorControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractRazorControl<T> : WebControl, IRazorControl, global::Sitecore.Layouts.IExpandable
    {

      //  private static volatile FileSystemWatcher _fileWatcher = null;
        private static readonly object _fileWatcherKey = new object();

        private readonly object _key = new object();
        private static readonly object _viewKey = new object();

        private static volatile Dictionary<string, string> _viewCache;
        private static volatile FileSystemWatcher _fileSystemWatcher;

        private ISitecoreContext _sitecoreContext;


        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>The profiler.</value>
        public IPerformanceProfiler Profiler { get; set; }

        /// <summary>
        /// A list of placeholders to render on the page.
        /// </summary>
        /// <value>The placeholders.</value>
        public IEnumerable<string> Placeholders
        {
            get;
            set;
        }

        /// <summary>
        /// View data
        /// </summary>
        /// <value>The view data.</value>
        public ViewDataDictionary ViewData { get; private set; }

        /// <summary>
        /// The path to the Razor view
        /// </summary>
        /// <value>The view.</value>
        public string View
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the Glass Context to use
        /// </summary>
        /// <value>The name of the context.</value>
        public string ContextName
        {
            get;
            set;
        }

        /// <summary>
        /// The model to pass to the Razor view.
        /// </summary>
        /// <value>The model.</value>
        public T Model
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sitecore service.
        /// </summary>
        /// <value>The sitecore service.</value>
        public ISitecoreContext SitecoreContext
        {
            get
            {
                if (_sitecoreContext == null)
                {
                    if (ContextName.IsNotNullOrEmpty())
                        _sitecoreContext = new SitecoreContext(ContextName);
                    else
                        _sitecoreContext = new SitecoreContext();
                }
                return _sitecoreContext;
            }
        }

        /// <summary>
        /// The view loader
        /// </summary>
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


        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="viewPath">The view path.</param>
        /// <returns>System.String.</returns>
        public static string GetFullPath(string viewPath)
        {
             viewPath = viewPath.Replace("/",@"\");
            if (viewPath.StartsWith(@"\")) viewPath = viewPath.Substring(1);
            var path = HttpRuntime.AppDomainAppPath + viewPath; 
            return path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRazorControl{T}"/> class.
        /// </summary>
        public AbstractRazorControl()
        {
            Profiler = new SitecoreProfiler();

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

        /// <summary>
        /// Called when [changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string path = e.FullPath.ToLower();
            if(_viewCache.ContainsKey(path))
                UpdateCache(path, ViewLoader);
        }



        /// <summary>
        /// Put your logic to create your model here
        /// </summary>
        /// <returns>`0.</returns>
        public abstract T GetModel();

        /// <summary>
        /// Returns either the data source item or if no data source is specified the context item
        /// </summary>
        /// <returns>Item.</returns>
        protected Item GetDataSourceOrContextItem()
        {
            return this.DataSource.IsNullOrEmpty() ? global::Sitecore.Context.Item :
                global::Sitecore.Context.Database.GetItem(this.DataSource);
        }

        /// <summary>
        /// Get caching identifier. Must be implemented by controls that supports caching.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <remarks>If an empty string is returned, the control will not be cached.</remarks>
        protected override string GetCachingID()
        {
            return this.View;
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        /// <exception cref="Glass.Mapper.Sc.Razor.RazorException"></exception>
        /// <remarks>When developing custom server controls, you can override this method to generate content for an ASP.NET page.</remarks>
        protected override void DoRender(HtmlTextWriter output)
        {
            Model = GetModel();

            var viewContents = GetRazorView(View);

            try
            {
                Profiler.Start("Razor engine {0}".Formatted(this.View));

                var template = RazorEngine.Razor.CreateTemplate(viewContents, Model) as TemplateBase<T>;

                template.Configure(SitecoreContext, ViewData, this);

                output.Write( template.CastTo<ITemplate<T>>().Run(new ExecuteContext()));

                Profiler.Start("Razor engine {0}".Formatted(this.View));

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

        /// <summary>
        /// Gets the razor view.
        /// </summary>
        /// <param name="viewPath">The view path.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Updates the cache.
        /// </summary>
        /// <param name="viewPath">The view path.</param>
        /// <param name="viewLoader">The view loader.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NullReferenceException">Could not find file {0}..Formatted(viewPath)</exception>
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

        /// <summary>
        /// Expands this instance.
        /// </summary>
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
