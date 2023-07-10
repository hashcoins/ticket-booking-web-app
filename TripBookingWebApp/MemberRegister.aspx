<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberRegister.aspx.cs" Inherits="TripBookingWebApp.Account.MemberRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 35px;
        }
        .auto-style2 {
            width: 12%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="registerLbl" runat="server" Font-Size="15pt" Text="Member Registration"></asp:Label>
            <table class="auto-style2">
                <tr>
                    <td class="auto-style1">Username:</td>
                    <td>
                        <asp:TextBox ID="usernameTxt" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Password:</td>
                    <td>
                        <asp:TextBox ID="passwordTxt" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Label ID="errorLbl" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Image ID="captchaImg" runat="server" Height="117px" Width="332px" />
            <br />
            Enter the text above into the following textbox before registering.<br />
            <asp:TextBox ID="captchaTxt" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="captchaLbl" runat="server" Visible="False"></asp:Label>
            <br />
            <br />
            <asp:Button ID="registerBtn" runat="server" OnClick="registerBtn_Click" Text="Register Account" />
            <br />
            <br />
            <asp:Button ID="homeBtn" runat="server" OnClick="homeBtn_Click" Text="Back to Home" />
        </div>
    </form>
</body>
</html>
