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
using Glass.Mapper.Sc;
using Glass.Mapper.Sites.Sc.Models.Content;
using Sitecore.Data.Comparers;

namespace Glass.Mapper.Sites.Sc.Client.Item
{
    public class EventSortOrder : DefaultComparer
    {
        protected override int DoCompare(Sitecore.Data.Items.Item item1, Sitecore.Data.Items.Item item2)
        {
            if (item1.TemplateName == "Event")
            {
                var evnt1 = item1.GlassCast<Event>();
                var evnt2 = item2.GlassCast<Event>();

                return evnt1.Start.CompareTo(evnt2.Start);
            }
               
            return base.DoCompare(item1, item2);
        }
    }
}
