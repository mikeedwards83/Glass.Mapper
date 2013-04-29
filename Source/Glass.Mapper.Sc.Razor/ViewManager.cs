using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Mapper.Sc.Razor
{
    public class ViewManager
    {

        private static volatile FileSystemWatcher _fileSystemWatcher;
        //  private static volatile FileSystemWatcher _fileWatcher = null;
        private static readonly object _fileWatcherKey = new object();
        private readonly object _key = new object();
        private static readonly object _viewKey = new object();

        private static volatile Dictionary<string, string> _viewCache;

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


        public ViewManager()
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
                            global::Sitecore.Diagnostics.Log.Error("Failed to setup Razor file watcher.", ex);
                        }
                    }
                }
            }
  
        }

        /// <summary>
        /// Called when [changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string path = e.FullPath.ToLower();
            if (_viewCache.ContainsKey(path))
                UpdateCache(path, ViewLoader);
        }


        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="viewPath">The view path.</param>
        /// <returns>System.String.</returns>
        public static string GetFullPath(string viewPath)
        {
            viewPath = viewPath.Replace("/", @"\");
            if (viewPath.StartsWith(@"\")) viewPath = viewPath.Substring(1);
            var path = HttpRuntime.AppDomainAppPath + viewPath;
            return path;
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

    }
}
