<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="VkOAuth.Index1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <body>
        <form id="form1" runat="server">
        <!-- Возможно: стоит проверять Sessin["UserId"] -->
        <% if (Session["UserId"] == null)
           { %>
          <asp:Button id="AuthButton"
           Text="Click me for greeting..."
           OnClick="AuthButton_Click" 
           runat="server"/>
        <% }
           else
           { %>
            Привет, <%:Session["FirstName"]%>  <%:Session["LastName"]%><br>
            UserID: <%:Session["UserID"]%><br>
            With acess token: <%:Session["AccessToken"]%>    
        <% } %>
    </form>
</body>
</html>
