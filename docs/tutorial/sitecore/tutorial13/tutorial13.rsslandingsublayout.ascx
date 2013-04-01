<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssLandingSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Landing.RssLandingSublayout" %>
<h2><%=Model.Title %></h2>

<ul class="thumbnails">
    <% foreach (var article in Model.GetArticles())
       { %>
    <li class="span4">
    <div class="thumbnail">
      <h3><a href="<%=article.Links.First().Uri.ToString() %>"><%=article.Title.Text %></a></h3>
      <p><%=article.Summary.Text %></p>
    </div>
  </li>
    <% } %>
</ul>