
using Sitecore.Common;

namespace Glass.Mapper.Sc
{
    public class VersionCountDisabler : Switcher<VersionCountState>
    {
        public VersionCountDisabler() : this(VersionCountState.Disabled) { }
        public VersionCountDisabler(VersionCountState state) : base(state) { }
    }

    public enum VersionCountState
    {
        Default,
        Disabled,
        Enabled
    }

}
