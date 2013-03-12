using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Razor.Dynamic
{
    public class DynamicField
    {
        string _fieldName;
        Item _item;


        public DynamicField(string fieldName, Item item){
            _fieldName = fieldName;
            _item = item;
        }
        public RawString Editable(){
          
                return new RawString(GetFieldContent());
        }

        public override string ToString()
        {
            return GetFieldContent(false);

        }

        public static implicit operator string(DynamicField field){
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
