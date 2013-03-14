using Sitecore.Web.UI.Sheer;
using Sitecore.Text;
using Sitecore;

namespace Glass.Mapper.Sc.Razor.Shell.Commands
{
    /// <summary>
    /// Class CreateDynamicRazor
    /// </summary>
    public class CreateDynamicRazor : global::Sitecore.Shell.Framework.Commands.Command
    {
        /// <summary>
        /// Executes the command in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(global::Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            if (context.Items.Length == 1)
            {
                global::Sitecore.Data.Items.Item item = context.Items[0];

                ClientPipelineArgs args = new ClientPipelineArgs();

                System.Collections.Specialized.NameValueCollection parameters =
                new System.Collections.Specialized.NameValueCollection();
                parameters["id"] = item.ID.ToString();
                parameters["language"] = item.Language.ToString();
                parameters["database"] = item.Database.Name;

                args.Parameters = parameters;

                global::Sitecore.Context.ClientPage.Start(this, "Run", args);
            }
        }

        /// <summary>
        /// Runs the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        protected void Run(ClientPipelineArgs args)
        {
            UrlString str = new UrlString(UIUtil.GetUri("control:GlassDynamicRazor", "id={AE723732-6D09-4DBA-B553-A1B399EB077D}&locationId=" + args.Parameters["id"]));

            if (!args.IsPostBack)
            {
                SheerResponse.ShowModalDialog(str.ToString(), true);
                args.WaitForPostBack();
                return;
            }
        }
    }
}
