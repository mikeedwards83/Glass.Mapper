<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Metadata.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Misc.Metadata" %>

<title><%=Model.Title %></title>

<meta runatname="updated" content="<%=Model.__updated %>" />

<link rel="canonical" href="http://sitecore.glass.com<%=Model.Url %>" />

<link rel="prev" href="http://sitecore.glass.com<%=Model.Parent.Url %>" />

<% foreach (var child in Model.Children)
   { %>
<link rel="next" href="http://sitecore.glass.com<%=child.Url %>"/>
<% } %>


