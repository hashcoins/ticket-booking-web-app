<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="TripBookingWebApp.Account.ShoppingCart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 608px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="topLbl" runat="server" Font-Size="15pt" Text="Shopping Cart"></asp:Label>
            <table style="width:100%;">
                <tr>
                    <td class="auto-style1">Flight</td>
                    <td>Hotel</td>
                </tr>
                <tr>
                    <td class="auto-style1">
        <asp:TextBox ID="savedFlightTxt" runat="server" Height="100%" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                    <td>
        <asp:TextBox ID="savedHotelTxt" runat="server" Height="100%" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Price:
                        <asp:Label ID="flightPriceLbl" runat="server"></asp:Label>
                    </td>
                    <td>Price:
                        <asp:Label ID="hotelPriceLbl" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="errorLbl" runat="server" EnableTheming="True" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:Label ID="totalPriceLbl" runat="server" Font-Size="13pt" Text="Total Price:"></asp:Label>
        <br />
        <br />
        <asp:Button ID="checkoutBtn" runat="server" OnClick="checkoutBtn_Click" Text="Checkout" />
        <br />
        <br />
        <asp:Button ID="homeBtn" runat="server" OnClick="homeBtn_Click" Text="Back to Home" />
    </form>
</body>
</html>
