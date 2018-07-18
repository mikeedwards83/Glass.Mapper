using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    public class DisableCache : SettingStack<Cache>
    {
        const string Key = "AADBFF1C-2EE9-475A-B5CA-3D0F32A1CECC";

        public DisableCache() : base(Cache.Disabled, Key)
        {
        }

        public static Cache Current
        {
            get { return GetCurrent(Key); }
        }
    }
    
}
