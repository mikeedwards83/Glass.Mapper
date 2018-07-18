using System;
using Sitecore.Data.Fields;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Fields
{
    /// <summary>
    /// Class Image
    /// </summary>
    [Serializable]
    public class Image
    {
        /// <summary>
        /// Gets or sets the alt.
        /// </summary>
        /// <value>The alt.</value>
        public string Alt { get; set; }
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public string Border { get; set; }
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>
        public string Class { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the H space.
        /// </summary>
        /// <value>The H space.</value>
        public int HSpace { get; set; }
        /// <summary>
        /// Gets the SRC.
        /// </summary>
        /// <value>The SRC.</value>
        public string Src { get; set; }
        /// <summary>
        /// Gets or sets the V space.
        /// </summary>
        /// <value>The V space.</value>
        public int VSpace { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the media id.
        /// </summary>
        /// <value>The media id.</value>
        public Guid MediaId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        public Language Language { get; set; }

        /// <summary>
        /// Indicates if the Sitecore Media Item exists in the database
        /// </summary>
        public bool MediaExists { get; set; }
    }
}




