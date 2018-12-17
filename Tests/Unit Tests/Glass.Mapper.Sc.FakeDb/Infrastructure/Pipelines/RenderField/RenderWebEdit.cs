using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Pipelines.RenderField;
using Sitecore.Shell;
using Sitecore.Sites;
using Sitecore.StringExtensions;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.PageModes;

namespace Glass.Mapper.Sc.FakeDb.Infrastructure.Pipelines.RenderField
{
    /// <summary>
    /// This is a copy of Sitecore.Pipelines.RenderField.RenderWebEditing but the CanWebEdit has been modified to make it work outside of a HTTPContext
    /// </summary>
    public class RenderWebEditing
    {
        /// <summary>Gets the field value.</summary>
        /// <param name="args">The arguments.</param>
        /// <contract>
        ///   <requires name="args" condition="none" />
        /// </contract>
        public void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            if (!this.CanWebEdit(args) && !args.WebEditParameters.ContainsKey("sc-highlight-contentchange") || (args.Item == null || !this.CanEditItem(args.Item)))
                return;
            Field field = args.Item.Fields[args.FieldName];
            if (field == null || !this.CanEditField(field))
                return;
            Item obj = field.Item;
            string str = obj[FieldIDs.Revision].Replace("-", string.Empty);
            string controlID = "fld_" + (object)obj.ID.ToShortID() + "_" + (object)field.ID.ToShortID() + "_" + (object)obj.Language + "_" + (object)obj.Version + "_" + str + "_" + (object)MainUtil.GetSequencer();
            HtmlTextWriter output = new HtmlTextWriter((TextWriter)new StringWriter());
            string rawValueContainer = this.GetRawValueContainer(field, controlID);
            output.Write(rawValueContainer);
            if (args.DisableWebEditContentEditing && args.DisableWebEditFieldWrapping)
                this.RenderWrapperlessField(output, args, field, controlID);
            else
                this.RenderWrappedField(output, args, field, controlID);
        }

        /// <summary>
        /// Determines whether this instance [can edit field] the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can edit field] the specified field; otherwise, <c>false</c>.
        /// </returns>
        private bool CanEditField(Field field)
        {
            Assert.ArgumentNotNull((object)field, "field");
            return field.CanWrite;
        }

        /// <summary>
        /// Determines whether this instance [can edit item] the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can edit item] the specified item; otherwise, <c>false</c>.
        /// </returns>
        private bool CanEditItem(Item item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            return (Sitecore.Context.IsAdministrator || !item.Locking.IsLocked() || item.Locking.HasLock()) && (item.Access.CanWrite() && item.Access.CanWriteLanguage() && !item.Appearance.ReadOnly);
        }

