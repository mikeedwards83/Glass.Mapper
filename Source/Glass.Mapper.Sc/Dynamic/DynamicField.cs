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
