using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClintTest.GameService;
using System.ServiceModel;
using System.Threading;

namespace ClintTest
{
    public partial class Form1 : Form
    {
        private GameServiceClient c = new GameServiceClient(new System.ServiceModel.InstanceContext(new CallBack()));
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtGameCode.Text != "")
            {
                var groupData = c.LogIn(Int32.Parse(txtGameCode.Text));

                txtMoves1.AppendText("Group Name :" + groupData.GroupName + Environment.NewLine);
                txtMoves1.AppendText("Player No :" + groupData.PlayerNo + Environment.NewLine);

                foreach (var player in groupData.PlayersNames)
                {
                    txtMoves1.AppendText("Player Name :" + player + Environment.NewLine);
                }

                c.Status(Int32.Parse(txtGameCode.Text));
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblTurn.Text = "";
            CallBack.SetDelegate(UpdateStausLabel);
            CallBack.SetDelegate(InitBoard);
            CallBack.SetDelegate(UpdateBoard);
        }
        private void InitBoard(GroupData rival, bool yourTurn)
        {
            txtMoves2.AppendText("Group Name :" + rival.GroupName + Environment.NewLine);
            txtMoves2.AppendText("Player No :" + rival.PlayerNo + Environment.NewLine);
            foreach (var player in rival.PlayersNames)
            {
                txtMoves2.AppendText("Player Name :" + player + Environment.NewLine);
            }

            lblTurn.Text = yourTurn ? "Yes" : "No";

            if (yourTurn)
            {
                lblStatus.Text = "";
                txtFrom.Enabled = true;
                txtTo.Enabled = true;
                btnMove.Enabled = true;
                btnWin.Enabled = true;
            }
            else
            {
                c.WaitingMyTurn(Int32.Parse(txtGameCode.Text));
            }
        }
        private void UpdateBoard(int moveFrom, int moveTo)
        {
            txtMoves2.AppendText("Move From :" + moveFrom + " , Move To :" + moveTo + Environment.NewLine);
            lblTurn.Text = "Yes";
            lblStatus.Text = "";
            txtFrom.Enabled = true;
            txtTo.Enabled = true;
            btnMove.Enabled = true;
            btnWin.Enabled = true;
        }
        private void UpdateStausLabel(string msg)
        {
            lblStatus.Text = msg;

            if (msg == "You Lost")
            {
                lblTurn.Text = "";
                txtFrom.Clear();
                txtTo.Clear();
                txtGameCode.Clear();
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
                btnMove.Enabled = false;
                btnWin.Enabled = false;
            }
        }
        private void btnMove_Click(object sender, EventArgs e)
        {
            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                c.Move(Int32.Parse(txtGameCode.Text), Int32.Parse(txtFrom.Text), Int32.Parse(txtTo.Text), false);
                txtMoves1.AppendText("Move From :" + Int32.Parse(txtFrom.Text) + " , Move To :" + Int32.Parse(txtTo.Text) + Environment.NewLine);
                lblTurn.Text = "No";
                txtFrom.Clear();
                txtTo.Clear();
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
                btnMove.Enabled = false;
                btnWin.Enabled = false;
            }
        }
        private void btnWin_Click(object sender, EventArgs e)
        {
            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                c.Move(Int32.Parse(txtGameCode.Text), Int32.Parse(txtFrom.Text), Int32.Parse(txtTo.Text), true);
                lblTurn.Text = "";
                lblStatus.Text = "You Win";
                txtFrom.Clear();
                txtTo.Clear();
                txtGameCode.Clear();
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
                btnMove.Enabled = false;
                btnWin.Enabled = false;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //c.Close();
        }
    }

    public class CallBack : IGameServiceCallback
    {
        private static Action<string> _statusTextChange = null;
        private static Action<GroupData, bool> _initBoard = null;
        private static Action<int, int> _updateBoard = null;

        public static void SetDelegate(Action<string> nAction)
        {
            _statusTextChange = nAction;
        }
        public static void SetDelegate(Action<GroupData, bool> nAction)
        {
            _initBoard = nAction;
        }
        public static void SetDelegate(Action<int, int> nAction)
        {
            _updateBoard = nAction;
        }
        public void InitBoard(GroupData rival, bool yourTurn)
        {
            _initBoard(rival, yourTurn);
        }

        public void UpdateBoard(int moveFrom, int moveTo)
        {
            _updateBoard(moveFrom, moveTo);
        }

        public void Waiting()
        {
            if (_statusTextChange != null)
                _statusTextChange("Waiting");
        }

        public void YouLost()
        {
            if (_statusTextChange != null)
                _statusTextChange("You Lost");
        }
    }
}
