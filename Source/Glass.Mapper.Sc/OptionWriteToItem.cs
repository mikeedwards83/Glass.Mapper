using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Glass.Mapper.Sc
{
    public class  WriteToItemOptions : WriteOptions
    {

        public object Model { get; set; }

        public Item Item { get; set; }

        public WriteToItemOptions() { }

        public WriteToItemOptions(object model, Item item)
        {
            Model = model;
            Item = item;
        }

        public override void Copy(WriteOptions other)
        {
            var local = other as WriteToItemOptions;
            if (local != null)
            {
                this.Model = local.Model;
                this.Item = local.Item;

            }
            base.Copy(other);
        }

        public override void Validate()
        {
            Assert.IsNotNull(Item, "Item is null");
            Assert.IsNotNull(Model, " Model is null");

            base.Validate();
        }
    }
}
