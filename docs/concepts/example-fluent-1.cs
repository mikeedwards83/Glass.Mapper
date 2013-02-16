            var loader = new SitecoreFluentConfigurationLoader();

            var myClass = loader.Add<MyClass>();
            myClass.Configure(x =>
                                     {
                                         x.Id(y => y.Id);
                                         x.Field(y => y.Field);
                                         x.Info(y => y.Name).InfoType(SitecoreInfoType.Name);
                                     });

