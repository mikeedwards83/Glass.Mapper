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

namespace Glass.Mapper.Sc.Fields
{
    /// <summary>
    /// Class Image
    /// </summary>
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
        public string Src { get; internal set; }
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

    }
}




