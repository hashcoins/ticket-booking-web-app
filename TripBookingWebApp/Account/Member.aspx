<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="TripBookingWebApp.Member" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 101px;
        }
        .auto-style2 {
            width: 50%;
        }
        .auto-style3 {
            height: 101px;
            width: 50%;
        }
        .auto-style4 {
            width: 50%;
        }
        .auto-style5 {
            width: 50%;
            height: 23px;
        }
        .auto-style9 {
            height: 400px;
        }
        .auto-style10 {
            width: 100%;
        }
        .auto-style11 {
            height: 30px;
        }
    * {
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  box-sizing: border-box;
}
  *,
  *:before,
  *:after {
    color: #000 !important;
    text-shadow: none !important;
    background: transparent !important;
    -webkit-box-shadow: none !important;
    box-shadow: none !important;
  }
  select[multiple],
select[size] {
  height: auto;
}
button,
select {
  text-transform: none;
}
        .auto-style12 {
            height: 32px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="headerLbl" runat="server" Font-Size="15pt" Text="Member Page"></asp:Label>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <asp:Button ID="homeBtn" runat="server" OnClick="homeBtn_Click" Text="Back to Home" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="signOutBtn" runat="server" OnClick="signOutBtn_Click" Text="Sign Out" style="height: 26px" />
            <br />
            <asp:Label ID="loadErrorLbl" runat="server"></asp:Label>
            <br />
            <asp:Label ID="cartLbl" runat="server" Font-Size="14pt" Text="Shopping Cart"></asp:Label>
            <table class="auto-style10">
                <tr>
                    <td class="auto-style5">Flight Booking</td>
                    <td class="auto-style5">Hotel Booking</td>
                </tr>
                <tr>
                    <td class="auto-style9">
        <asp:TextBox ID="savedFlightTxt" runat="server" Height="100%" TextMode="MultiLine" Width="100%" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td class="auto-style9">
        <asp:TextBox ID="savedHotelTxt" runat="server" Height="100%" TextMode="MultiLine" Width="100%" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style11">Price:
                        <asp:Label ID="flightPriceLbl" runat="server">$0</asp:Label>
                    </td>
                    <td class="auto-style11">Price:
                        <asp:Label ID="hotelPriceLbl" runat="server">$0</asp:Label>
                    </td>
                </tr>
            </table>
            <br />
        <asp:Label ID="totalPriceLbl" runat="server" Font-Size="13pt" Text="Total Price: $0"></asp:Label>
            <br />
            <br />
            <asp:Button ID="purchaseBtn" runat="server" Text="Purchase Flight and Hotel Bookings" OnClick="purchaseBtn_Click" />
&nbsp;&nbsp;&nbsp;
            <br />
            <asp:Label ID="cartErrorLbl" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Label ID="planLbl" runat="server" Font-Size="14pt" Text="Plan a Vacation"></asp:Label>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    Origin Latitude:
                    <asp:TextBox ID="originLatTxt" runat="server"></asp:TextBox>
                    <br />
                    Origin Longitude:
                    <asp:TextBox ID="originLonTxt" runat="server"></asp:TextBox>
                    <br />
                    Depart Date:
                    <asp:TextBox ID="departDateTxt" placeholder="(MM/DD/YYYY)" runat="server"></asp:TextBox>
                    <br />
                    Return Date:
                    <asp:TextBox ID="returnDateTxt" placeholder="(MM/DD/YYYY)" runat="server"></asp:TextBox>
                    <br />
                    Adult Count:
                    <asp:TextBox ID="adultsTxt" runat="server"></asp:TextBox>
                    <br />
                    Children Count:
                    <asp:TextBox ID="childrenTxt" runat="server"></asp:TextBox>
                    <br />
                    Hotel Rooms:
                    <asp:TextBox ID="roomsTxt" runat="server"></asp:TextBox>
                    <br />
                    <asp:Button ID="findVacationBtn" runat="server" OnClick="findVacationBtn_Click" Text="Find Vacation Spot" />
                    <br />
                    <asp:Label ID="vacationLbl" runat="server" Text="Vacation Location: "></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            Select a flight booking, and then a hotel booking.<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style2" style="height: 20px">Flight Bookings</td>
                        <td class="auto-style3" style="height: 20px">Hotel Bookings</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="height: 22px">
                            <asp:ListBox ID="flightListBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="flightListBox_SelectedIndexChanged" Rows="1"></asp:ListBox>
                        </td>
                        <td style="height: 22px">
                            <asp:ListBox ID="hotelListBox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="hotelListBox_SelectedIndexChanged" Rows="1"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style12">
                            <asp:Button ID="saveFlightBtn" runat="server" OnClick="saveFlightBtn_Click" Text="Save Selected Flight" />
                        </td>
                        <td class="auto-style12">
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
                Total Price:
                <asp:Label ID="planTotalPriceLbl" runat="server" Text="N/A"></asp:Label>
                <br />
                <asp:Label ID="planErrorLbl" runat="server" ForeColor="Red"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
            <asp:Button ID="moveToCartBtn" runat="server" OnClick="moveToCartBtn_Click" Text="Transfer Bookings to Shopping Cart" />
            <br />
        </div>
    </form>
</body>
</html>