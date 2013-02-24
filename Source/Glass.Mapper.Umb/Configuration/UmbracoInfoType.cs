using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Umb.Configuration
{
    public enum UmbracoInfoType
    {
        /// <summary>
        /// No value has been set
        /// </summary>
        NotSet,
        /// <summary>
        /// The item's content path. The property type must be System.String
        /// </summary>
        ContentPath,
        /// <summary>
        /// The item's display name. The property type must be System.String
        /// </summary>
        DisplayName,
        /// <summary>
        /// The item's full path. The property type must be System.String
        /// </summary>
        FullPath,
        /// <summary>
        /// The item's key . The property type must be System.String
        /// </summary>
        Key,
        /// <summary>
        /// The item's media URL. The property type must be System.String
        /// </summary>
        MediaUrl,
        /// <summary>
        /// The item's path. The property type must be System.String
        /// </summary>
        Path,
        /// <summary>
        /// The item's template Id. The property type must be System.Guid
        /// </summary>
        TemplateId,
        /// <summary>
        /// The item's template name. The property type must be System.String
        /// </summary>
        TemplateName,
        /// <summary>
        /// The item's URL. The property type must be System.String
        /// </summary>
        Url,
        /// <summary>
        /// The item's version. The property type must be System.Int32
        /// </summary>
        Version,
        /// <summary>
        /// The item's Name. The property type must be System.String
        /// </summary>
        Name
    }
}
