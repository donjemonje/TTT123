using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public struct Coordinate
    {
        public int row;
        public int column;

        public Coordinate(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public override string ToString()
        {
            return "row: "+ row +", column: " + column;
        }
    }
}
