<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberLogin.aspx.cs" Inherits="TripBookingWebApp.Account.MemberLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style3 {
            margin-left: 0px;
        }
        .auto-style5 {
            width: 84px;
        }
        .auto-style6 {
            width: 1179px;
        }
        .auto-style7 {
            width: 14%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="headerLbl" runat="server" Font-Size="15pt" Text="Member Login"></asp:Label>
            <table cellpadding="5pt" class="auto-style7">
                <tr>
                    <td class="auto-style5">Username:</td>
                    <td class="auto-style6">
                        <asp:TextBox ID="usernameTxt" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Password:</td>
                    <td class="auto-style6">
                        <asp:TextBox ID="passwordTxt" runat="server" CssClass="auto-style3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">
                        <asp:Button ID="loginBtn" runat="server" Text="Login" OnClick="loginBtn_Click" />
                    </td>
                    <td class="auto-style6">
                        &nbsp;</td>
                </tr>
                </table>
            <asp:Label ID="errorLbl" runat="server"></asp:Label>
            <br />
            <br />
            New user? Please
            <asp:Button ID="registerBtn" runat="server" OnClick="registerBtn_Click" Text="register." Height="21px" Width="55px" />
            <br />
            <br />
            <asp:Button ID="homeBtn" runat="server" OnClick="homeBtn_Click" Text="Back to Home" />
        </div>
    </form>
</body>
</html>
