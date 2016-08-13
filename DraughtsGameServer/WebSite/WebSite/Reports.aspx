<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="WebSite.Reports" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/Content/bootstrap.min.css") %>" type="text/css" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/Content/bootstrap-select.css") %>" type="text/css" />
    <script src='<%=ResolveClientUrl("~/Scripts/jquery-3.1.0.min.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/Scripts/bootstrap.min.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/Scripts/bootstrap-select.js") %>' type="text/javascript"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(".selectpicker").selectpicker();
            $('.dropdown-toggle').dropdown();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            $(".selectpicker").selectpicker();
            $('.dropdown-toggle').dropdown();
        });
    </script>
    <style>
        
    </style>

    <h2><%: Title %></h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <hr />
    <br />
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#menu1">Players Information</a></li>
        <li><a data-toggle="tab" href="#menu2">Games Information</a></li>
        <li><a data-toggle="tab" href="#menu3">Games According To Player</a></li>
        <li><a data-toggle="tab" href="#menu4">Players According To Game</a></li>
        <li><a data-toggle="tab" href="#menu5">Player Stats</a></li>
    </ul>

    <div class="tab-content">
        <div id="menu1" class="tab-pane fade in active">
            <br />
            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped text-center">
                                <thead>
                                    <tr>
                                        <th class="text-center">ID Card Number</th>
                                        <th class="text-center">Player Name</th>
                                        <th class="text-center">Group Name</th>
                                        <th class="text-center">Created Date</th>
                                        <th></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr align="center">
                                <td>
                                    <asp:Label ID="lblPlayerId" runat="server" Text='<%# Eval("PlayerId") %>' Visible="false" />
                                    <asp:Label ID="lblPlayerIdCard" runat="server" Text='<%# Eval("IdCardNo") %>' />
                                    <asp:TextBox ID="txtPlayerIdCard" class="form-control input-sm" runat="server" Width="120" Text='<%# Eval("IdCardNo") %>' Visible="false" />
                                    <asp:RequiredFieldValidator ID="vldPlayerIdCard" runat="server" Visible="false" ControlToValidate="txtPlayerIdCard"
                                        CssClass="text-danger" ErrorMessage="The ID Card Number field is required." />
                                </td>
                                <td>
                                    <asp:Label ID="lblPlayerName" runat="server" Text='<%# Eval("PlayerName") %>' />
                                    <asp:TextBox ID="txtPlayerName" class="form-control input-sm" runat="server" Width="120" Text='<%# Eval("PlayerName") %>' Visible="false" />
                                    <asp:RequiredFieldValidator ID="vldPlayerName" runat="server" Visible="false" ControlToValidate="txtPlayerName"
                                        CssClass="text-danger" ErrorMessage="The Player Name field is required." />
                                </td>
                                <td>
                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>' />
                                    <asp:TextBox ID="txtGroupName" class="form-control input-sm" runat="server" Width="120" Text='<%# Eval("GroupName") %>' Visible="false" />
                                    <asp:RequiredFieldValidator ID="vldGroupName" runat="server" Visible="false" ControlToValidate="txtGroupName"
                                        CssClass="text-danger" ErrorMessage="The Group Name field is required." />
                                </td>
                                <td>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate") %>' />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkEdit" Text="Edit" OnClick="OnEdit" runat="server" />&nbsp;
                                <asp:LinkButton ID="lnkUpdate" Text="Update" OnClick="OnUpdate" runat="server" Visible="false" />&nbsp;
                                <asp:LinkButton ID="lnkCancel" CausesValidation="false" Text="Cancel" OnClick="OnCancel" runat="server" Visible="false" />&nbsp;
                                <asp:LinkButton ID="lnkDelete" Text="Delete" OnClick="OnDelete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="menu2" class="tab-pane fade">
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="Repeater2" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped text-center">
                                <thead>
                                    <tr>
                                        <th class="text-center">Game Code</th>
                                        <th class="text-center">Game Groups</th>
                                        <th class="text-center">Winner Group</th>
                                        <th class="text-center">Created Date</th>
                                        <th></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 120px;">
                                    <asp:Label ID="lblGameId" runat="server" Text='<%# Eval("GameId") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblGroups" runat="server" Text='<%# Eval("GroupsName") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblWinnerGroup" runat="server" Text='<%# Eval("WinnerGroup") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate") %>' />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkDelete" Text="Delete" OnClick="OnDelete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="menu3" class="tab-pane fade">
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <div class="row" style="padding-left: 30px;">
                        <label for="DropDownList1" class="control-label">Select Player :</label>
                        <asp:DropDownList ID="DropDownList1" CssClass="selectpicker" AutoPostBack="true" runat="server" OnSelectedIndexChanged="OnPlayerSelectionChange" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                    </div>
                    <hr />
                    <asp:Repeater ID="Repeater3" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped text-center">
                                <thead>
                                    <tr>
                                        <th class="text-center">Game Code</th>
                                        <th class="text-center">Game Groups</th>
                                        <th class="text-center">Winner Group</th>
                                        <th class="text-center">Created Date</th>
                                        <th></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 120px;">
                                    <asp:Label ID="lblGameId" runat="server" Text='<%# Eval("GameId") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblGroups" runat="server" Text='<%# Eval("GroupsName") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblWinnerGroup" runat="server" Text='<%# Eval("WinnerGroup") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate") %>' />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkDelete" Text="Delete" OnClick="OnDelete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="menu4" class="tab-pane fade">
            <br />
            <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <div class="row" style="padding-left: 30px;">
                        <label for="DropDownList2" class="control-label">Select Game :</label>
                        <asp:DropDownList ID="DropDownList2" data-width="auto" CssClass="selectpicker" OnSelectedIndexChanged="OnGameSelectionChange" AutoPostBack="true" runat="server" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr />
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="Repeater4" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped text-center">
                                <thead>
                                    <tr>
                                        <th class="text-center">ID Card Number</th>
                                        <th class="text-center">Player Name</th>
                                        <th class="text-center">Group Name</th>
                                        <th class="text-center">Created Date</th>
                                        <th></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr align="center">
                                <td>
                                    <asp:Label ID="lblPlayerId" runat="server" Text='<%# Eval("PlayerId") %>' Visible="false" />
                                    <asp:Label ID="lblPlayerIdCard" runat="server" Text='<%# Eval("IdCardNo") %>' />
                                    <asp:TextBox ID="txtPlayerIdCard" class="form-control input-sm" runat="server" Width="120" Text='<%# Eval("IdCardNo") %>' Visible="false" />
                                    <asp:RequiredFieldValidator ID="vldPlayerIdCard" runat="server" Visible="false" ControlToValidate="txtPlayerIdCard"
                                        CssClass="text-danger" ErrorMessage="The ID Card Number field is required." />
                                </td>
                                <td>
                                    <asp:Label ID="lblPlayerName" runat="server" Text='<%# Eval("PlayerName") %>' />
                                    <asp:TextBox ID="txtPlayerName" class="form-control input-sm" runat="server" Width="120" Text='<%# Eval("PlayerName") %>' Visible="false" />
                                    <asp:RequiredFieldValidator ID="vldPlayerName" runat="server" Visible="false" ControlToValidate="txtPlayerName"
                                        CssClass="text-danger" ErrorMessage="The Player Name field is required." />
                                </td>
                                <td>
                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>' />
                                    <asp:TextBox ID="txtGroupName" class="form-control input-sm" runat="server" Width="120" Text='<%# Eval("GroupName") %>' Visible="false" />
                                    <asp:RequiredFieldValidator ID="vldGroupName" runat="server" Visible="false" ControlToValidate="txtGroupName"
                                        CssClass="text-danger" ErrorMessage="The Group Name field is required." />
                                </td>
                                <td>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate") %>' />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkEdit" Text="Edit" OnClick="OnEdit" runat="server" />&nbsp;
                                        <asp:LinkButton ID="lnkUpdate" Text="Update" OnClick="OnUpdate" runat="server" Visible="false" />&nbsp;
                                        <asp:LinkButton ID="lnkCancel" CausesValidation="false" Text="Cancel" OnClick="OnCancel" runat="server" Visible="false" />&nbsp;
                                        <asp:LinkButton ID="lnkDelete" Text="Delete" OnClick="OnDelete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDownList2" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="menu5" class="tab-pane fade">
            <br />
            <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="Repeater5" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped text-center">
                                <thead>
                                    <tr>
                                        <th class="text-center">ID Card Number</th>
                                        <th class="text-center">Player Name</th>
                                        <th class="text-center">Number of Games</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPlayerIdCard" runat="server" Text='<%# Eval("IdCardNo") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblPlayerName" runat="server" Text='<%# Eval("PlayerName") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblNoOfGame" runat="server" Text='<%# Eval("NoOfGames") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

