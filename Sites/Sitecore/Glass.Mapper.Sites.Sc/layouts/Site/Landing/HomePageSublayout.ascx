<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomePageSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Landing.HomePageSublayout" %>
<div class="hero-unit">
    <h1><%=Model.Title %></h1>
    <%=Model.MainBody %>
</div>

<div class="span6">
    <h3>Featured News</h3>
    <ul class="thumbnails">
        <% foreach (var news in Model.News)
           { %>
        <li class="span3">
            <div class="thumbnail">
                <img src="<%=news.FeaturedImage.Src  %>" alt="<%=news.FeaturedImage.Alt %>">
                <h4>
                    <a href="<%=news.Url %>">
                        <%=news.Title %>
                    </a>
                </h4>
                <p><%=news.Abstract %></p>
            </div>
        </li>

        <% } %>
    </ul>
</div>
