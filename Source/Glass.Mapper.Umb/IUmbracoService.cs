using System;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public interface IUmbracoService: IAbstractService
    {
        object CreateClass(Type type, INode node, bool isLazy = false, bool inferType = false);

    }
}
