<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventsLandingSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Landing.EventsLandingSublayout" %>
<div class="page-header">
    <h1><%=Editable(x=> x.Title) %> <small></small></h1>
</div>
<ul class="thumbnails">
    <% foreach (var evnt in Model.Events.OrderBy(x=>x.Start))
       { %>
    <li>
        <div class="span1"></div>
        <div class="span7">
            <div class="row">
                <div class="span5">
                    <h3><a href="<%=evnt.Url %>"><%=evnt.Title %></a></h3>
                </div>
                <div class="span2">
                    <p>Start: <%=evnt.Start.ToString("dd MMM yyyy") %></p>
                    <p>End: <%=evnt.End.ToString("dd MMM yyyy") %></p>
                </div>
            </div>
            <div class="row">
                <p  class="span7">
                    <%=evnt.Abstract %>
                </p>
            </div>
            <div class="row">
                <div class="span7">
                    <a title="<%=evnt.Title %>" class="pull-right" href="<%=evnt.Url %>">Find Out More</a>
                </div>
            </div>

        </div>
        <div class="span1"></div>
    </li>
    <% } %>
</ul>
