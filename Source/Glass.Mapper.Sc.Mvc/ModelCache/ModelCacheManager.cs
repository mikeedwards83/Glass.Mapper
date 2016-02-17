using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper.Sc.ModelCache
{
    public class ModelCacheManager : IModelCacheManager
    {
        public static FileSystemWatcher FileSystemWatcher { get; private set; }

        public static ConcurrentDictionary<string, Type> CachedTypes { get; private set; }

        public static ConcurrentDictionary<string, string> CachedKeys { get; private set; }

        static ModelCacheManager()
        {
            CachedKeys = new ConcurrentDictionary<string, string>();
            CachedTypes = new ConcurrentDictionary<string, Type>();
            FileSystemWatcher = new FileSystemWatcher(HttpContext.Current.Server.MapPath("/"))
            {
                Filter = "*.cshtml",
                IncludeSubdirectories = true
            };

            FileSystemWatcher.Changed += FileSystemWatcher_Changed;
            FileSystemWatcher.Deleted += FileSystemWatcher_Changed;
            FileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            FileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Type outType;
            CachedTypes.TryRemove(e.OldFullPath, out outType);
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Type outType;
            CachedTypes.TryRemove(e.FullPath, out outType);
        }

        public virtual void Add(string path, Type modelType)
        {
            CachedTypes.TryAdd(path, modelType);
        }

        public virtual Type Get(string path)
        {
            Type retVal;
            return CachedTypes.TryGetValue(path, out retVal) ? retVal : null;
        }

        public string GetKey(string path)
        {
           return CachedKeys.GetOrAdd(path, key => HttpContext.Current.Server.MapPath(key));            
        }
    }
}
