<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tutorial2Sublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.Tutorial2.Layouts.Tutorial2Sublayout" %>
<h1><%=Model.Title %></h1>

<dl>
    <dt>Item Name:</dt>
    <dd><%=Model.Name %></dd>
    <dt>Item Path:</dt>
    <dd><%=Model.Path %></dd>
    <dt>Template Name:</dt>
    <dd><%=Model.TemplateName %></dd>
    <dt>Created Date</dt>
    <dd><%=Model.Created.ToString("hh:mm:ss dd-MM-yyyy") %></dd>
    <dt>Main Content</dt>
    <dd><%=Model.MainContent %></dd>
    <dt>Image</dt>
    <dd>
        <img src="<%=Model.Image.Src %>" alt="<%=Model.Image.Alt %>"/>
    </dd>
</dl>