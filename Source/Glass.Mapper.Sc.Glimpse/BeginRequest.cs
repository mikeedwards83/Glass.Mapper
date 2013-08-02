using Sitecore.Pipelines;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Glass.Mapper.Sc.Glimpse
{
    public class BeginRequest
    {
        public void Process(HttpRequestArgs args)
        {
            var page = Sitecore.Context.Page.Page;

            //page.PreRender += page_PreRender;

        }

        void page_PreRender(object sender, EventArgs e)
        {
            string name = "prerender";
            PipelineStart.Profiler.Start(name);
            PipelineStart.Profiler.End(name);
        }
    }

    public class GlimpsePage : Page
    {
        public GlimpsePage()
        {

            this.InitComplete += GlimpsePage_InitComplete;

        }

        void GlimpsePage_InitComplete(object sender, EventArgs e)
        {
            ChildBind(this);
           
        }
        
        private void ChildBind(Control control)
        {
            control.PreRender += control_PreRender;
            control.Load += control_Load;
            
            foreach (Control child in control.Controls)
            {
                ChildBind(child);
            }
        }

        void control_Load(object sender, EventArgs e)
        {
            Record("Load ", sender);

        }

        void control_PreRender(object sender, EventArgs e)
        {
            Record("PreRender", sender);
        }

        private void Record(string evtname, object sender)
        {
            StringBuilder sb = new StringBuilder(evtname+" ");

            bool match = false;

            if (sender is Sublayout)
            {
                var sub = sender as Sublayout;
                sb.Append(sub.Path);
                match = true;
            }

            if (match)
            {
                PipelineStart.Profiler.Start(sb.ToString());
                PipelineStart.Profiler.End(sb.ToString());
            }
        }
    
    }
}
