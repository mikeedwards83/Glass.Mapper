﻿using System;
using System.Text.RegularExpressions;
using Sitecore;
#if SC93 || SC100 || SC101 || SC102 || SC103 || SC104
using Sitecore.Abstractions;
#endif
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Pipelines.GetChromeData
{
    public class EditFrameBuilder : GetChromeDataProcessor
    {
        private const string WebEditButtonRoot = "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/";
        public const string BuildToken = "glassBuild:";

        private readonly TemplateID _folderTemplateId = new TemplateID(new ID("{B0666CFE-8C7B-4CC1-8E32-8253742EBFA9}"));
        private readonly TemplateID _editTemplateId = new TemplateID(new ID("{1AB4F9AD-B004-413C-8924-3E07143A614B}"));

        /// <summary>
        /// Prefixes for items created in the core database when using dynamic edit frames. Default value is empty.
        /// </summary>
        public static string EditFrameItemPrefix = "";
        public static string EditName = "Edit Fields";
        private static string HeaderField = "Header";
        private static string FieldsField = "Fields";
        public static string DefaultTitle = "Edit Fields";
        private static string DefaultTitleField = "Default Title";
        private static string DefaultTooltipField = "Default Tooltip";
        private static string TooltipField = "Tooltip";
        private static string IconField = "Icon";
        public static string Tooltip = "Edit the following fields: {0}";
        public static string Icon = "people/16x16/cubes_blue.png";

        private readonly Regex _titleRegex = new Regex("<title>(?<title>([^<]*))<title>");

#if SC93 || SC100 || SC101 || SC102 || SC103 || SC104
        public EditFrameBuilder(BaseClient client) : base(client)
        {

        }
#endif

        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            Assert.IsNotNull((object)args.ChromeData, "Chrome Data");
            if (!"editFrame".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
                return;

            string str = StringUtil.GetString(args.CustomData["buttonsPath"], Settings.WebEdit.DefaultButtonPath);
            Assert.IsNotNull((object)str, "path");
            if (str.StartsWith(BuildToken, StringComparison.InvariantCulture))
            {


                string defaultTitle = DefaultTitle;

                if (str.Contains("<title>"))
                {
                    var match = _titleRegex.Match(str);
                    var title = match.Groups["title"].Value;
                    defaultTitle = title;
                    str = _titleRegex.Replace(str, "");
                }

                var fields = str.Substring(BuildToken.Length);

                var name = ItemUtil.ProposeValidItemName(fields);

                //me this is hack for really long names
                if (name.Length > 40)
                {
                    var hash = name.GetHashCode();
                    if (hash < 0)
                    {
                        hash *= -1;
                    }
                    name = hash.ToString();
                }

                name = EditFrameItemPrefix + name;
                str = "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/" + name;
                Database database = Factory.GetDatabase("core");
                Assert.IsNotNull((object)database, "core");
                using (new SecurityDisabler())
                {
                    Item folderItem = database.GetItem(str);

                    if (folderItem == null)
                    {


                        var rootItem = database.GetItem(WebEditButtonRoot);

                        folderItem = rootItem.Add(name, _folderTemplateId);

                        string toolTipText = Tooltip.Formatted(fields.Replace("|", ", "));

                        folderItem.Editing.BeginEdit();
                        folderItem[DefaultTooltipField] = toolTipText;
                        folderItem[DefaultTitleField] = defaultTitle;
                        folderItem.Editing.EndEdit();



                        var editItem = folderItem.Add(EditName, _editTemplateId);

                        editItem.Editing.BeginEdit();

                        editItem[HeaderField] = EditName;
                        editItem[FieldsField] = fields;
                        editItem[IconField] = Icon;
                        editItem[TooltipField] = toolTipText;

                        editItem.Editing.EndEdit();

                    }

                    args.CustomData["buttonsPath"] = folderItem.Paths.Path;

                }


            }


        }



    }

}
