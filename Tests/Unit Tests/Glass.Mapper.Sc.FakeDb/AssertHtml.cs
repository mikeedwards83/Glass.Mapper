using System;
using System.Web;
using System.Xml;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb
{
    public static class AssertHtml
    {
        public static  void AreHtmlElementsEqual(string expected, string actual, string element)
        {
            try
            {
                XmlDocument xmlExpected = new XmlDocument();
                xmlExpected.LoadXml(expected);

                XmlDocument xmlActual = new XmlDocument();
                xmlActual.LoadXml(actual);

                var imgExpected = xmlExpected.SelectSingleNode(element);
                var imgActual = xmlActual.SelectSingleNode(element);

                if (imgExpected.Attributes.Count != imgActual.Attributes.Count)
                {
                        throw new AssertionException("Attributes count do not match");

                }
                foreach (XmlAttribute attrExpected in imgExpected.Attributes)
                {
                    var attrActual = imgActual.Attributes[attrExpected.Name];

                    if (attrActual.Value != attrExpected.Value)
                    {
                        throw new AssertionException("Attributes do not match '{0}'".Formatted(attrActual.Name));
                    }
                }


            }
            catch (AssertionException)
            {
                throw;
            }
            catch (Exception ex)
            {
               throw new AssertionException("HTML elements are different", ex);

            }

        }
        public static  void AreImgEqual(string expected, string actual)
        {
            try
            {
                XmlDocument xmlExpected = new XmlDocument();
                xmlExpected.LoadXml(expected);

                XmlDocument xmlActual = new XmlDocument();
                xmlActual.LoadXml(actual);

                var imgExpected = xmlExpected.SelectSingleNode("/img");
                var imgActual = xmlActual.SelectSingleNode("/img");

                if (imgExpected.Attributes.Count != imgActual.Attributes.Count)
                {
                        throw new AssertionException("Attributes count do not match");

                }
                foreach (XmlAttribute attrExpected in imgExpected.Attributes)
                {
                    var attrActual = imgActual.Attributes[attrExpected.Name];
                    if (attrActual.Name == "src")
                    {
                        continue;
                    }

                    if (attrActual.Value != attrExpected.Value)
                    {
                        throw new AssertionException("Attributes do not match '{0}'".Formatted(attrActual.Name));
                    }
                }

                var srcActual = imgActual.Attributes["src"];
                var srcExpected = imgExpected.Attributes["src"];

                var partsActual = srcActual.Value.Split('?');
                var partsExpected = srcExpected.Value.Split('?');

                if (partsExpected.Length != partsActual.Length)
                {
                    throw new AssertionException("Url parts do not match");
                }
                var pathActual = partsActual[0];
                var pathExpected = partsExpected[0];

                

                if (pathActual != pathExpected)
                {
                        throw new AssertionException("Paths do not match");
                }

                if (partsActual.Length == 2)
                {
                    var qsActual = HttpUtility.ParseQueryString(partsActual[1]);
                    var qsExpected = HttpUtility.ParseQueryString(partsExpected[1]);

                    if (qsActual.Count != qsExpected.Count)
                    {
                        throw new AssertionException("Query string parameter count different");
                    }
                    foreach (string key in qsActual.Keys)
                    {
                        var qsValueActual = qsActual[key];
                        var qsValueExpected = qsExpected[key];

                        if (qsValueExpected != qsValueActual)
                        {
                            throw new AssertionException("Query string parameter count different");
                        }
                    }
                }


            }
            catch (AssertionException)
            {
                throw;
            }
            catch (Exception ex)
            {
               throw new AssertionException("HTML elements are different", ex);

            }

        }
    }
}
