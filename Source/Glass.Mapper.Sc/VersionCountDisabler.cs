using Sitecore.Common;

namespace Glass.Mapper.Sc
{
    public class VersionCountDisabler : Switcher<VersionCountState>
    {
            public VersionCountDisabler():base(VersionCountState.Disabled){}
    }

    public enum VersionCountState
    {
        Default,
        Disabled,
        Enabled
    }

}
