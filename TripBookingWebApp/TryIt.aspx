<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt.aspx.cs" Inherits="TripBookingWebApp.WebForm1" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

  * {
    color: #000 !important;
    text-shadow: none !important;
    background: transparent !important;
    -webkit-box-shadow: none !important;
    box-shadow: none !important;
  }
    * {
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  box-sizing: border-box;
}
        .auto-style2 {
            width: 50%;
        }
        .auto-style3 {
            height: 101px;
            width: 50%;
        }
        .auto-style1 {
            height: 101px;
        }
        .auto-style5 {
            width: 50%;
            height: 23px;
        }
        .auto-style6 {
            height: 23px;
        }
        .auto-style7 {
            width: 50%;
            height: 155px;
        }
        .auto-style8 {
            height: 155px;
        }
        .auto-style4 {
            width: 50%;
        }
        .auto-style9 {
            height: 53px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="tryItLbl" runat="server" Font-Size="25pt" Text="Try It"></asp:Label>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <br />
    <asp:Button ID="homeBtn" runat="server" OnClick="homeBtn_Click" Text="Home" />
            <br />
            <br />
            <asp:Label ID="domingLbl" runat="server" Font-Size="20pt" Text="Cristion Dominguez"></asp:Label>
            <asp:UpdatePanel ID="encryptionPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="encryptionLbl" runat="server" Font-Size="15pt" Text="Encryption DLL"></asp:Label>
                    <br />
                    Input:
                    <asp:TextBox ID="encInputTxt" runat="server" Width="50%"></asp:TextBox>
                    <br />
                    <asp:Button ID="encryptBtn" runat="server" OnClick="encryptBtn_Click" Text="Encrypt" />
                    &nbsp;&nbsp;
                    <asp:Button ID="decryptBtn" runat="server" OnClick="decryptBtn_Click" Text="Decrypt" />
                    <br />
                    Output:
                    <asp:Label ID="encOutputLbl" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:UpdatePanel ID="xmlPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="xmlLbl" runat="server" Font-Size="15pt" Text="Member.xml Manipulation"></asp:Label>
                    &nbsp;<asp:Label ID="xmlLoadErrorLbl" runat="server"></asp:Label>
                    <table style="width: 100%;">
                        <tr>
                            <td class="auto-style2">Add Member</td>
                            <td>Authenticate Member</td>
                        </tr>
                        <tr>
                            <td class="auto-style3">Username:
                                <asp:TextBox ID="addUsernameTxt" runat="server"></asp:TextBox>
                                <br />
                                Password:
                                <asp:TextBox ID="addPasswordTxt" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="addBtn" runat="server" OnClick="addBtn_Click" Text="Add" />
                                <br />
                                <asp:Label ID="addResultLbl" runat="server"></asp:Label>
                                <br />
                            </td>
                            <td class="auto-style1">Username:
                                <asp:TextBox ID="authUsernameTxt" runat="server"></asp:TextBox>
                                <br />
                                Password:
                                <asp:TextBox ID="authPasswordTxt" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="authBtn" runat="server" OnClick="authBtn_Click" Text="Authenticate" />
                                <br />
                                <asp:Label ID="authResultLbl" runat="server"></asp:Label>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <br />
                    Member.xml File:<br />
                    <asp:TextBox ID="memberTxt" runat="server" Height="400px" TextMode="MultiLine" Width="100%" ReadOnly="True" Wrap="False"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:UpdatePanel ID="bookingPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="bookingLbl" runat="server" Font-Size="15pt" Text="AirportAndHotel Service"></asp:Label>
                    <table style="width:100%;">
                        <tr>
                            <td class="auto-style5">GetFlightBookings Operation</td>
                            <td class="auto-style6">GetHotelBooking Operation</td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Origin Latitude:
                                <asp:TextBox ID="flightOriginLatTxt" runat="server"></asp:TextBox>
                                <br />
                                Origin Longitude:
                                <asp:TextBox ID="flightOriginLonTxt" runat="server"></asp:TextBox>
                                <br />
                                Destination Latitude:
                                <asp:TextBox ID="flightDestLatTxt" runat="server"></asp:TextBox>
                                <br />
                                Destination Longitude:
                                <asp:TextBox ID="flightDestLonTxt" runat="server"></asp:TextBox>
                                <br />
                                Depart Date:
                                <asp:TextBox ID="flightDepartDateTxt" placeholder="(MM/DD/YYYY)" runat="server"></asp:TextBox>
                                <br />
                                Return Date:
                                <asp:TextBox ID="flightReturnDateTxt" placeholder="(MM/DD/YYYY)" runat="server"></asp:TextBox>
                                <br />
                                Adult Count:
                                <asp:TextBox ID="flightAdultsTxt" runat="server"></asp:TextBox>
                                <br />
                                Children Count:
                                <asp:TextBox ID="flightChildrenTxt" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button ID="flightBtn" runat="server" OnClick="flightBtn_Click" Text="Get Flights" />
                            </td>
                            <td class="auto-style8">Destination Latitude:
                                <asp:TextBox ID="hotelDestLatTxt" runat="server"></asp:TextBox>
                                <br />
                                Destination Longitude:
                                <asp:TextBox ID="hotelDestLonTxt" runat="server"></asp:TextBox>
                                <br />
                                Check-In Date:
                                <asp:TextBox ID="hotelCheckInTxt" placeholder="(MM/DD/YYYY)" runat="server"></asp:TextBox>
                                <br />
                                Check-Out Date:
                                <asp:TextBox ID="hotelCheckOutTxt" placeholder="(MM/DD/YYYY)" runat="server"></asp:TextBox>
                                <br />
                                Room Count:
                                <asp:TextBox ID="hotelRoomsTxt" runat="server"></asp:TextBox>
                                <br />
                                Adult Count:
                                <asp:TextBox ID="hotelAdultsTxt" runat="server"></asp:TextBox>
                                <br />
                                Children Count:
                                <asp:TextBox ID="hotelChildrenTxt" runat="server"></asp:TextBox>
                                <br />
                                <br />
                                <asp:Button ID="hotelBtn" runat="server" OnClick="hotelBtn_Click" Text="Get Hotels" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9" colspan="2">
                                <asp:UpdateProgress ID="bookingProgress" runat="server">
                                    <ProgressTemplate>
                                        <asp:Image ID="loadingImg" runat="server" Height="50px" ImageUrl="~/Images/loadingAnimationLoop.gif" Width="50px" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:TextBox ID="flightResultTxt" runat="server" Height="400px" ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="hotelResultTxt" runat="server" Height="400px" ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
                    <asp:Label ID="thompsonLbl" runat="server" Font-Size="20pt" Text="Hunter Thompson"></asp:Label>
                    <br />
                    <asp:Label ID="usersHeadLbl" runat="server" Font-Size="15pt" Text="Users on Site"></asp:Label>
                    <br />
                    The amount of users on this site is currently
                    <asp:Label ID="usersAmountLbl" runat="server"></asp:Label>
                    .<br />
                    <asp:UpdatePanel ID="forecastPanel" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="forecastLbl" runat="server" Font-Size="15pt" Text="Weather Forecast Service"></asp:Label>
                            <br />
                            Enter a US valid zipcode to gain a 5 day forecast of the weather at that location.<br /> <br /> Zipcode:
                            <asp:TextBox ID="zipcodeTextBox" runat="server"></asp:TextBox>
                            <br />
                            <p>
                                Check box to rememer this location during the current session (ID: <asp:Label ID="sessionIdLbl" runat="server"></asp:Label>
                                ).
                                <asp:CheckBox ID="sessionCheckBox" runat="server" />
                            </p>
                            <p>
                                Check box to remember this location upon revisiting this website.
                                <asp:CheckBox ID="cookieCheckBox" runat="server" />
                            </p>
                            <p>
                                Saved Cities for Session:
                                <asp:ListBox ID="sessionCitiesListBox" runat="server" Height="34px" Width="121px"></asp:ListBox>
                            </p>
                            <p>
                                <asp:Button ID="zipcodeSubmitBtn" runat="server" OnClick="zipcodeSubmitBtn_Click" Text="Submit" />
                            </p>
                            <p>
                                <asp:Label ID="sessionStatusLbl" runat="server"></asp:Label>
                            </p>
                            <p>
                                <asp:Label ID="cookieStatusLbl" runat="server"></asp:Label>
                            </p>
                            <p>
                                &nbsp;</p>
                            <p>
                                <asp:Label ID="selectedCityLbl" runat="server" Text="City:"></asp:Label>
                            </p>
                            <p>
                                <asp:ListBox ID="day1ListBox" runat="server" Height="86px" Width="129px">
                                    <asp:ListItem> </asp:ListItem>
                                </asp:ListBox>
                                &nbsp;
                                <asp:Label ID="day1Lbl" runat="server"></asp:Label>
                            </p>
                            <p>
                                <asp:ListBox ID="day2ListBox" runat="server" Height="86px" Width="129px">
                                    <asp:ListItem> </asp:ListItem>
                                </asp:ListBox>
                                &nbsp;
                                <asp:Label ID="day2Lbl" runat="server"></asp:Label>
                            </p>
                            <p>
                                <asp:ListBox ID="day3ListBox" runat="server" Height="86px" Width="129px">
                                    <asp:ListItem> </asp:ListItem>
                                </asp:ListBox>
                                &nbsp;
                                <asp:Label ID="day3Lbl" runat="server"></asp:Label>
                            </p>
                            <p>
                                <asp:ListBox ID="day4ListBox" runat="server" Height="86px" Width="129px">
                                    <asp:ListItem> </asp:ListItem>
                                </asp:ListBox>
                                &nbsp;
                                <asp:Label ID="day4Lbl" runat="server"></asp:Label>
                            </p>
                            <p>
                                <asp:ListBox ID="day5ListBox" runat="server" Height="86px" Width="129px">
                                    <asp:ListItem> </asp:ListItem>
                                </asp:ListBox>
                                &nbsp;
                                <asp:Label ID="day5Lbl" runat="server"></asp:Label>
                            </p>
                        </ContentTemplate>
        </asp:UpdatePanel>
        <p>
                    <asp:Label ID="danielLbl" runat="server" Font-Size="20pt" Text="Secret Daniel"></asp:Label>
                    </p>
        <asp:UpdatePanel ID="captchaPanel" runat="server">
            <ContentTemplate>
                <asp:Label ID="captchaLbl" runat="server" Font-Size="15pt" Text="Captcha"></asp:Label>
                <br />
                <asp:Image ID="captchaImg" runat="server" Height="117px" Width="332px" />
                <br />
                <asp:TextBox ID="captchaTxt" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="captchaResultLbl" runat="server"></asp:Label>
                <br />
                <asp:Button ID="captchaBtn" runat="server" OnClick="captchaBtn_Click" Text="Compare Text" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="captchaChangeBtn" runat="server" OnClick="captchaChangeBtn_Click" Text="Change Image" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="vacationPanel" runat="server">
            <ContentTemplate>
                <p>
                    <asp:Label ID="vacationLbl" runat="server" Font-Size="15pt" Text="Vacation Service"></asp:Label>
                </p>
                <p>
                    Origin Latitude:
                    <asp:TextBox ID="vacationLatTextBox" runat="server"></asp:TextBox>
                </p>
                <p>
                    Origin Longitude:
                    <asp:TextBox ID="vacationLonTextBox" runat="server"></asp:TextBox>
                </p>
                <p>
                    <asp:Button ID="vacationBtn" runat="server" Text="Find Vacation Location" OnClick="vacationBtn_Click" />
                </p>
                <p>
                    <asp:Label ID="vacationResultLbl" runat="server" Text="Result: "></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>     
    </form>
</body>
</html>
