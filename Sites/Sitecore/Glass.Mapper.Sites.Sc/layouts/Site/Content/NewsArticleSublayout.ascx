<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsArticleSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Content.NewsArticleSublayout" %>
<div class="page-header">
    <h1><%=Editable(x=> x.Title) %> <small>Date: <%=Editable(x => x.Date, x => x.Date.ToString("dd MMM yyyy"))  %></small></h1>
</div>
<%=Editable(x=> x.MainBody )%>

<% if (IsInEditingMode)
   { %>
        <div class="well">
            <h3>Additional fields</h3>
            <dl>
                <dt>Abstract</dt>
                <dd>
                    <%=Editable(x=>x.Abstract) %>
                </dd>
                <dt>Featured Image</dt>
                <dd>
                    <%=Editable(x=>x.FeaturedImage) %>
                </dd>
            </dl>

           
        </div>

<% } %>