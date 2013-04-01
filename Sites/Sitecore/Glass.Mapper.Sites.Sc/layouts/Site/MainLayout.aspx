<%@ Page Language="c#" CodePage="65001" AutoEventWireup="true" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html>
<html>
<head>
    <sc:Sublayout runat="server" Path="/layouts/site/misc/metadata.ascx"/>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <script src="//code.jquery.com/jquery.js"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.2.1/js/bootstrap.min.js"></script>
    <link href="//bootswatch.com/slate/bootstrap.min.css" rel="stylesheet" />

    <sc:VisitorIdentification runat="server" />
</head>
<body>
    <form runat="server">
        <sc:Sublayout runat="server" Path="/layouts/Site/Misc/TopNavigationSublayout.ascx" />
        <div class="container">
            <div class="row">
                <div class="span12">
                    <sc:Placeholder runat="server" Key="main" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
