<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsLandingSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Landing.NewsLandingSublayout" %>
<div class="page-header">
    <h1><%=Editable(x=> x.Title) %> <small></small></h1>
</div>
<ul class="thumbnails">
    <% foreach (var article in Model.Articles.OrderBy(x=>x.Date))
       { %>
    <li>
        <div class="span1"></div>
        <div class="span3">
            <a title="<%=article.Title %>" class="thumbnail">
                <img src="<%=article.FeaturedImage.Src%>"  />
            </a>
        </div>
        <div class="span7">
            <div class="row">
                <div class="span5">
                    <h3><a href="<%=article.Url %>"><%=article.Title %></a></h3>
                </div>
                <div class="span2">
                    <p><%=article.Date.ToString("dd MMM yyyy") %></p>
                </div>
            </div>
            <div class="row">
                <p  class="span7">
                    <%=article.Abstract %>
                </p>
            </div>
            <div class="row">
                <div class="span7">
                    <a title="<%=article.Title %>" class="pull-right" href="<%=article.Url %>">Read More</a>
                </div>
            </div>

        </div>
        <div class="span1"></div>
    </li>
    <% } %>
</ul>
