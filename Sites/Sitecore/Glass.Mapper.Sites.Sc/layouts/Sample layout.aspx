<%@ Import Namespace="Sitecore.Analytics"%>
<%@ Page Language="c#" Inherits="System.Web.UI.Page" CodePage="65001" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <title>Welcome to Sitecore</title>
  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  <meta name="CODE_LANGUAGE" content="C#" />
  <meta name="vs_defaultClientScript" content="JavaScript" />
  <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
  <link href="/default.css" rel="stylesheet" />
  <sc:VisitorIdentification runat="server" />
</head>
<body> 
  <form id="mainform" method="post" runat="server">
    <div id="MainPanel">
      <sc:placeholder key="main" runat="server" /> 
    </div>
  </form>
 </body>
</html>
