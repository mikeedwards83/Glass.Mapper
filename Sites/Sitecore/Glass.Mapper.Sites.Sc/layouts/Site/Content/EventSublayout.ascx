<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Content.EventSublayout" %>
<div class="page-header">
    <h1><%=Editable(x=> x.Title) %> 
        <small>
            From <%=Editable(x => x.Start, x => x.Start.ToString("dd MMM yyyy"))  %>
            to <%=Editable(x => x.End, x => x.End.ToString("dd MMM yyyy"))  %>
        </small>

    </h1>
</div>
<p class="well">
    Location: <%= Editable(x=>x.Location) %>
</p>
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
            </dl>
        </div>

<% } %>