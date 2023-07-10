<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TripBookingWebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <p/>
    <p style="text-align:center">
        <asp:Label ID="homeLbl" runat="server" Font-Size="25pt" Text="Home"></asp:Label>
    </p>
    <p>
        <meta charset="utf-8" />
        <meta charset="utf-8" />
        <meta charset="utf-8" />
        <b id="docs-internal-guid-c0719415-7fff-4dda-4c75-616fc23b2580" style="font-weight:normal;"><span style="font-size:12pt;font-family:'Open Sans',sans-serif;color:#000000;background-color:transparent;font-weight:400;font-style:normal;font-variant:normal;text-decoration:none;vertical-align:baseline;white-space:pre;white-space:pre-wrap;">The services in this website request the user&#39;s latitude, longitude, departe date, return date, adult count, children count, and room count. After acquiring the data, the services determine a vacation spot in latitude and longitude, and allows the user to choose a flight to and a hotel at said spot. To utilize these services, click &quot;Member Page&quot; button to register or log in.</span></b></p>
    <meta charset="utf-8" />
    <b id="docs-internal-guid-24ab333c-7fff-c0bb-7988-08a4550eaceb" style="font-weight: normal;">
    </b>
    <br class="Apple-interchange-newline" />
<p>
    <asp:Button ID="memberBtn" runat="server" OnClick="memberBtn_Click" Text="Member Page" />
