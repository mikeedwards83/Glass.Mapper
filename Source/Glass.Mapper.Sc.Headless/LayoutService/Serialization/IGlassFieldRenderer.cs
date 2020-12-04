using Glass.Mapper.Sc.Fields;
using Sitecore.LayoutService.Serialization;

namespace Glass.Mapper.Sc.LayoutService.Serialization
{
    public interface IGlassFieldRenderer
    {
        FieldRendererResult RenderField(
            GlassField field,
            bool disableExperienceEditorRendering = false);
    }
}