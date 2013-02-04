using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Dynamic;
using Sitecore.Data;
using NUnit.Framework;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Integration.Dynamic
{
    public class DynamicCollectionFixture
    {
        Database _db;
        private const string TargetPath = "/sitecore/content/Tests/Dynamic/DynamicCollection/Target";

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            global::Sitecore.Context.Site = global::Sitecore.Sites.SiteContext.GetSite("website");
        }

        #region METHOD - SELECT

        [Test]
        public void First()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.First();
           
            //Assert
            Assert.AreEqual(TargetPath+"/Child1", child.Path);
        }

        [Test]
        public void First_WithPredicate()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);
            string name = "Child2";
            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.First(Dy.Fc(x => x.Name == "Child2"));

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.Path);
        }

        [Test]
        public void FirstOrDefault()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.FirstOrDefault();

            //Assert
            Assert.AreEqual(TargetPath+"/Child1", child.Path);
        }

        [Test]
        public void FirstOrDefault_WithPredicate()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);
            string name = "Child2";
            dynamic d = new DynamicItem(item);

            //Act
            var func = Dy.Fc(x => x.Name == "Child2");

            var child = d.Children.First(func);

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.Path);
        }

      

        [Test]
        public void Last()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.Last();

            //Assert
            Assert.AreEqual(TargetPath+"/Child3", child.Path);
        }

        [Test]
        public void Last_WithPredicate()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.Last(Dy.Fc(x=>x.Name == "Child2"));

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.Path);
        }


        [Test]
        public void LastOrDefault()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.LastOrDefault();

            //Assert
            Assert.AreEqual(TargetPath+"/Child3", child.Path);
        }

        [Test]
        public void LastOrDefault_WithPredicate()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.LastOrDefault(Dy.Fc(x => x.Name == "Child2"));

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.Path);
        }


        [Test]
        public void ElementAt()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.ElementAt(1);

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.Path);
        }


        [Test]
        public void Where()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.Where(Dy.Fc(x => x.Name == "Child2")).First();

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.Path);
        }

        [Test]
        public void Any()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result1 = d.Children.Any(Dy.Fc(x => x.Name == "Child2"));
            var result2 = d.Children.Any(Dy.Fc(x => x.Name == "NotThere"));

            //Assert
            Assert.AreEqual(true, result1);
            Assert.AreEqual(false, result2);

        }

        [Test]
        public void All()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);
            
            //Act
            var result1 = d.Children.All(Dy.Fc(x => x.Name.StartsWith("Child")));
            var result2 = d.Children.All(Dy.Fc(x => x.Name.StartsWith("Child2")));

            //Assert
            Assert.AreEqual(true, result1);
            Assert.AreEqual(false, result2);

        }

        [Test]
        public void Select_ToKnown()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var children = d.Children.Select(Dy.FcT<Known>(x => new Known { Name = x.Name })) as IEnumerable<Known>;
            var child = children.First();
            //Assert
            
            Assert.AreEqual("Child1", child.Name);
        }

        [Test]
        public void Select_ToDynamic()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var children = d.Children.Select(Dy.FcT(x => new { Name = x.Name })) as IEnumerable<dynamic>;
            var child = children.First();
            //Assert

            Assert.AreEqual("Child1", child.Name);
        }

        [Test]
        public void Where_Select_First_DyamicsTypes()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var child = d.Children.Where(Dy.Fc(x=> x.Name == "Child2")).Select(Dy.FcT(x => new { NewPath = x.Path, NewName = x.Name })).First();

            //Assert
            Assert.AreEqual(TargetPath+"/Child2", child.NewPath);
            Assert.AreEqual("Child2", child.NewName);

        }


        [Test]
        public void Count()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var count = d.Children.Count();

            //Assert
            Assert.AreEqual(3, count);
        }

        [Test]
        public void ForEach()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var children = d.Children;

            //Assert
            int total = 0;
            foreach (var child in children)
            {
                total++;
                switch (total)
                {
                    case 1:
                        Assert.AreEqual(TargetPath+"/Child1", child.Path);
                        break;
                    case 2:
                        Assert.AreEqual(TargetPath + "/Child2", child.Path);
                        break;
                    case 3:
                        Assert.AreEqual(TargetPath + "/Child3", child.Path);
                        break;
                }
            }

            Assert.AreEqual(3, total);
                
        }
        #endregion

        public class Known
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }
    }
}
