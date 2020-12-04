using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.Services
{
    public class GlassFieldService : IGlassFieldService
    {
        public IEnumerable<GlassField> GetItemFields<T>(T item) where T : class
        {
            foreach (var itemPropertyInfo in item.GetType().GetProperties())
            {
                yield return new GlassField
                {
                    Name = itemPropertyInfo.Name,
                    Value = (string)itemPropertyInfo.GetValue(item, null),
                    Type = typeof(T)
            };
            }
        }
    }
}