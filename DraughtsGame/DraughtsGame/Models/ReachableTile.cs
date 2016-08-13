using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public class ReachableTile : Tile
    {
        public Draught draught;

        public ReachableTile(int row, int column, Draught draught) : base (row, column)
        {
            this.draught = draught;
        }
    }
}
