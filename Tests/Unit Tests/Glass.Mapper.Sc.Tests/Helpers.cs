using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sitecore.Common;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;
using Sitecore.Security.Domains;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Tests
{
    public  class Helpers
    {
        public static Item CreateFakeItem(Dictionary<Guid, string> fields, string name="itemName")
        {
            var id = new ID(Guid.NewGuid());
            var templateId = new ID(Guid.NewGuid());
            var language = Language.Current;
            var version = Sitecore.Data.Version.Latest;
            
            var itemDefinition = new ItemDefinition(id, name, id, ID.Null);
            var fieldList = new FieldList();

            foreach (var fieldId in fields.Keys)
            {
                fieldList.Add(new ID(fieldId), fields[fieldId]);
            }

            var itemData = new ItemData(itemDefinition, language, version, fieldList);
            var item = new Item(id, itemData, new Database("master"));
            return item;
        }

        public static Item CreateFakeItem(Guid fieldId, string fieldValue, string name = "itemName")
        {
            var dic = new Dictionary<Guid, string>();
            dic.Add(fieldId, fieldValue);
            return CreateFakeItem(dic, name);
        }
    

      
    }
    public class FakeAuthorizationProvider : AuthorizationProvider
    {
        public override AccessRuleCollection GetAccessRules(ISecurable entity)
        {
            return new AccessRuleCollection();
        }

        public override void SetAccessRules(ISecurable entity, AccessRuleCollection rules)
        {
        }

        protected override AccessResult GetAccessCore(ISecurable entity, Account account, AccessRight accessRight)
        {
            return new AccessResult(AccessPermission.Allow, new AccessExplanation("fake"));
        }
    }
    public class FakeAccessRightProvider : AccessRightProvider
    {
        public override AccessRight GetAccessRight(string accessRightName)
        {
            return new AccessRight(accessRightName);

        }
    }
    public class FakeDomainProvider : DomainProvider
    {
        public override void AddDomain(string domainName, bool locallyManaged)
        {
            
        }

        public override Domain GetDomain(string name)
        {
            return new Domain(name);
        }

        public override IEnumerable<Domain> GetDomains()
        {
            return new Domain[] {};
        }

        public override void RemoveDomain(string domainName)
        {
           
        }
    }
}
