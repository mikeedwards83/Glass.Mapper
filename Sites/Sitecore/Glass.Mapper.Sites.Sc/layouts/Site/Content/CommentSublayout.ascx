<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentSublayout.ascx.cs" Inherits="Glass.Mapper.Sites.Sc.layouts.Site.Content.CommentSublayout" %>
<form>
    <fieldset>
        <asp:Label runat="server" AssociatedControlID="CommentName">Name:</asp:Label>
        <asp:TextBox runat="server" ID="CommentName" CssClass="input-xlarge"></asp:TextBox>
        <asp:Label ID="Label1" runat="server" AssociatedControlID="CommentEmail">Email:</asp:Label>
        <asp:TextBox runat="server" ID="CommentEmail" CssClass="input-xlarge"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" AssociatedControlID="CommentContent">Comment:</asp:Label>
        <asp:TextBox runat="server" ID="CommentContent" TextMode="MultiLine" CssClass="input-xlarge"></asp:TextBox>
        <div>
            <asp:Button runat="server" ID="CommentSubmit" Text="Submit" CssClass="btn" />
        </div>
    </fieldset>
</form>
<div class="alert alert-success" runat="server" id="CommentThankYou" Visible="false">
    <strong>Thank You for commenting!</strong> Please wait while we approve you comment.
</div>

<% if (Model.Children.Any())
   { %>
<h2>Comments</h2>
<ul>
    <% foreach (var comment in Model.Children)
       { %>
    <li>
        <small><%=comment.Date.ToString("dd MMM yy") %></small> <strong><%=comment.FullName %></strong> 
        <p>
            <%=comment.Content %>
        </p>
    </li>
    <% } %>
</ul>
<% } %>
