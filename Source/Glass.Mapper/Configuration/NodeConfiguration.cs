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

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class NodeConfiguration
    /// </summary>
    public class NodeConfiguration : AbstractPropertyConfiguration
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        public bool IsLazy { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }


        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new NodeConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as NodeConfiguration;

            config.Id = Id;
            config.IsLazy = IsLazy;
            config.Path = Path;
            config.InferType = InferType;

            base.Copy(copy);
        }
    }
}




