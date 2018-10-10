using System;
using System.Linq.Expressions;

namespace Glass.Mapper.Sc.Configuration.Fluent
{

    /// <summary>
    /// Used to map item information to a class property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreInfo<T> : AbstractPropertyBuilder<T, SitecoreInfoConfiguration>
    {
        private readonly SitecoreTypeConfiguration _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfo{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreInfo(Expression<Func<T, object>> ex, SitecoreTypeConfiguration owner):base(ex)
        {
            _owner = owner;
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>SitecoreInfo{`0}.</returns>
        public SitecoreInfo<T> InfoType(SitecoreInfoType type)
        {
            Configuration.Type = type;
            SitecoreTypeConfiguration.AssignInfoToKnownProperty(_owner, Configuration);
            return this;
        }
        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>SitecoreInfo{`0}.</returns>
        public SitecoreInfo<T> UrlOptions(SitecoreInfoUrlOptions option)
        {
            Configuration.UrlOptions = option;
            return this;
        }

        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>SitecoreInfo{`0}.</returns>
        public SitecoreInfo<T> UrlOptions(SitecoreInfoMediaUrlOptions option)
        {
            Configuration.MediaUrlOptions = option;
            return this;
        }


    }
}




