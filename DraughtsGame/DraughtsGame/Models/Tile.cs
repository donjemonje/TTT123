using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public class Tile
    {
        public readonly int row;
        public readonly int column;

        public Tile(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
