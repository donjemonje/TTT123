using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSite
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindRepeater1();
                this.BindRepeater2();
                this.BindPlayerDropDownList();
                this.BindGameDropDownList();
                this.BindRepeater3();
                this.BindRepeater4();
                this.BindRepeater5();
            }
        }

        private void BindRepeater1()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var list = c.GetPlayersInfo();
                channel.Close();

                Repeater1.DataSource = list;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private void BindPlayerDropDownList()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var list = c.GetPlayersInfo();
                channel.Close();


                DropDownList1.DataSource = list.GroupBy(x=> new { x.PlayerName, x.IdCardNo })
                                               .Select(x=> new { Id = x.Key.IdCardNo,Name = x.Key.PlayerName + " - " + x.Key.IdCardNo }).ToList() ;
                DropDownList1.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        private void BindGameDropDownList()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var list = c.GetGameInfo();
                channel.Close();

                DropDownList2.DataSource = list.Select(x => new { Id = x.GameId, Name = x.GroupsName + " ( " + x.CreatedDate + " )"}).ToList();
                DropDownList2.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        private void BindRepeater2()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var list = c.GetGameInfo();
                channel.Close();

                Repeater2.DataSource = list;
                Repeater2.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        protected void OnPlayerSelectionChange(object sender, EventArgs e)
        {
            this.BindRepeater3();
        }
        private void BindRepeater3()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var playerId = DropDownList1.SelectedValue;
                var list = c.GetGameInfoByPlayerId(playerId.ToString());
                channel.Close();

                Repeater3.DataSource = list;
                Repeater3.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        protected void OnGameSelectionChange(object sender, EventArgs e)
        {
            this.BindRepeater4();
        }
        private void BindRepeater4()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var gameId = DropDownList2.SelectedValue;
                var list = c.GetPlayerInfoByGameId(gameId.ToString());
                channel.Close();

                Repeater4.DataSource = list;
                Repeater4.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        private void BindRepeater5()
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();


                var list = c.GetPlayerStatistics();
                channel.Close();

                Repeater5.DataSource = list;
                Repeater5.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
        protected void OnEdit(object sender, EventArgs e)
        {
            //Find the reference of the Repeater Item.
            RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            this.ToggleElements(item, true);
        }

        protected void OnUpdate(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            Repeater repeter = (sender as LinkButton).Parent.Parent as Repeater;

            switch (repeter.ID)
            {
                case "Repeater1":
                    PlayerUpdate(item);
                    break;
                case "Repeater4":
                    PlayerUpdate(item);
                    break;
            }

            BindAll();
        }

        private void BindAll()
        {
            this.BindRepeater1();
            this.BindRepeater2();
            this.BindPlayerDropDownList();
            this.BindGameDropDownList();
            this.BindRepeater3();
            this.BindRepeater4();
            this.BindRepeater5();
        }

        private void PlayerUpdate(RepeaterItem item)
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var id = (item.FindControl("lblPlayerId") as Label).Text;
                var playerName = (item.FindControl("txtPlayerName") as TextBox).Text;
                var playerIdCardNo = (item.FindControl("txtPlayerIdCard") as TextBox).Text;
                var groupName = (item.FindControl("txtGroupName") as TextBox).Text;

                c.UpdatePalyer(id, playerName, playerIdCardNo,groupName);

                channel.Close();
            }
            catch(Exception ex)
            {
                ErrorMessage.Text = ex.Message.ToString();
            }
        }
        protected void OnDelete(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            Repeater repeter = (sender as LinkButton).Parent.Parent as Repeater;

            switch (repeter.ID)
            {
                case "Repeater1":
                    PlayerDelete(item);
                    break;
                case "Repeater2":
                    GameDelete(item);
                    break;
                case "Repeater3":
                    GameDelete(item);
                    break;
                case "Repeater4":
                    PlayerDelete(item);
                    break;
            }

            BindAll();
        }

        private void PlayerDelete(RepeaterItem item)
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var idCardNo = (item.FindControl("lblPlayerIdCard") as Label).Text;
                
                c.DeletePlayer(idCardNo);

                channel.Close();
                
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message.ToString();
            }
        }

        private void GameDelete(RepeaterItem item)
        {
            WebChannelFactory<IRegistrationService> channel = new WebChannelFactory<IRegistrationService>(GlobalParam.address);
            try
            {
                channel.Open();
                IRegistrationService c = channel.CreateChannel();

                var id = (item.FindControl("lblGameId") as Label).Text;

                c.DeleteGame(id);

                channel.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message.ToString();
            }
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            //Find the reference of the Repeater.
            Repeater repeter = (sender as LinkButton).Parent.Parent as Repeater;
            
            switch (repeter.ID)
            {
                case "Repeater1":
                    this.BindRepeater1();
                    break;
                case "Repeater4":
                    this.BindRepeater4();
                    break;
            }

        }

        private void ToggleElements(RepeaterItem item, bool isEdit)
        {
            //Toggle Buttons.
            item.FindControl("lnkEdit").Visible = !isEdit;
            item.FindControl("lnkUpdate").Visible = isEdit;
            item.FindControl("lnkCancel").Visible = isEdit;
            item.FindControl("lnkDelete").Visible = !isEdit;

            //Toggle Labels.
            item.FindControl("lblPlayerIdCard").Visible = !isEdit;
            item.FindControl("lblPlayerName").Visible = !isEdit;
            item.FindControl("lblGroupName").Visible = !isEdit;

            //Toggle TextBoxes.
            item.FindControl("txtPlayerIdCard").Visible = isEdit;
            item.FindControl("txtPlayerName").Visible = isEdit;
            item.FindControl("txtGroupName").Visible = isEdit;

            //Toggle Validators.
            item.FindControl("vldPlayerIdCard").Visible = isEdit;
            item.FindControl("vldPlayerName").Visible = isEdit;
            item.FindControl("vldGroupName").Visible = isEdit;
        }
    }

    [DataContract(Name = "PlayerInfo", Namespace = "")]
    public class PlayerInfo
    {
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public string PlayerName { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int NoOfGames { get; set; }
    }

    [DataContract(Name = "GameInfo", Namespace = "")]
    public class GameInfo
    {
        [DataMember]
        public int GameId { get; set; }
        [DataMember]
        public string GroupsName { get; set; }
        [DataMember]
        public string WinnerGroup { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}