&nbsp;&nbsp;&nbsp;
    <asp:Button ID="staffBtn" runat="server" OnClick="staffBtn_Click" Text="Staff Page" />
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="tryItBtn" runat="server" OnClick="tryItBtn_Click" Text="Try It Page" />
    </p>
    <p class="c4 c13">
        <span class="c2"></span>
    </p>
    <a id="t.ad945891aa229e5e5ef8ca7e42a864804ad2318b"></a>
    <table class="c19" id="summaryTable" style="width: 100%">
        <tr class="c8">
            <td class="c11" colspan="4" style="text-align:center; height: 60px;">
        <asp:Label ID="summaryTableLbl" runat="server" Font-Size="20pt" Text="Applications and Components Summary Table"></asp:Label>
            </td>
        </tr>
        <tr class="c8">
            <td class="c11" colspan="4" rowspan="1" style="text-align:center; height: 141px;">
                <p class="c14">
                    <span class="c2">Percentage of Contribution:</span></p>
                <p class="c14">
                    <span class="c2">Cristion Dominguez — 40%</span></p>
                <p class="c14">
                    <span class="c2">Secret Daniel — 30%</span></p>
                <p class="c14">
                    <span class="c2">Hunter Thompson — 30%</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px; height: 38px; text-align:center">
                <p class="c1">
                    <span class="c2">
                    <asp:Label ID="providerLbl" runat="server" Font-Size="15pt" Text="Provider"></asp:Label>
                    </span></p>
            </td>
            <td class="c9" colspan="1" rowspan="1" style="height: 38px; text-align:center; width: 312px;">
                <p class="c1">
                    <span class="c2">
                    <asp:Label ID="componentTypeLbl" runat="server" Font-Size="15pt" Text="Component Type" Width="100%"></asp:Label>
                    </span></p>
            </td>
            <td class="c9" colspan="1" rowspan="1" style="width: 498px; height: 38px; text-align:center">
                <p class="c1">
                    <span class="c2">
                    <asp:Label ID="componentDescLbl" runat="server" Font-Size="15pt" Text="Component Description"></asp:Label>
                    </span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px; height: 38px; text-align:center">
                <p class="c1">
                    <span class="c2">
                    <asp:Label ID="resourcesLbl" runat="server" Font-Size="15pt" Text="Resources and Location"></asp:Label>
                    </span>
                </p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Cristion Dominguez</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    <span class="c2">DLL</span></p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <span class="c2"><strong>Encryption/Decryption</strong></span></p>
                <p class="c1">
                    <span class="c2">Performs cipher encryption or decryption on each character of the input string.</span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c2">Input: string</span></p>
                <p class="c1">
                    <span class="c2">Output: string</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    <span class="c2">The cipher is manually coded by Cristion Dominguez based on DES.</span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c2">The cipher is utilized in the Member Login and Member Register pages for encrypting and decrypting passwords.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Cristion Dominguez</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    <span class="c2">XML Manipulation</span></p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">

                <p class="c1">
                    <strong>Member.xml</strong></p>
                    <p class="c1">
                        <span class="c2">Stores members’ usernames, passwords, saved flight booking, and saved hotel booking.</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    <span class="c2">The manipulation is linked to the Member Login (searching through usernames and encrypted passwords), Member Register (adding a username and encrypted password), and Member (adding saved flight and hotel bookings) pages.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Cristion Dominguez</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    <span class="c2">SVC Service</span></p>
                <p class="c1">
                    <a href="http://localhost:54814/AirportAndHotelService.svc">http://localhost:54814/AirportAndHotelService.svc</a></p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <span class="c2"><strong>Airport and Hotel Service</strong></span></p>
                <p class="c1">
                    <span class="c0" style="text-decoration: underline">GetFlightBookings Operation</span></p>
                <p class="c1">
                    Returns data on available flight itineraries from location to another.</p>
                <p class="c1">
                    <span class="c16">Inputs:</span></p>
                <p class="c1">
                    <span class="c2">• string originLocation</span></p>
                <p class="c1">
                    <span class="c2">• string originLatitude</span></p>
                <p class="c1">
                    <span class="c2">• string destinationLatitude</span></p>
                <p class="c1">
                    <span class="c2">• string destinationLongitude</span></p>
                <p class="c1">
                    <span class="c2">• string departDate</span></p>
                <p class="c1">
                    <span class="c2">• string returnDate</span></p>
                <p class="c1">
                    <span class="c2">• int adults&nbsp; // amount of adults</span></p>
                <p class="c1">
                    <span class="c2">• int children&nbsp; // amount of children</span></p>
                <p class="c1">
                    <span class="c16">Output:</span></p>
                <p class="c1">
                    <span class="c2">• string[][]&nbsp; // each row has information on an available flight booking</span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c0" style="text-decoration: underline">GetHotelBookings Operation</span></p>
                <p class="c1">
                    Returns data on available hotel bookings at a specified location.</p>
                <p class="c1">
                    <span class="c15">Inputs:</span></p>
                <p class="c1">
                    <span class="c2">• string latitude</span></p>
                <p class="c1">
                    <span class="c2">• string longitude</span></p>
                <p class="c1">
                    <span class="c2">• string checkInDate</span></p>
                <p class="c1">
                    <span class="c2">• string checkOutDate</span></p>
                <p class="c1">
                    <span class="c2">• string rooms&nbsp; // amount of rooms</span></p>
                <p class="c1">
                    <span class="c2">• string adults&nbsp; // amount of adults</span></p>
                <p class="c1">
                    <span class="c2">• string children&nbsp; // amount of children</span></p>
                <p class="c1">
                    <span class="c12">
                    <br />
                    </span><span class="c16">Output:</span></p>
                <p class="c1">
                    <span class="c2">• string[][]&nbsp; </span><span class="c12">// each row has information on an available hotel booking</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    <span class="c2">Airplane flight data shall be retrieved from:</span></p>
                <p class="c1">
                    <span class="c6"><a class="c10" href="https://www.google.com/url?q=https://tequila-api.kiwi.com/v2&amp;sa=D&amp;source=editors&amp;ust=1638145507440000&amp;usg=AOvVaw3AXq0fBOcue2_1RLjQRvd-">https://tequila-api.kiwi.com/v2</a></span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c2">Hotel booking data at a location shall be from:</span></p>
                <p class="c1">
                    <span class="c6"><a class="c10" href="https://www.google.com/url?q=http://api.hotwire.com/v1/search/hotel&amp;sa=D&amp;source=editors&amp;ust=1638145507441000&amp;usg=AOvVaw3pZEqk-zMws0JdV4EMCs5O">http://api.hotwire.com/v1/search/hotel</a></span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c12">Since the Hotwire API does not provide hotel identities, the identity of a hotel shall be randomly selected from </span><span class="c6"><a class="c10" href="https://www.google.com/url?q=https://api.yelp.com/v3&amp;sa=D&amp;source=editors&amp;ust=1638145507442000&amp;usg=AOvVaw1SUFaKxcO79tvcgL3Jnv9T">https://api.yelp.com/v3</a></span><span class="c2">&nbsp;centered around the vacation spot.</span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c2">The service is implemented in the Member page.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Secret Daniel</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    Captcha</p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <span class="c2">Shows a picture of words and letters put together, which the user must replicate in a textbox before gaining access to another page.</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    T<span class="c2">he functionality is implemented within the Member Register page.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Secret Daniel</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    SVC Service</p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <strong>Vacation Service</strong></p>
                <p class="c1">
                    <span class="c2">Gets the location of a vacation spot based on the position inputted by the user.</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    <span class="c2">The service scans a website for available vacation spots and it is utilized in the Member page.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Secret Daniel</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    <span class="c2">XML manipulation</span></p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <strong>Staff.xml</strong></p>
                <p class="c1">
                    <span class="c2">Stores staff&#39;s usernames and passwords, and allows staff usernames and passwords to be added.</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    T<span class="c2">he manipulation is implemented in the Staff page.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Hunter Thompson</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    <span class="c2">Cookies</span></p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <strong>Member Info</strong></p>
                <p class="c1">
                    <span class="c2">Saves member credentials for the next time the user visits the website.</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    Cookies are saved in the Member Register and Member Loging pages. Cookies are retrieved in the Member page.</p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Hunter Thompson</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    <span class="c2">Global Event Handler</span></p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    T<span class="c2">he handler tracks current visitors on the website.</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    T<span class="c2">he handler is coded within the Global.asax file.</span></p>
            </td>
        </tr>
        <tr class="c8">
            <td class="c3" colspan="1" rowspan="1" style="width: 58px">
                <p class="c1">
                    <span class="c2">Hunter Thompson</span></p>
            </td>
            <td colspan="1" rowspan="1" class="modal-sm" style="width: 312px">
                <p class="c1">
                    SVC Service</p>
            </td>
            <td class="modal-sm" colspan="1" rowspan="1" style="width: 498px">
                <p class="c1">
                    <strong>Weather Service</strong></p>
                <p class="c1">
                    R<span class="c2">eturns a 5 Day Forecast given a zip code within the US.</span></p>
                <p class="c1">
                    &nbsp;</p>
                <p class="c1">
                    <span class="c2">Input: int Zipcode</span></p>
                <p class="c1">
                    <span class="c2">Output: string[] Forecast</span></p>
            </td>
            <td class="c7" rowspan="1" style="width: 347px">
                <p class="c1">
                    <span class="c2">Data will be returned by json through the api: api.openweathermap.org/data/2.5/forecast?zip={zipcode},us&amp;appid={apiKey}</span></p>
            </td>
        </tr>
    </table>
    <p class="c1 c4">
        <span class="c18"></span>
    </p>
<p>
    &nbsp;</p>

</asp:Content>