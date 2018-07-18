using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class MediaUrlOptionsResolverFixture
    {
        #region GetMediaUrlOptions - SitecoreInfoMediaUrlOptions

        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_AbsolutePath()
        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.AlwaysIncludeServerUrl;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.IsTrue(result.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_AllowStretch()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.AllowStretch;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.IsTrue(result.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_AlwaysIncludeServerUrl()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.AlwaysIncludeServerUrl;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.IsTrue(result.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_DisableBrowserCache()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.DisableBrowserCache;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.IsTrue(result.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_DisableMediaCache()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.DisableMediaCache;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.IsTrue(result.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_IgnoreAspectRatio()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.IgnoreAspectRatio;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.IsTrue(result.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_IncludeExtension()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.RemoveExtension;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.IsFalse(result.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_LowercaseUrls()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.LowercaseUrls;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.IsTrue(result.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_Thumbnail()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.Thumbnail;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.IsTrue(result.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_UseDefaultIcon()


        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.UseDefaultIcon;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.IsTrue(result.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreInfoMediaUrlOptions_UseItemPath()
        {
            //Arrange
            var option = SitecoreInfoMediaUrlOptions.UseItemPath;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.IsTrue(result.UseItemPath, "UseItemPath");

        }

        #endregion

        #region GetMediaUrlOptions - SitecoreMediaUrlOptions

        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_AbsolutePath()
        {
            //Arrange
            var option = SitecoreMediaUrlOptions.AlwaysIncludeServerUrl;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.IsTrue(result.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_AllowStretch()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.AllowStretch;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.IsTrue(result.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_AlwaysIncludeServerUrl()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.AlwaysIncludeServerUrl;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.IsTrue(result.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_DisableBrowserCache()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.DisableBrowserCache;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.IsTrue(result.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_DisableMediaCache()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.DisableMediaCache;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.IsTrue(result.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_IgnoreAspectRatio()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.IgnoreAspectRatio;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.IsTrue(result.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_IncludeExtension()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.RemoveExtension;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.IsFalse(result.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_LowercaseUrls()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.LowercaseUrls;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.IsTrue(result.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_Thumbnail()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.Thumbnail;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.IsTrue(result.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_UseDefaultIcon()


        {
            //Arrange
            var option = SitecoreMediaUrlOptions.UseDefaultIcon;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.IsTrue(result.UseDefaultIcon, "UseDefaultIcon");
            Assert.AreEqual(result.UseItemPath, MediaUrlOptions.Empty.UseItemPath, "UseItemPath");

        }
        [Test]
        public void GetMediaUrlOptions_SitecoreMediaUrlOptions_UseItemPath()
        {
            //Arrange
            var option = SitecoreMediaUrlOptions.UseItemPath;
            var urlOptionsResolver = new MediaUrlOptionsResolver();

            //Act
            var result = urlOptionsResolver.GetMediaUrlOptions(option);

            //Assert
            Assert.AreEqual(result.AbsolutePath, MediaUrlOptions.Empty.AbsolutePath, "AbsolutePath");
            Assert.AreEqual(result.AllowStretch, MediaUrlOptions.Empty.AllowStretch, "AllowStretch");
            Assert.AreEqual(result.AlwaysIncludeServerUrl, MediaUrlOptions.Empty.AlwaysIncludeServerUrl, "AlwaysIncludeServerUrl");
            Assert.AreEqual(result.DisableBrowserCache, MediaUrlOptions.Empty.DisableBrowserCache, "DisableBrowserCache");
            Assert.AreEqual(result.DisableMediaCache, MediaUrlOptions.Empty.DisableMediaCache, "DisableMediaCache");
            Assert.AreEqual(result.IgnoreAspectRatio, MediaUrlOptions.Empty.IgnoreAspectRatio, "IgnoreAspectRatio");
            Assert.AreEqual(result.IncludeExtension, MediaUrlOptions.Empty.IncludeExtension, "IncludeExtension");
            Assert.AreEqual(result.LowercaseUrls, MediaUrlOptions.Empty.LowercaseUrls, "LowercaseUrls");
            Assert.AreEqual(result.Thumbnail, MediaUrlOptions.Empty.Thumbnail, "Thumbnail");
            Assert.AreEqual(result.UseDefaultIcon, MediaUrlOptions.Empty.UseDefaultIcon, "UseDefaultIcon");
            Assert.IsTrue(result.UseItemPath, "UseItemPath");

        }

        #endregion
    }
}
