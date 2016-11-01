using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Infrastructure
{
    public class FakeDbField : DbField
    {
        public FakeDbField(ID id) : base(id)
        {
        }

        public FakeDbField(string name) : base(name)
        {
        }

        public FakeDbField(ID id, string value) : base(id)
        {
            this.Value = value;
        }
        public FakeDbField(string name, string value) : base(name)
        {
            this.Value = value;
        }

        public FakeDbField(string name, ID id) : base(name, id)
        {
        }

        public FakeDbField(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }
    }
}
