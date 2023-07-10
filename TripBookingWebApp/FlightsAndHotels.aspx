<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightsAndHotels.aspx.cs" Inherits="TripBookingWebApp.FlightsAndHotels" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 625px;
        }
        .auto-style2 {
            width: 625px;
            height: 26px;
        }
        .auto-style3 {
            height: 26px;
        }
        .auto-style4 {
            width: 625px;
            height: 38px;
        }
        .auto-style5 {
            height: 38px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="topLbl" runat="server" Font-Size="15pt" Text="Flights and Hotels Selection"></asp:Label>
            <br />
            Select a flight first, and then a hotel.<asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style2">Flights</td>
                        <td class="auto-style3">Hotel Bookings</td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:ListBox ID="flightListBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="flightListBox_SelectedIndexChanged" Rows="1"></asp:ListBox>
                        </td>
                        <td>
                            <asp:ListBox ID="hotelListBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="hotelListBox_SelectedIndexChanged" Rows="1"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Button ID="saveFlightBtn" runat="server" OnClick="saveFlightBtn_Click" Text="Save Selected Flight" />
                        </td>
                        <td>
                            <asp:Button ID="saveHotelBtn" runat="server" OnClick="saveHotelBtn_Click" Text="Save Selected Hotel" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style4">Saved Flight:
                            <asp:Button ID="showSavedFlightBtn" runat="server" BackColor="White" BorderStyle="Dotted" OnClick="showSavedFlightBtn_Click" style="height: 29px" Text="N/A" />
                        </td>
                        <td class="auto-style5">Saved Hotel:
                            <asp:Button ID="showSavedHotelBtn" runat="server" BackColor="White" BorderStyle="Dotted" OnClick="showSavedHotelBtn_Click" Text="N/A" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="loadingImg" runat="server" Height="50px" ImageUrl="~/Images/loadingAnimationLoop.gif" Width="50px" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Label ID="infoLbl" runat="server" Text="Information of Selected Option:"></asp:Label>
                <br />
                <asp:TextBox ID="infoTxt" runat="server" Height="140px" TextMode="MultiLine" Width="1260px"></asp:TextBox>
                <br />
                <asp:Label ID="errorLbl" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                <br />
                Total Price:
                <asp:Label ID="priceLbl" runat="server" Text="N/A"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Button ID="bookBtn" runat="server" Text="Book Flight and Hotel" OnClick="bookBtn_Click" />
        <br />
        <br />
        <asp:Button ID="homeBtn" runat="server" Text="Back to Home" />
        <br />
    </form>
</body>
</html>