        /// <summary>
        /// Determines whether this instance [can web edit] the specified args.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private bool CanWebEdit(RenderFieldArgs args)
        {
            if (EnableWebEditMode.AllowWebEdit)
            {
                return true;
            }
            if (args.DisableWebEdit)
                return false;
            SiteContext site = Sitecore.Context.Site;

            return site != null 
                && site.DisplayMode == DisplayMode.Edit && (!(WebUtil.GetQueryString("sc_duration") == "temporary")
#if SC82 || SC90 || SC91
                && Sitecore.Context.PageMode.IsExperienceEditorEditing);
#else
                && Sitecore.Context.PageMode.IsPageEditorEditing);
#endif

        }

        /// <summary>Renders the field without a wrapper.</summary>
        /// <param name="output">The output.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="field"> </param>
        /// <param name="controlID"> </param>
        private void RenderWrapperlessField(HtmlTextWriter output, RenderFieldArgs args, Field field, string controlID)
        {
            Assert.ArgumentNotNull((object)output, "output");
            Assert.ArgumentNotNull((object)args, "args");
            Assert.ArgumentNotNull((object)controlID, "controlID");
            Tag fieldTag = CreateFieldTag("code", args, controlID);
            fieldTag.Class = "scpm";
            fieldTag.Add("kind", "open").Add("type", "text/sitecore").Add("chromeType", "field");
            string str = args.Result.FirstPart;
            if (string.IsNullOrEmpty(str))
            {
                fieldTag.Add("scWatermark", "true");
                string defaultText = GetDefaultText(args);
                str = defaultText;
                if (StringUtil.RemoveTags(defaultText) == defaultText)
                    str = "<span class='scTextWrapper'>" + defaultText + "</span>";
            }
            this.AddParameters(fieldTag, args);
            string fieldData =GetFieldData(args, field, controlID);
            fieldTag.InnerHtml = fieldData;
            output.Write(fieldTag.ToString());
            output.Write(str);
            args.Result.FirstPart = output.InnerWriter.ToString();
            Tag tag = new Tag("code")
            {
                Class = "scpm"
            };
            tag.Add("kind", "close").Add("type", "text/sitecore").Add("chromeType", "field");
            args.Result.LastPart += tag.ToString();
        }

        /// <summary>Renders the inline editable field.</summary>
        /// <param name="output">The output.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="field">The field.</param>
        /// <param name="controlID">The control Id.</param>
        private void RenderWrappedField(HtmlTextWriter output, RenderFieldArgs args, Field field, string controlID)
        {
            Assert.ArgumentNotNull((object)output, "output");
            Assert.ArgumentNotNull((object)args, "args");
            Assert.ArgumentNotNull((object)controlID, "controlID");
            string fieldData = GetFieldData(args, field, controlID);
            output.Write("<span class=\"scChromeData\">{0}</span>", (object)fieldData);
            Tag fieldTag =CreateFieldTag(this.GetEditableElementTagName(args), args, controlID);
            fieldTag.Class = "scWebEditInput";
            if (!args.DisableWebEditContentEditing)
                fieldTag.Add("contenteditable", "true");
            string str = args.Result.FirstPart;
            if (string.IsNullOrEmpty(str))
            {
                fieldTag.Add("scWatermark", "true");
                str =GetDefaultText(args);
            }
            this.AddParameters(fieldTag, args);
            if (args.FieldTypeKey.ToLowerInvariant() == "word document" && args.Parameters["editormode"] == "inline")
                ApplyWordFieldStyle(fieldTag, args);
            output.Write(fieldTag.Start());
            output.Write(str);
            args.Result.FirstPart = output.InnerWriter.ToString();
            args.Result.LastPart += fieldTag.End();
        }

        /// <summary>Adds the parameters.</summary>
        /// <param name="tag">The tag.</param>
        /// <param name="args">The arguments.</param>
        private void AddParameters(Tag tag, RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)tag, "tag");
            Assert.ArgumentNotNull((object)args, "args");
            if (args.WebEditParameters.Count <= 0)
                return;
            UrlString urlString = new UrlString();
            foreach (KeyValuePair<string, string> webEditParameter in (SafeDictionary<string, string>)args.WebEditParameters)
                urlString.Add(webEditParameter.Key, webEditParameter.Value);
            tag.Add("sc_parameters", urlString.ToString());
        }

        /// <summary>Gets the name of the editable element tag.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private string GetEditableElementTagName(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            string str = "span";
            if ((UIUtil.IsFirefox() || UIUtil.IsWebkit()) && (UIUtil.SupportsInlineEditing() && MainUtil.GetBool(args.Parameters["block-content"], false)))
                str = "div";
            return str;
        }

        /// <summary>Gets the field value HTML.</summary>
        /// <param name="field">The field.</param>
        /// <param name="controlID">The control ID.</param>
        /// <returns>The field value HTML.</returns>
        private string GetRawValueContainer(Field field, string controlID)
        {
            Assert.ArgumentNotNull((object)field, "field");
            Assert.ArgumentNotNull((object)controlID, "controlID");
            return "<input id='{0}' class='scFieldValue' name='{0}' type='hidden' value=\"{1}\" />".FormatWith((object)controlID, (object)HttpUtility.HtmlEncode(field.Value));
        }

        /// <summary>Gets the default image.</summary>
        /// <param name="args">The args.</param>
        /// <returns>The default image.</returns>
        private static string GetDefaultText(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            string @string = StringUtil.GetString(new string[2]
            {
        args.RenderParameters["default-text"],
        string.Empty
            });
            using (new LanguageSwitcher(WebUtil.GetCookieValue("shell", "lang", Sitecore.Context.Language.Name)))
            {
                if (@string.IsNullOrEmpty())
                {
                    Database database = Factory.GetDatabase("core");
                    Assert.IsNotNull((object)database, "core");
                    Item obj = database.GetItem("/sitecore/content/Applications/WebEdit/WebEdit Texts");
                    Assert.IsNotNull((object)obj, "/sitecore/content/Applications/WebEdit/WebEdit Texts");
                    @string = obj["Default Text"];
                }
                if (string.Compare(args.RenderParameters["show-title-when-blank"], "true", StringComparison.InvariantCultureIgnoreCase) == 0)
                    @string = GetFieldDisplayName(args) + ": " + @string;
            }
            return @string;
        }

        /// <summary>Gets the display name of the field.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The get field display name.</returns>
        private static string GetFieldDisplayName(RenderFieldArgs args)
        {
            Assert.IsNotNull((object)args, "args");
            Assert.IsNotNull((object)args.Item, "item");
            Item obj;
            if (string.Compare(WebUtil.GetCookieValue("shell", "lang", Sitecore.Context.Language.Name), args.Item.Language.Name, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                obj = args.Item.Database.GetItem(args.Item.ID);
                Assert.IsNotNull((object)obj, "Item");
            }
            else
                obj = args.Item;
            Field field = obj.Fields[args.FieldName];
            if (field != null)
                return field.DisplayName;
            return args.FieldName;
        }

        /// <summary>Gets the word style string.</summary>
        /// <param name="tag">The tag.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The word style string.</returns>
        private static void ApplyWordFieldStyle(Tag tag, RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)tag, "tag");
            Assert.ArgumentNotNull((object)args, "args");
            string str1 = args.Parameters["editorwidth"] ?? Settings.WordOCX.Width;
            string str2 = args.Parameters["editorheight"] ?? Settings.WordOCX.Height;
            string str3 = args.Parameters["editorpadding"] ?? Settings.WordOCX.Padding;
            string str4 = str1.ToLowerInvariant().Replace("px", string.Empty);
            int int1 = MainUtil.GetInt(str4, -1);
            string str5 = str2.ToLowerInvariant().Replace("px", string.Empty);
            int int2 = MainUtil.GetInt(str5, -1);
            int int3 = MainUtil.GetInt(str3.ToLowerInvariant().Replace("px", string.Empty), -1);
            if (int3 >= 0)
            {
                if (int1 >= 0)
                    str4 = (int1 + 2 * int3).ToString() + string.Empty;
                if (int2 >= 0)
                    str5 = (int2 + 2 * int3).ToString() + string.Empty;
            }
            tag.Class += " scWordContainer";
            tag.Style = "width:{0}px;height:{1}px;padding:{2};".FormatWith((object)str4, (object)str5, (object)str3);
        }

        /// <summary>Renders the buttons.</summary>
        /// <param name="commands">The commands.</param>
        /// <param name="field">The field.</param>
        /// <param name="controlID"> </param>
        private static void SetCommandParametersValue(IEnumerable<WebEditButton> commands, Field field, string controlID)
        {
            Assert.ArgumentNotNull((object)commands, "commands");
            Assert.ArgumentNotNull((object)field, "field");
            Assert.ArgumentNotNull((object)controlID, "controlID");
            Item obj = field.Item;
            string newValue;
            if (UserOptions.WebEdit.UsePopupContentEditor)
            {
                newValue = "javascript:Sitecore.WebEdit.postRequest(\"webedit:edit(id=" + (object)obj.ID + ",language=" + (object)obj.Language + ",version=" + (object)obj.Version + ")\")";
            }
            else
            {
                UrlString urlString = new UrlString(WebUtil.GetRawUrl());
                urlString["sc_ce"] = "1";
                urlString["sc_ce_uri"] = HttpUtility.UrlEncode(obj.Uri.ToString());
                newValue = urlString.ToString();
            }
            foreach (WebEditButton command in commands)
            {
                if (!string.IsNullOrEmpty(command.Click))
                {
                    string str = command.Click.Replace("$URL", newValue).Replace("$ItemID", obj.ID.ToString()).Replace("$Language", obj.Language.ToString()).Replace("$Version", obj.Version.ToString()).Replace("$FieldID", field.ID.ToString()).Replace("$ControlID", controlID).Replace("$MessageParameters", "itemid=" + (object)obj.ID + ",language=" + (object)obj.Language + ",version=" + (object)obj.Version + ",fieldid=" + (object)field.ID + ",controlid=" + controlID).Replace("$JavascriptParameters", "\"" + (object)obj.ID + "\",\"" + (object)obj.Language + "\",\"" + (object)obj.Version + "\",\"" + (object)field.ID + "\",\"" + controlID + "\"");
                    command.Click = str;
                }
            }
        }

        /// <summary>Renders the bottom bar.</summary>
        /// <param name="args">The arguments.</param>
        /// <param name="field">The field.</param>
        /// <param name="controlID"> </param>
        private static string GetFieldData(RenderFieldArgs args, Field field, string controlID)
        {
            Assert.ArgumentNotNull((object)args, "args");
            Assert.ArgumentNotNull((object)field, "field");
            Assert.ArgumentNotNull((object)controlID, "controlID");
            Item obj = field.Item;
            Assert.IsNotNull((object)Sitecore.Context.Site, "site");
            using (new LanguageSwitcher(WebUtil.GetCookieValue("shell", "lang", Sitecore.Context.Site.Language)))
            {
                GetChromeDataArgs args1 = new GetChromeDataArgs("field", obj, args.Parameters);
                args1.CustomData["field"] = (object)field;
                GetChromeDataPipeline.Run(args1);
                ChromeData chromeData = args1.ChromeData;
                SetCommandParametersValue((IEnumerable<WebEditButton>)chromeData.Commands, field, controlID);
                return chromeData.ToJson();
            }
        }

        /// <summary>Creates the field tag.</summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="controlID"> </param>
        /// <returns>The field tag.</returns>
        private static Tag CreateFieldTag(string tagName, RenderFieldArgs args, string controlID)
        {
            Assert.ArgumentNotNull((object)tagName, "tagName");
            Assert.ArgumentNotNull((object)args, "args");
            Tag tag = new Tag(tagName)
            {
                ID = controlID + "_edit"
            };
            tag.Add("scFieldType", args.FieldTypeKey);
            return tag;
        }
    }
}
