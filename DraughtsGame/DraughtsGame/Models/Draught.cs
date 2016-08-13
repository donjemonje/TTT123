using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{


    public class Draught
    {
        public readonly PlayerType associatedPlayer;


        public Draught(PlayerType associatedPlayer)
        {
            this.associatedPlayer = associatedPlayer;
        }
    }
}
