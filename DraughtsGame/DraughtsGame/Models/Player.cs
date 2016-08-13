using DraughtsGame.GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public class Player
    {
        public readonly PlayerType type;
        public int draughtsCount;
        public GroupData groupData;
        public bool isClient;

        public Player(PlayerType type, int draughtsCount, GroupData groupData, bool isClient)
        {
            this.type = type;
            this.draughtsCount = draughtsCount;
            this.groupData = groupData;
            this.isClient = isClient;
        }
    }
}
