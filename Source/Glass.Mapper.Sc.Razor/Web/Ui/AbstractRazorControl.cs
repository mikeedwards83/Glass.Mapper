/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Profilers;
using Glass.Mapper.Sc.Profilers;
using RazorEngine.Templating;
using Sitecore.Web.UI;
using System.Web.UI;
using Sitecore.Data.Items;
using System.Web.Mvc;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class AbstractRazorControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractRazorControl<T> : WebControl, IRazorControl, Sitecore.Layouts.IExpandable
    {

        
        IPerformanceProfiler _profiler = new SitecoreProfiler();
        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>
        /// The profiler.
        /// </value>
        public IPerformanceProfiler Profiler
        {
            get{
                return _profiler;
            }
            set
            {
                _profiler = value;
            }
        }

        private ISitecoreContext _sitecoreContext;

        /// <summary>
        /// Gets the view manager.
        /// </summary>
        /// <value>
        /// The view manager.
        /// </value>
        public ViewManager ViewManager { get; private set; }

        ///// <summary>
        ///// A list of placeholders to render on the page.
        ///// </summary>
        ///// <value>The placeholders.</value>
        //public IEnumerable<string> Placeholders
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// View data
        /// </summary>
        /// <value>The view data.</value>
        public ViewDataDictionary ViewData { get; private set; }

        /// <summary>
        /// The path to the Razor view
        /// </summary>
        /// <value>The view.</value>
        public CachedView View
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the Glass Context to use
        /// </summary>
        /// <value>The name of the context.</value>
        public string ContextName
        {
            get;
            set;
        }

        /// <summary>
        /// The model to pass to the Razor view.
        /// </summary>
        /// <value>The model.</value>
        public T Model
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sitecore service.
        /// </summary>
        /// <value>The sitecore service.</value>
        public ISitecoreContext SitecoreContext
        {
            get
            {
                
                if (_sitecoreContext == null)
                {
                    if (ContextName.IsNotNullOrEmpty())
                    {
                        _sitecoreContext = new SitecoreContext(ContextName)
                            {
                                Profiler = Profiler
                            };
                    }
                    else
                    {
                        _sitecoreContext = new SitecoreContext
                            {
                            Profiler = Profiler
                        };
                    }
                }
                return _sitecoreContext;
            }
        }

        /// <summary>
        /// Gives access to the GlassHtml helper class
        /// </summary>
        public IGlassHtml GlassHtml { get;  set; }

       
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRazorControl{T}"/> class.
        /// </summary>
        public AbstractRazorControl()
        {
            ViewData = new ViewDataDictionary();
            ViewManager = new ViewManager();
            GlassHtml = new GlassHtml(new SitecoreContext());
        }

        /// <summary>
        /// Put your logic to create your model here
        /// </summary>
        /// <returns>`0.</returns>
        public abstract T GetModel();

        /// <summary>
        /// Returns either the data source item or if no data source is specified the context item
        /// </summary>
        /// <returns>Item.</returns>
        protected Item GetDataSourceOrContextItem()
        {
            return DataSource.IsNullOrEmpty() ? Sitecore.Context.Item :
                Sitecore.Context.Database.GetItem(DataSource);
        }

        /// <summary>
        /// Get caching identifier. Must be implemented by controls that supports caching.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <remarks>If an empty string is returned, the control will not be cached.</remarks>
        protected override string GetCachingID()
        {
            return View.Name;
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        /// <exception cref="Glass.Mapper.Sc.Razor.RazorException"></exception>
        /// <remarks>When developing custom server controls, you can override this method to generate content for an ASP.NET page.</remarks>
        protected override void DoRender(HtmlTextWriter output)
        {
            try
            {
             
                    Profiler.Start("Razor engine {0}".Formatted(View.Name));

                    Profiler.Start("Get Model");

                    Model = GetModel();

                    Profiler.End("Get Model");


                    Profiler.Start("Create Template");

                    var template =
                        RazorEngine.Razor.GetTemplate(View.ViewContent, Model, View.Name) as ITemplateBase;

                    Profiler.End("Create Template");

                    Profiler.Start("Configure Template");

                    template.Configure(SitecoreContext, ViewData, this);

                    Profiler.End("Configure Template");

                    Profiler.Start("Run Template");

                    output.Write(((RazorEngine.Templating.ITemplate)template).Run(new ExecuteContext()));

                    Profiler.End("Run Template");
            }
            catch (TemplateCompilationException ex)
            {
                var errors = new StringBuilder();
                ex.Errors.ForEach(x =>
                                      {
                                          errors.AppendLine("File: {0}".Formatted(View));
                                          errors.AppendLine(x.ErrorText);
                                      });


                //   throw new RazorException(errors.ToString());

                WriteException(output, ex);
            }
            catch (Exception ex)
            {
                WriteException(output, ex);
            }
            finally
            {
                Profiler.End("Razor engine {0}".Formatted(View));
            }
        }

        private void WriteException(HtmlTextWriter output, Exception ex)
        {

            

            output.Write("<h1>Glass Razor Rendering Exception</h1>");
            output.Write("<p>View: {0}</p>".Formatted(View.Name));
            
            Exception subEx = ex;
            while (subEx != null)
            {
                output.Write("<p>{0}</p>".Formatted(subEx.Message));
                output.Write("<pre>{0}</pre>".Formatted(subEx.StackTrace));

                subEx = subEx.InnerException;
            }

            Sitecore.Diagnostics.Log.Error("Glass Razor Rendering Error {0}".Formatted(View), ex, this);

        }

        /// <summary>
        /// Expands this instance.
        /// </summary>
        public void Expand()
        {
            if (View.Placeholders != null)
            {
                foreach (var placeHolderName in View.Placeholders)
                {
                    var holder = new Sitecore.Web.UI.WebControls.Placeholder();
                    holder.Key = placeHolderName.ToLower();
                    Controls.Add(holder);
                }
            }

            Controls.Cast<Control>().Where(x => x is Sitecore.Layouts.IExpandable)
                .Cast<Sitecore.Layouts.IExpandable>().ToList().ForEach(x => x.Expand());
        }

        /// <summary>
        /// Converts rendering parameters to a concrete type. Use this method if you have defined the template ID on the 
        /// model configuration.
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <returns></returns>
        public virtual TK GetRenderingParameters<TK>() where TK : class
        {
            return GlassHtml.GetRenderingParameters<TK>(Parameters);
        }

    }
   
}

