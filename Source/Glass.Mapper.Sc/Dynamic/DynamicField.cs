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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Dynamic
{
    public class DynamicField
    {
        string _fieldName;
        Item _item;


        public DynamicField(string fieldName, Item item)
        {
            _fieldName = fieldName;
            _item = item;
        }
        public string Editable()
        {

            return GetFieldContent();
        }

        public override string ToString()
        {
            return GetFieldContent(false);

        }

        public static implicit operator string(DynamicField field)
        {
            return field.ToString();
        }

        private string GetFieldContent(bool editable = true)
        {
            FieldRenderer render = new FieldRenderer();
            render.FieldName = _fieldName;
            render.Item = _item;
            render.DisableWebEditing = !editable;
            return render.Render();
        }
    }
}



