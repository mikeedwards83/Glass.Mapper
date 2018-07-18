using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using NSubstitute;
using NUnit.Framework;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.FakeDb;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Version = Sitecore.Data.Version;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class SitecoreServiceLegacyFixture
    {

        #region Method - AddVersion

        [Test]
        public void AddVersion_NewVersionCreated()
        {
            //Assign

            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);

                var oldVersion = service.GetItem<StubClass>(path);

                //Act
                using (new SecurityDisabler())
                {
                    var newVersion = service.AddVersion(oldVersion);

                    //clean up
                    var item = database.Database.GetItem(path, newVersion.Language, Version.Parse(newVersion.Version));
                    item.Versions.RemoveVersion();
                    //Assert
                    Assert.AreEqual(oldVersion.Version + 1, newVersion.Version);
                }
            }
        }

        #endregion

        #region Method - GetItem


        [Test]
        public void GetItem_UsingItemIdAsItem_ReturnsItem()
        {
            //Assign


            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));


                var service = new SitecoreService(database.Database);

                //Act
                var result = service.GetItem<Item>(id);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.ID.Guid);
            }
        }


        [Test]
        public void GetItem_UsingItemId_ReturnsItem()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));


                var service = new SitecoreService(database.Database);

                //Act
                var result = (StubClass)service.GetItem<StubClass>(id);

                //Assert
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void GetItem_UsingItemId_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClassWithProperty)));


                var service = new SitecoreService(database.Database);

                //Act
                var result = service.GetItem<StubClassWithProperty>(id);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Target", result.Name);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test" }
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var language = LanguageManager.GetLanguage("af-ZA");


                //Act
                var result = service.GetItem<StubClass>(id, language);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage1Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;

                //Act
                var result = service.GetItem<StubClass, int>(id, language, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage2Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;
                string param2 = "2param";

                //Act
                var result = service.GetItem<StubClass, int, string>(id, language, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage3Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime>(id, language, param1, param2, param3);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage4Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;
                bool param4 = true;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime, bool>(id, language, param1, param2,
                    param3, param4);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");

                //Act
                var result = service.GetItem<StubClass>(path, language);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage1Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;

                //Act
                var result = service.GetItem<StubClass, int>(path, language, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage2Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;
                string param2 = "2param";

                //Act
                var result = service.GetItem<StubClass, int, string>(path, language, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage3Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime>(path, language, param1, param2, param3);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage4Parameter_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;
                bool param4 = true;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime, bool>(path, language, param1, param2,
                    param3, param4);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguageVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                //Act
                var result = service.GetItem<StubClass>(id, language, version);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage1ParameterVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                Version version = Version.Parse(1);
                var language = LanguageManager.GetLanguage("af-ZA");
                int param1 = 1;

                //Act
                var result = service.GetItem<StubClass, int>(id, language, version, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage2ParameterVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;
                string param2 = "2param";

                //Act
                var result = service.GetItem<StubClass, int, string>(id, language, version, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage3ParameterVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime>(id, language, version, param1, param2,
                    param3);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemIdLanguage4ParameterVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;
                bool param4 = true;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime, bool>(id, language, version, param1,
                    param2, param3, param4);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(id, result.Id);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguageVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);

                //Act
                var result = service.GetItem<StubClass>(path, language, version);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage1ParameterVersion_ReturnsItemName()
        {
            //Assign
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;

                //Act
                var result = service.GetItem<StubClass, int>(path, language, version, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage2ParameterVersion_ReturnsItemName()
        {
            //Assign
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;
                string param2 = "2param";

                //Act
                var result = service.GetItem<StubClass, int, string>(path, language, version, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage3ParameterVersion_ReturnsItemName()
        {
            //Assign
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime>(path, language, version, param1, param2,
                    param3);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(version.Number, result.Version);
            }
        }

        [Test]
        public void GetItem_UsingItemPathLanguage4ParameterVersion_ReturnsItemName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string path = "/sitecore/content/Target";
                var language = LanguageManager.GetLanguage("af-ZA");
                Version version = Version.Parse(1);
                int param1 = 1;
                string param2 = "2param";
                DateTime param3 = DateTime.Now;
                bool param4 = true;

                //Act
                var result = service.GetItem<StubClass, int, string, DateTime, bool>(path, language, version, param1,
                    param2, param3, param4);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(path, result.Path);
                Assert.AreEqual(language, result.Language);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
                Assert.AreEqual(version.Number, result.Version);
            }
        }



        #endregion

        #region Method - Query

        [Test]
        public void Query_ReturnsItemsBeneathFolder_ThreeItemsReturned()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbItem("Child1"),
                    new DbItem("Child2"),
                    new DbItem("Child3")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);
                string query = "/sitecore/content/Target/*";

                //Act
                var result = service.Query<StubClass>(query);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count());
            }
        }

        [Test]
        public void Query_ReturnsItemsBeneathFolderBasedOnLanguage_TwoItemsReturned()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    new DbItem("Child1")
                    {
                        new DbField("Value")
                        {
                            {"af-ZA", 1, "test"}
                        }
                    },
                    new DbItem("Child2")
                    {
                        new DbField("Value")
                        {
                            {"af-ZA", 1, "test"}
                        }
                    },

                    new DbItem("Child3")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

               
                    string query = "/sitecore/content/Target/*";
                    var language = LanguageManager.GetLanguage("af-ZA");

                    //Act
                    var result = service.Query<StubClass>(query, language);

                    //Assert
                    Assert.IsNotNull(result);
                    Assert.AreEqual(2, result.Count());
            }
        }

        #endregion

        #region Method - Save

        [Test]
        public void Save_ItemDisplayNameChanged_SavesDisplayName()
        {
            //Assign

            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubSaving)));
                var service = new SitecoreService(database.Database);

                var itemPath = "/sitecore/content/Target";
                string expected = "new name";

                var currentItem = database.Database.GetItem(itemPath);

                var cls = new StubSaving();
                cls.Id = currentItem.ID.Guid;

                //setup item
                using (new SecurityDisabler())
                {
                    currentItem.Editing.BeginEdit();
                    currentItem[Global.Fields.DisplayName] = "old name";
                    currentItem.Editing.EndEdit();
                }


                using (new SecurityDisabler())
                {
                    //Act
                    cls.Name = expected;
                    service.Save(cls);

                    //Assert
                    var newItem = database.GetItem(itemPath);

                    Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);
                }

            }
        }

        [Test]
        public void Save_ItemDisplayNameChangedUsingProxy_SavesDisplayName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubSaving)));
                var service = new SitecoreService(database.Database);

                var itemPath = "/sitecore/content/Target";
                string expected = "new name";

                var currentItem = database.Database.GetItem(itemPath);


                //setup item
                using (new SecurityDisabler())
                {
                    currentItem.Editing.BeginEdit();
                    currentItem[Global.Fields.DisplayName] = "old name";
                    currentItem.Editing.EndEdit();
                }

                var cls = service.GetItem<StubSaving>(itemPath, true);

                using (new SecurityDisabler())
                {
                    //Act
                    cls.Name = expected;
                    service.Save(cls);

                    //Assert
                    var newItem = database.Database.GetItem(itemPath);

                    Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);

                    Assert.IsTrue(cls is StubSaving);
                    Assert.AreNotEqual(typeof(StubSaving), cls.GetType());
                }

            }
        }

        [Test]
        public void Save_ItemDisplayNameChangedUsingProxyUsingInterface_SavesDisplayName()
        {
            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubSaving)));
                var service = new SitecoreService(database.Database);

                var itemPath = "/sitecore/content/Target";
                string expected = "new name";

                var currentItem = database.Database.GetItem(itemPath);

                //setup item
                using (new SecurityDisabler())
                {
                    currentItem.Editing.BeginEdit();
                    currentItem[Global.Fields.DisplayName] = "old name";
                    currentItem.Editing.EndEdit();
                }

                var cls = service.GetItem<IStubSaving>(itemPath);
                using (new SecurityDisabler())
                {
                    //Act
                    cls.Name = expected;
                    service.Save(cls);

                    //Assert
                    var newItem = database.Database.GetItem(itemPath);

                    Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);

                    Assert.IsTrue(cls is IStubSaving);
                    Assert.AreNotEqual(typeof(IStubSaving), cls.GetType());
                }

            }

        }

        #endregion

        #region Method - CreateTypes

        [Test]
        public void CreateTypes_TwoItems_ReturnsTwoClasses()
        {
            //Assign

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target1"),
                new Sitecore.FakeDb.DbItem("Target2")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var result1 = database.GetItem("/sitecore/content/Target1");
                var result2 = database.GetItem("/sitecore/content/Target2");

                //Act
                var results =
                    service.CreateTypes(typeof(StubClass), () => new[] { result1, result2 }, false, false) as
                        IEnumerable<StubClass>;

                //Assert
                Assert.AreEqual(2, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
                Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
            }
        }

        [Test]
        public void CreateTypes_TwoItemsOnly1WithLanguage_ReturnsOneClasses()
        {
            //Assign
            var language = LanguageManager.GetLanguage("af-ZA");

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target1")
                {
                    new DbField("Value")
                    {
                        {"af-ZA", 1, "test"}
                    }
                }
                ,
                new Sitecore.FakeDb.DbItem("Target2")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var result1 = database.Database.GetItem("/sitecore/content/Target1", language);
                var result2 = database.Database.GetItem("/sitecore/content/Target2", language);

                //Act
                var results =
                    service.CreateTypes(typeof(StubClass), () => new[] { result1, result2 }, false, false) as
                        IEnumerable<StubClass>;

                //Assert
                Assert.AreEqual(1, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            }
        }

        #endregion

        #region Method - CreateType

        [Test]
        public void CreateType_NoConstructorArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                //Act
                var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
            }
        }

        [Test]
        public void CastToType_NoConstructorArgsTyped_ReturnsSameAsCreateType()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                //Act
                var result1 = service.Cast<StubClass>(item, false, false);
                var result2 = service.CreateType<StubClass>(item, false, false);

                //Assert
                Assert.IsNotNull(result1);
                Assert.AreEqual(result2.Id, result1.Id);
                Assert.AreEqual(result2.Name, result1.Name);
            }
        }

        [Test]
        public void CreateType_NoConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                //Act
                var result = service.CreateType<StubClass>(item, false, false);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
            }
        }

        [Test]
        public void CreateType_OneConstructorArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;

                //Act
                var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
            }
        }

        [Test]
        public void CreateType_OneConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;

                //Act
                var result = service.CreateType<StubClass, int>(item, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
            }
        }

        [Test]
        public void CreateType_TwoConstructorArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";

                //Act
                var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
            }
        }

        [Test]
        public void CreateType_TwoConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                var param1 = 456;
                var param2 = "hello world";

                //Act
                var result = service.CreateType<StubClass, int, string>(item, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
            }
        }

        [Test]
        public void CreateType_ThreeConstructorArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;


                //Act
                var result =
                    (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2, param3);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
            }
        }

        [Test]
        public void CreateType_ThreeConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;


                //Act
                var result = service.CreateType<StubClass, int, string, DateTime>(item, param1, param2, param3, false,
                    false);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
            }
        }

        [Test]
        public void CreateType_FourConstructorArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;
                var param4 = true;

                //Act
                var result =
                    (StubClass)
                    service.CreateType(typeof(StubClass), item, false, false, param1, param2, param3, param4);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
            }
        }

        [Test]
        public void CreateType_FourConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;
                var param4 = true;

                //Act
                var result = service.CreateType<StubClass, int, string, DateTime, bool>(item, param1, param2, param3,
                    param4, false, false);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
            }
        }

        [Test]
        public void CreateType_FiveConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;
                var param4 = true;
                var param5 = TimeSpan.Zero;

                //Act
                var result = service.CreateType<StubClass, int, string, DateTime, bool, TimeSpan>(item, param1, param2,
                    param3, param4, param5);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
                Assert.AreEqual(param5, result.Param5);
            }
        }

        [Test]
        public void CreateType_OneConstructor_FromParamsArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;

                //Act
                var result = service.CreateType<StubClass>(item, false, false, param1);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
            }
        }

        [Test]
        public void CreateType_TwoConstructor_FromParamsArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";

                //Act
                var result = service.CreateType<StubClass>(item, false, false, param1, param2);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
            }
        }

        [Test]
        public void CreateType_ThreeConstructor_FromParamsArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;

                //Act
                var result = service.CreateType<StubClass>(item, false, false, param1, param2, param3);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
            }
        }

        [Test]
        public void CreateType_FourConstructor_FromParamsArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;
                var param4 = true;

                //Act
                var result = service.CreateType<StubClass>(item, false, false, param1, param2, param3, param4);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
            }
        }

        [Test]
        public void CreateType_FiveConstructor_FromParamsArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                var param1 = 456;
                var param2 = "hello world";
                DateTime param3 = DateTime.Now;
                var param4 = true;
                var param5 = TimeSpan.Zero;

                //Act
                var result = service.CreateType<StubClass>(item, false, false, param1, param2, param3, param4, param5);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(item.ID.Guid, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
                Assert.AreEqual(param5, result.Param5);
            }
        }

        [Test]
        // [ExpectedException(typeof(NotSupportedException))]
        public void CreateType_TooManyConstructor_FromParamsArgs_Excepts()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");


                //Act
                StubClass result = null;
                Assert.Throws<NotSupportedException>(() =>
                {
                    result = service.CreateType<StubClass>(item, false, false, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
                });
                //Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void CreateType_IncorrectConstructor_FromParamsArgs_ReturnsItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                //Act
                try
                {
                    var result = service.CreateType<StubClass>(item, false, false, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                    //Assert
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex is MapperException);
                    Assert.IsTrue(ex.InnerException is ArgumentNullException);
                }
            }
        }

        [Test]
        public void CreateType_IncorrectConstructor_NoParameterlessConstructor_ThrowsException()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                var item = database.GetItem("/sitecore/content/Target");

                //Act
                try
                {
                    var result = service.CreateType<StubClass>(item, false, false, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                    //Assert
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex is MapperException);
                    Assert.IsTrue(ex.InnerException is ArgumentNullException);
                }
            }
        }

        #endregion

        #region Method - Create


        [Test]
        public void Create_TypeNotPreloaded_CreatesANewItem()
        {
            //Assign

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChild";


                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                Assert.AreEqual(0, context.TypeConfigurations.Count);
                var parent = service.GetItem<StubClass>(parentPath);

                var child = new StubClassNotPreloaded();
                child.Name = "newChild";

                //Act
                using (new SecurityDisabler())
                {
                    service.Create(parent, child);
                }

                //Assert
                var newItem = database.GetItem(childPath);

                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }

                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(child.Id, newItem.ID.Guid);
            }
        }

        [Test]
        public void Create_CreatesANewItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChild";

                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                var parent = service.GetItem<StubClass>(parentPath);

                var child = new StubClass();
                child.Name = "newChild";

                //Act
                using (new SecurityDisabler())
                {
                    service.Create(parent, child);
                }

                //Assert
                var newItem = database.GetItem(childPath);

                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }

                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(child.Id, newItem.ID.Guid);
            }
        }

        [Test]
        public void Create_CreatesANewItem_WithSpecificId()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChild";
                var id = Guid.NewGuid();

                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                var parent = service.GetItem<StubClass>(parentPath);

                var child = new StubClass();
                child.Id = id;
                child.Name = "newChild";

                //Act
                using (new SecurityDisabler())
                {
                    service.Create(parent, child);
                }

                //Assert
                var newItem = database.GetItem(childPath);

                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }

                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(id, newItem.ID.Guid);
            }
        }

        [Test]
        public void Create_UsingInterface_CreatesANewItem()
        {
            //Assign

            string fieldValue = Guid.NewGuid().ToString();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target"),
                new DbTemplate(new ID(StubInterfaceAutoMappedConst.TemplateId))
                {
                    {"StringField",""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubInterfaceAutoMapped)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChild";

                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                var parent = service.GetItem<StubClass>(parentPath);

                var child = Substitute.For<StubInterfaceAutoMapped>();
                child.Name = "newChild";
                child.StringField = fieldValue;

                //Act
                using (new SecurityDisabler())
                {
                    service.Create(parent, child);
                }

                //Assert
                var newItem = database.GetItem(childPath);

                Assert.AreEqual(fieldValue, newItem["StringField"]);

                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }

                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(child.Id, newItem.ID.Guid);
            }
        }

        [Test]
        public void Create_UsingInterfaceAndName_CreatesANewItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target"),
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChild";


                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                var parent = service.GetItem<StubClass>(parentPath);

                //Act
                StubClass child = null;
                using (new SecurityDisabler())
                {
                    child = service.Create<StubClass, StubClass>(parent, "newChild");
                }

                //Assert
                var newItem = database.GetItem(childPath);

                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }

                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(child.Id, newItem.ID.Guid);
            }
        }

        [Test]

        //TODO: fix, waiting on fix https://github.com/sergeyshushlyapin/Sitecore.FakeDb/issues/138
        public void Create_UsingInterfaceAndNameAndLanguage_CreatesANewItem()
        {
            //Assign
            Language lang = LanguageManager.GetLanguage("af-ZA");
            ID templateId = ID.NewID;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", ID.NewID),
            })
            {
                var dependencyResolver = Utilities.CreateStandardResolver();
                dependencyResolver.ObjectConstructionFactory.Replace<ItemVersionCountByRevisionTask, ItemVersionCountTask>(()=>new ItemVersionCountTask());

                var context = Context.Create(dependencyResolver);
               
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChild";
                string name = "newChild";

                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                var parent = service.GetItem<StubClass>(parentPath);

                //Act
                StubClass child = null;
                using (new SecurityDisabler())
                {
                    child = service.Create<StubClass, StubClass>(parent, name, lang);
                }

                //Assert
                var newItem = database.Database.GetItem(childPath, lang);

                Assert.AreEqual(name, child.Name);
                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(child.Id, newItem.ID.Guid);
                Assert.AreEqual(1, newItem.Versions.Count);
                Assert.AreEqual(1, newItem.Versions.Count);
                Assert.AreEqual(lang, child.Language);


                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }


            }
        }

        [Test]
        public void Create_AutoMappedClass_CreatesANewItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target"),
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClassAutoMapped)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/newChildAutoMapped";

                using (new SecurityDisabler())
                {
                    var parentItem = database.GetItem(parentPath);
                    parentItem.DeleteChildren();
                }

                var parent = service.GetItem<StubClassAutoMapped>(parentPath);

                var child = new StubClassAutoMapped();
                child.Name = "newChildAutoMapped";

                //Act
                using (new SecurityDisabler())
                {
                    service.Create(parent, child);
                }

                //Assert
                var newItem = database.GetItem(childPath);

                using (new SecurityDisabler())
                {
                    newItem.Delete();
                }

                Assert.AreEqual(child.Name, newItem.Name);
                Assert.AreEqual(child.Id, newItem.ID.Guid);
            }
        }

        #endregion

        #region Method - Delete

        [Test]
        public void Delete_RemovesItemFromDatabase()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target"),
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string parentPath = "/sitecore/content/Target";
                string childPath = "/sitecore/content/Target/Child";

                var parent = database.GetItem(parentPath);

                //clean up any outstanding items
                Item child;
                using (new SecurityDisabler())
                {
                    parent.DeleteChildren();


                    child = parent.Add("Child", new TemplateID(new ID(StubClass.TemplateId)));
                }
                Assert.IsNotNull(child);

                var childClass = service.GetItem<StubClass>(childPath);
                Assert.IsNotNull(childClass);

                //Act
                using (new SecurityDisabler())
                {
                    service.Delete(childClass);
                }

                //Assert
                var newItem = database.GetItem(childPath);

                Assert.IsNull(newItem);
            }
        }

        #endregion

        #region Method - Move

        [Test]
        public void Move_MovesItemFromParent1ToParent2()
        {
            //Assign

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target1")
                {
                    new Sitecore.FakeDb.DbItem("Target"),
                },
                new Sitecore.FakeDb.DbItem("Target2"),
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));
                var service = new SitecoreService(database.Database);

                string parent1Path = "/sitecore/content/Target1";
                string parent2Path = "/sitecore/content/Target2";
                string targetPath = "/sitecore/content/Target1/Target";
                string targetNewPath = "/sitecore/content/Target2/Target";


                var parent1 = database.GetItem(parent1Path);
                var parent2 = database.GetItem(parent1Path);
                var target = database.GetItem(targetPath);

                Assert.AreEqual(parent1.ID, target.Parent.ID);

                var parent2Class = service.GetItem<StubClass>(parent2Path);
                var targetClass = service.GetItem<StubClass>(targetPath);

                //Act
                using (new SecurityDisabler())
                {
                    service.Move(targetClass, parent2Class);
                }

                //Assert
                var targetNew = database.GetItem(targetNewPath);

                Assert.IsNotNull(targetNew);
                using (new SecurityDisabler())
                {
                    targetNew.MoveTo(parent1);
                }

            }
        }

        #endregion

        #region OnDemand Mapping

        [Test]
        public void OnDemandMapping_AutomaticallyMapsProperties()
        {
            //Assign
            var id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"StringField", ""},
                    {"DateField", ""}
                },
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database);
                string text = "test text 1";
                string path = "/sitecore/content/Target";
                DateTime date = new DateTime(2013, 04, 03, 12, 15, 10);

                var item = database.GetItem(path);

                using (new ItemEditing(item, true))
                {
                    item["StringField"] = text;
                    item["DateField"] = date.ToString("yyyyMMddThhmmss");
                }

                //Act
                var result = service.GetItem<OnDemandMapping>(path);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(text, result.StringField);
                Assert.AreEqual(date, result.DateField);
            }

        }

        #endregion

        #region Map

        [Test]
        public void Map_ClassWithId_MapsFieldValues()
        {
            //Assign#

            var id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"StringField", ""},
                    {"DateField", ""}
                },
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database);

                string text = "test text 1";
                DateTime date = new DateTime(2013, 04, 03, 12, 15, 10);
                var item = database.GetItem(id.ToString());
                using (new ItemEditing(item, true))
                {
                    item["StringField"] = text;
                    item["DateField"] = date.ToString("yyyyMMddThhmmss");
                }



                var model = new MapStub();
                model.Id = id;

                //Act
                service.Map(model);

                //Assert
                Assert.AreEqual(text, model.StringField);
                Assert.AreEqual(date, model.DateField);

            }
        }



        [Test]
        public void Map_ClassWithItemUri_MapsFieldValues()
        {
            //Assign#

            var id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
                {
                    {"StringField", ""},
                    {"DateField", ""}
                },
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database);

                string text = "test text 1";
                DateTime date = new DateTime(2013, 04, 03, 12, 15, 10);
                var item = database.GetItem(id.ToString());
                using (new ItemEditing(item, true))
                {
                    item["StringField"] = text;
                    item["DateField"] = date.ToString("yyyyMMddThhmmss");
                }



                var model = new MapItemUriStub(); ;
                model.Uri = new ItemUri(new ID(id), Language.Parse("en"), database.Database);


                //Act
                service.Map(model);

                //Assert
                Assert.AreEqual(text, model.StringField);
                Assert.AreEqual(date, model.DateField);

            }
        }

        #endregion

        #region Stubs


        public class StubParameterless
        {
            public StubParameterless(string value) { }
        }
        [SitecoreType]
        public class MapStub
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreField]
            public virtual string StringField { get; set; }

            [SitecoreField]
            public virtual DateTime DateField { get; set; }

        }
        [SitecoreType]
        public class MapItemUriStub
        {
            [SitecoreInfo(SitecoreInfoType.ItemUri)]
            public virtual ItemUri Uri { get; set; }

            [SitecoreField]
            public virtual string StringField { get; set; }

            [SitecoreField]
            public virtual DateTime DateField { get; set; }

        }
        [SitecoreType]
        public class StubSaving
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(Type = SitecoreInfoType.DisplayName)]
            public virtual string Name { get; set; }
        }

        [SitecoreType]
        public interface IStubSaving
        {
            [SitecoreId]
            Guid Id { get; set; }

            [SitecoreInfo(Type = SitecoreInfoType.DisplayName)]
            string Name { get; set; }
        }

        [SitecoreType(TemplateId = TemplateId)]
        public class StubClass
        {

            public const string TemplateId = "{ABE81623-6250-46F3-914C-6926697B9A86}";

            public TimeSpan Param5 { get; set; }

            public DateTime Param3 { get; set; }
            public bool Param4 { get; set; }
            public string Param2 { get; set; }
            public int Param1 { get; set; }

            public StubClass()
            {
            }
            public StubClass(int param1)
            {
                Param1 = param1;
            }

            public StubClass(int param1, string param2)
                : this(param1)
            {
                Param2 = param2;
            }

            public StubClass(int param1, string param2, DateTime param3)
                : this(param1, param2)
            {
                Param3 = param3;
            }
            public StubClass(int param1, string param2, DateTime param3, bool param4)
                : this(param1, param2, param3)
            {
                Param4 = param4;
            }

            public StubClass(int param1, string param2, DateTime param3, bool param4, TimeSpan param5)
                : this(param1, param2, param3, param4)
            {
                Param5 = param5;
            }

            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(SitecoreInfoType.Language)]
            public virtual Language Language { get; set; }

            [SitecoreInfo(SitecoreInfoType.FullPath)]
            public virtual string Path { get; set; }

            [SitecoreInfo(SitecoreInfoType.Version)]
            public virtual int Version { get; set; }

            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }

            [SitecoreField]
            public virtual string Field { get; set; }
        }

        [SitecoreType(TemplateId = StubClass.TemplateId, AutoMap = true)]
        public class StubClassNotPreloaded
        {
            public const string TemplateId = "{ABE81623-6250-46F3-914C-6926697B9A86}";

            public virtual Guid Id { get; set; }
            public virtual string Name { get; set; }
        }

        [SitecoreType]
        public class StubClassWithProperty
        {
            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }
        }


        public class OnDemandMapping
        {
            public virtual string StringField { get; set; }
            public virtual DateTime DateField { get; set; }
        }

        [SitecoreType(TemplateId = TemplateId, AutoMap = true)]
        public class StubClassAutoMapped
        {
            public const string TemplateId = "{ABE81623-6250-46F3-914C-6926697B9A86}";


            public virtual Guid Id { get; set; }

            public virtual Language Language { get; set; }

            public virtual string Path { get; set; }

            public virtual int Version { get; set; }

            public virtual string Name { get; set; }
        }

        public class StubInterfaceAutoMappedConst
        {

            public const string TemplateId = "{7FC4F278-ADDA-4683-944C-554D0913CB33}";
        }
        [SitecoreType(TemplateId = StubInterfaceAutoMappedConst.TemplateId, AutoMap = true)]
        public interface StubInterfaceAutoMapped
        {
            Guid Id { get; set; }

            Language Language { get; set; }

            string Path { get; set; }

            int Version { get; set; }

            string Name { get; set; }

            string StringField { get; set; }
        }

        public interface IOne
        {
            string StringField { get; set; }
        }
        public interface ITwo
        {
            DateTime DateField { get; set; }
        }
        public interface IThree
        {
            string StringField { get; set; }
        }
        public interface IFour
        {
            string StringField { get; set; }
        }
        public interface IFive
        {
            DateTime DateField { get; set; }
        }
        #endregion





    }
}



