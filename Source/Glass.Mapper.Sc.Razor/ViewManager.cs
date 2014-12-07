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
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// ViewManager
    /// </summary>
    public class ViewManager
    {

        private static volatile FileSystemWatcher _fileSystemWatcher;
        //  private static volatile FileSystemWatcher _fileWatcher = null;
        private static readonly object _fileWatcherKey = new object();
        private static readonly object _key = new object();
        private static readonly object _viewKey = new object();

        private static volatile Dictionary<string, CachedView> _viewCache;

        /// <summary>
        /// The view loader
        /// </summary>
        static Func<string, string> ViewLoader = viewPath =>
        {
            try
            {
                //TODO: more error catching
                return File.ReadAllText(viewPath);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to read Razor view", ex, typeof(IRazorControl));
            }
            return "";
        };


        /// <summary>
        /// Initializes the <see cref="ViewManager"/> class.
        /// </summary>
        static ViewManager()
        {
            if (_viewCache == null)
            {
                lock (_key)
                {
                    if (_viewCache == null)
                    {
                        _viewCache = new Dictionary<string, CachedView>();
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
                            Sitecore.Diagnostics.Log.Error("Failed to setup Razor file watcher.", ex);
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
        private static void OnChanged(object source, FileSystemEventArgs e)
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
        public static CachedView GetRazorView(string viewPath)
        {
            Sitecore.Diagnostics.Profiler.StartOperation("Razor - Get view {0}".Formatted(viewPath));

            viewPath = GetFullPath(viewPath);

            viewPath = viewPath.ToLower();

            if (!_viewCache.ContainsKey(viewPath))
            {
                Sitecore.Diagnostics.Profiler.StartOperation("Razor - Updating view cache {0}".Formatted(viewPath));
                UpdateCache(viewPath, ViewLoader);
                Sitecore.Diagnostics.Profiler.EndOperation("Razor - Updating view cache {0}".Formatted(viewPath));
            }
            if (!_viewCache.ContainsKey(viewPath))
            {
                Sitecore.Diagnostics.Log.Warn("Failed to find razor view {0}".Formatted(viewPath), string.Empty);
                return null;
            }

            Sitecore.Diagnostics.Profiler.EndOperation("Razor - Get view {0}".Formatted(viewPath));

            return  _viewCache[viewPath];
        }


        private static Regex _placeholders = new Regex("@Placeholder\\(\"([^\"]*)\"\\)");

        /// <summary>
        /// Updates the cache.
        /// </summary>
        /// <param name="viewPath">The view path.</param>
        /// <param name="viewLoader">The view loader.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NullReferenceException">Could not find file {0}..Formatted(viewPath)</exception>
        private static void UpdateCache(string viewPath, Func<string, string> viewLoader)
        {
            viewPath = viewPath.ToLower();

            string finalview = viewLoader(viewPath);


            lock (_viewKey)
            {
                if (finalview.IsNullOrWhiteSpace())
                {
                    Sitecore.Diagnostics.Log.Warn("Could not update cached view because view content was null or empty {0}".Formatted(viewPath), "");
                    _viewCache.Remove(viewPath);
                }
                else{
                    try
                    {
                        var cached = new CachedView();

                        cached.ViewContent = finalview;

                        List<string> placeholders = new List<string>();

                        var matches = _placeholders.Matches(cached.ViewContent);
                        foreach (Match match in matches)
                        {
                            placeholders.Add(match.Groups[1].Value);    
                        }

                        cached.Placeholders = placeholders;
                        var template = RazorEngine.Razor.CreateTemplate(cached.ViewContent);
                        
                        cached.Type = template.GetType().BaseType.IsGenericType
                            ? template.GetType().BaseType.GetGenericArguments()[0]
                            : template.GetType().BaseType;
                        cached.Name = viewPath;
                        _viewCache[viewPath] = cached;
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Failed to update Razor cache.", ex, "");
                        _viewCache.Remove(viewPath);
                    }
                }
            }
        }
    }
}

