namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Enum SitecoreInfoType
    /// </summary>
    public enum SitecoreInfoType
    {
        /// <summary>
        /// No value has been set
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// The item's content path. The property type must be System.String
        /// </summary>
        ContentPath =1,
        /// <summary>
        /// The item's display name. The property type must be System.String
        /// </summary>
        DisplayName = 2,
        /// <summary>
        /// The item's full path. The property type must be System.String
        /// </summary>
        FullPath =3,
        /// <summary>
        /// The item's key . The property type must be System.String
        /// </summary>
        Key =4,
        /// <summary>
        /// The item's ItemUri . The property type must be Sitecore.Data.ItemUri
        /// </summary>
        ItemUri =5,
        /// <summary>
        /// The item's media URL. The property type must be System.String
        /// </summary>
        MediaUrl =6,
        /// <summary>
        /// The item's path. The property type must be System.String
        /// </summary>
        Path=7,
        /// <summary>
        /// The item's template Id. The property type must be System.Guid
        /// </summary>
        TemplateId=8,
        /// <summary>
        /// The item's template name. The property type must be System.String
        /// </summary>
        TemplateName=9,
        /// <summary>
        /// The item's URL. The property type must be System.String
        /// </summary>
        Url=10,
        /// <summary>
        /// The item's version. The property type must be System.Int32
        /// </summary>
        Version=11,
        /// <summary>
        /// The item's Name. The property type must be System.String
        /// </summary>
        Name=12,
        /// <summary>
        /// The items language. The property type must be Sitecore.Globalization.Language
        /// </summary>
        Language=13,
        
        /// <summary>
        /// Gets the Base Template IDs - does not return the template is. The property type must be IEnumerable&lt;Guid&gt;
        /// </summary>
        BaseTemplateIds=14,

#if SC81 || SC82 || SC90 || SC91 || SC92
        /// <summary>
        /// Gets  the original language.
        /// </summary>
        OriginalLanguage =15,
        /// <summary>
        /// Get the originator Id
        /// </summary>
        OriginatorId = 16
#endif

    }
}




