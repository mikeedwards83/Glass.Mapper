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
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using Glass.Mapper.Sites.Sc.Services.BbcNews;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{

    public class RssLanding
    {
        private readonly IRssService _service;

        public virtual string RssUrl { get; set; }

        public virtual int Number { get; set; }

        public virtual string Title { get; set; }

        public RssLanding(IRssService service)
        {
            _service = service;
        }

        public IEnumerable<SyndicationItem> GetArticles()
        {
            var result = _service.GetArticles(RssUrl);
            return result.Take(Number);
        }
    }
}
