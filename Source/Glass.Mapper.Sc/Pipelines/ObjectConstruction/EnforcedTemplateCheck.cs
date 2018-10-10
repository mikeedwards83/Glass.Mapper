using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class EnforcedTemplateCheck : AbstractObjectConstructionTask
    {
        private static ConcurrentDictionary<string, bool> _cache;

        static EnforcedTemplateCheck()
        {
            _cache = new ConcurrentDictionary<string, bool>();
        }

        public EnforcedTemplateCheck()
        {
            Name = "EnforcedTemplateCheck";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null)
            {
                var options = args.Options as GetItemOptions;

                if (options != null)
                {
                    PerformTemplateCheck(args, options.TemplateId, options.EnforceTemplate);
                }
                else
                {
                    base.Execute(args);
                }
            }
        }

        protected virtual void PerformTemplateCheck(ObjectConstructionArgs args, ID templateId, SitecoreEnforceTemplate enforceTemplate)
        {
            if (!enforceTemplate.IsEnabled())
            {
                base.Execute(args);
            }
            else
            {
                if (ID.IsNullOrEmpty(templateId))
                {
                    throw new MapperException("Cannot EnforceTemplate with Null TemplateID");
                }
                else
                {

                    var scArgs = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;

                    var key = "{0} {1} {2}".Formatted(templateId, scArgs.Item.TemplateID,
                        enforceTemplate);
                    var result = false;

                    if (_cache.ContainsKey(key))
                    {
                        result = _cache[key];
                    }
                    else
                    {
                        var item = scArgs.Item;

                        if (enforceTemplate == SitecoreEnforceTemplate.TemplateAndBase)
                        {
                            result = TemplateAndBaseCheck(item.Template, templateId);
                        }
                        else if (enforceTemplate == SitecoreEnforceTemplate.Template)
                        {
                            result = item.TemplateID == templateId;
                        }

                        _cache.TryAdd(key, result);
                    }

                    if (result)
                    {
                        base.Execute(args);
                    }
                }
            }
        }

        protected virtual bool TemplateAndBaseCheck(TemplateItem template, ID templateId)
        {
            if (template.ID == templateId)
            {
                return true;
            }

            return template.BaseTemplates.Any(baseTemplate => TemplateAndBaseCheck(baseTemplate, templateId));
        }
    }
}
