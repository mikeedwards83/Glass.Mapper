<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<style>
    th {
        text-align: left;
    }
</style>
<body>


    <%
        bool withProps = HttpContext.Current.Request.QueryString["withProperties"] == "true";
    %>
    <p>
        <a href="?withProperties=true">Display Properties</a>
    </p>
    <% foreach (KeyValuePair<string, Glass.Mapper.Context> context in Glass.Mapper.Context.Contexts)
        {     %>

    <h1><%= context.Value.Name %></h1>

    <ul>
        <li>Configurations: <%=context.Value.TypeConfigurations.Count %></li>
    </ul>
    <table>
        <tr>
            <th>Type</th>
            <th>IsInterface</th>
            <th>IsAbstract</th>
            <th>Properties</th>
            <th>Cached</th>
            <th>EnforceTemplate</th>
        </tr>inter
        <% foreach (KeyValuePair<Type, Glass.Mapper.Configuration.AbstractTypeConfiguration> config in context.Value.TypeConfigurations.OrderBy(x=>x.Key.FullName))
            {
                Glass.Mapper.Sc.Configuration.SitecoreTypeConfiguration scConfig = config.Value as Glass.Mapper.Sc.Configuration.SitecoreTypeConfiguration;
        %>


        <tr>
            <th>
                <%=config.Value.Type.FullName %>
            </th>
            <td><%=config.Value.Type.IsInterface %></td>
            <td><%=config.Value.Type.IsAbstract %></td>
            <td><%=config.Value.Properties.Count()  %></td>
            <%if (scConfig != null)
                { %>
            <td><%= scConfig.Cache %></td>
            <td><%= scConfig.EnforceTemplate %></td>
            <%} %>
        </tr>
        <%if (withProps)
            { %>


        <%foreach (Glass.Mapper.Configuration.AbstractPropertyConfiguration property in config.Value.Properties)
            { %>
        <tr>
            <td colspan="3"></td>
            <td>
                <%=property.PropertyInfo.Name %>
            </td>
            <td>
                <%=property.Mapper.GetType().Name %>
            </td>
            <td>
                <%=property.Mapper.GetType().Assembly.FullName.StartsWith("Glass.Mapper.Sc") ? "": "Custom" %>
            </td>
        </tr>

        <%} %>

        <%} %>
        <%} %>
    </table>
    <%} %>
</body>
</html>
