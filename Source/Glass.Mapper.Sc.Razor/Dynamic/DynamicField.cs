using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Razor.Dynamic
{
    /// <summary>
    /// Class DynamicField
    /// </summary>
    public class DynamicField
    {
        string _fieldName;
        Item _item;


        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicField"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="item">The item.</param>
        public DynamicField(string fieldName, Item item){
            _fieldName = fieldName;
            _item = item;
        }
        /// <summary>
        /// Editables this instance.
        /// </summary>
        /// <returns>RawString.</returns>
        public RawString Editable(){
          
                return new RawString(GetFieldContent());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return GetFieldContent(false);

        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DynamicField"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(DynamicField field){
           return field.ToString();
        }

        /// <summary>
        /// Gets the content of the field.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        /// <returns>System.String.</returns>
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
