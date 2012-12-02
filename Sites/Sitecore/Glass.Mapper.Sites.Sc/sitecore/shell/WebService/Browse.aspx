<%@ Page Language="C#" %>
<%
  var username = Request.QueryString["u"] ??  string.Empty;
  var password = Request.QueryString["p"] ??  string.Empty;
  var redirect = Request.QueryString["r"] ??  string.Empty;

  Sitecore.Security.Authentication.AuthenticationManager.Login(username, password);
  
  Response.Redirect(redirect);
%>
