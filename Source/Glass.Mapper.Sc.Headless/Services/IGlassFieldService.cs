using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.Services
{
    public interface IGlassFieldService
    {
        IEnumerable<GlassField> GetItemFields<T>(T item) where T : class;
    }
}