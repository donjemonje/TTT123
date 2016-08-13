<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebSite.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <p class="text-success">
        <asp:Literal runat="server" ID="SuccessMessage" />
    </p>
    <div class="form-horizontal">
        <hr />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="GroupName" CssClass="col-md-2 control-label">Group Name</asp:Label>
            <div class="col-md-3">
                <asp:TextBox runat="server" ID="GroupName" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="GroupName"
                    CssClass="text-danger" ErrorMessage="The group name field is required." />
            </div>
            <asp:Button runat="server" ID="addPlayerBtn" Text="Add Player" CssClass="col-md-1 btn btn-default" OnClick="addPlayerBtn_Click" />
        </div>
        <hr />
        <asp:PlaceHolder runat="server" ID="placeHolder1"></asp:PlaceHolder>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" ID="registerBtn" Text="Register" CssClass="btn btn-primary" OnClick="registerBtn_Click" />
                <asp:Button runat="server" ID="resetBtn" Text="Clear" CausesValidation="false" CssClass="btn btn-default" OnClick="resetBtn_Click" />
            </div>
        </div>
    </div>
</asp:Content>
