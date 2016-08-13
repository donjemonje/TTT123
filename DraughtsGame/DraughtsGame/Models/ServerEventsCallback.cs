using DraughtsGame.GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public class ServerEventsCallback : IGameServiceCallback
    {
        private Action<string> _statusTextChange = null;
        private Action<GroupData, bool> _initBoard = null;
        private Action<int, int> _updateBoard = null;

        public void SetDelegate(Action<string> nAction)
        {
            _statusTextChange = nAction;
        }
        public void SetDelegate(Action<GroupData, bool> nAction)
        {
            _initBoard = nAction;
        }
        public void SetDelegate(Action<int, int> nAction)
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

    