<%@ Page Language="c#" CodePage="65001" AutoEventWireup="true" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html>
<html>
<head>
    <title>Insert the page title here.</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <script src="//code.jquery.com/jquery.js"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.2.1/js/bootstrap.min.js"></script>
    <link href="//bootswatch.com/united/bootstrap.min.css" rel="stylesheet" />

    <sc:VisitorIdentification runat="server" />
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="span12">
                <sc:Placeholder runat="server" Key="main" />
            </div>
        </div>
    </div>
</body>
</html>
