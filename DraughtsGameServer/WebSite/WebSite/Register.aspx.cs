using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSite
{
    public partial class Register : System.Web.UI.Page
    {
        List<string> controlsIdList = new List<string>();
        int counter = 0;
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            controlsIdList = (List<string>)ViewState["controlsIdList"];

            foreach (string Id in controlsIdList)
            {
                counter++;
                CreatePlayerDataRow(counter.ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["status"] != null)
            {
                string strStatus = Request.QueryString["status"].ToString();
                string strGameId = Request.QueryString["gameid"].ToString();

                if (strStatus.ToUpper() == "REGISTERED")
                    SuccessMessage.Text = "You are registered successfully, your game code is " + strGameId;
            }
        }

        protected void addPlayerBtn_Click(object sender, EventArgs e)
        {
            counter++;
            CreatePlayerDataRow(counter.ToString());
            controlsIdList.Add(counter.ToString());

            ViewState["controlsIdList"] = controlsIdList;
        }

        private void CreatePlayerDataRow(string Id)
        {
            TextBox nameTb = new TextBox();
            TextBox idCardTb = new TextBox();
            Label nameLabel = new Label();
            Label idCardLabel = new Label();
            RequiredFieldValidator valid1 = new RequiredFieldValidator();
            RequiredFieldValidator valid2 = new RequiredFieldValidator();

            nameTb.ID = "nameTb" + Id;
            nameLabel.AssociatedControlID = nameTb.ID;
            nameLabel.Text = "Player Name";
            nameLabel.ID = "namelab" + Id;
            valid1.ControlToValidate = nameTb.ID;
            valid1.ErrorMessage = "The player name field is required.";
            valid1.ID = "valid1" + Id;

            idCardTb.ID = "idCardTb" + Id;
            idCardTb.TextMode = TextBoxMode.Number;
            idCardLabel.AssociatedControlID = idCardTb.ID;
            idCardLabel.Text = "ID Card Number";
            idCardLabel.ID = "idCardLabel" + Id;
            valid2.ControlToValidate = idCardTb.ID;
            valid2.ErrorMessage = "The ID card number field is required.";
            valid2.ID = "valid2" + Id;

            nameTb.CssClass = idCardTb.CssClass = "form-control";
            nameLabel.CssClass = idCardLabel.CssClass = "col-sm-2 control-label";
            valid1.CssClass = valid2.CssClass = "text-danger";

            placeHolder1.Controls.Add(new LiteralControl("<div class='form-group'>"));
            placeHolder1.Controls.Add(nameLabel);
            placeHolder1.Controls.Add(new LiteralControl("<div class='col-sm-3'>"));
            placeHolder1.Controls.Add(nameTb);
            placeHolder1.Controls.Add(valid1);
            placeHolder1.Controls.Add(new LiteralControl("</div>"));
            placeHolder1.Controls.Add(idCardLabel);
            placeHolder1.Controls.Add(new LiteralControl("<div class='col-sm-3'>"));
            placeHolder1.Controls.Add(idCardTb);
            placeHolder1.Controls.Add(valid2);
            placeHolder1.Controls.Add(new LiteralControl("</div>"));
            placeHolder1.Controls.Add(new LiteralControl("</div>"));
        }

        protected void resetBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Register.aspx", true);
        }

        protected void registerBtn_Click(object sender, EventArgs e)
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                string gameId = c.RegistrationGame();
                string groupId = c.CreateGroup(GroupName.Text,gameId);
                

                foreach (var id in controlsIdList)
                {
                    TextBox nameTb = placeHolder1.FindControl("nameTb" + id) as TextBox;
                    TextBox idCardTb = placeHolder1.FindControl("idCardTb" + id) as TextBox;

                    c.AddPlayer(groupId, nameTb.Text, idCardTb.Text);
                }

                channel.Close();

                Response.Redirect("~/Register.aspx?status=registered&gameid=" + groupId, true);
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
    }

    [ServiceContract]
    public interface IRegistrationService
    {
        [OperationContract]
        [WebGet(UriTemplate = "CreateGroup/{name}/{gameId}")]
        string CreateGroup(string name, string gameId);
        [OperationContract]
        [WebInvoke(UriTemplate = "UpdateGroup/{name}")]
        void UpdateGroup(string groupId, string name);
        [OperationContract]
        [WebInvoke(UriTemplate = "AddPlayer/{groupId}/{playerName}/{playerIDCard}")]
        void AddPlayer(string groupId, string playerName, string playerIdCard);
        [OperationContract]
        [WebInvoke(UriTemplate = "UpdatePalyer/{id}/{playerName}/{playerIdCard}/{groupName}")]
        void UpdatePalyer(string id, string playerName, string playerIdCard,string groupName);
        [OperationContract]
        [WebInvoke(UriTemplate = "DeletePlayer/{id}")]
        void DeletePlayer(string id);
        [OperationContract]
        [WebGet(UriTemplate = "RegistrationGame")]
        string RegistrationGame();
        [OperationContract]
        [WebGet(UriTemplate = "DeleteGame/{id}")]
        void DeleteGame(string id);
        [OperationContract]
        [WebGet()]
        List<PlayerInfo> GetPlayersInfo();
        [OperationContract]
        [WebGet]
        List<GameInfo> GetGameInfo();
        [OperationContract]
        [WebGet(UriTemplate = "GetGameInfoByPlayerId/{playerIdCardNo}")]
        List<GameInfo> GetGameInfoByPlayerId(string playerIdCardNo);
        [OperationContract]
        [WebGet(UriTemplate = "GetPlayerInfoByGameId/{gameId}")]
        List<PlayerInfo> GetPlayerInfoByGameId(string gameId);
        [OperationContract]
        [WebGet]
        List<PlayerInfo> GetPlayerStatistics();
    }
}

    