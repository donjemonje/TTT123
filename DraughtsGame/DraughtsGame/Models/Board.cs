using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public class Board
    {
        public const int NUM_ROWS = 8;
        public const int NUM_COLUMNS = 4;
        public const int STARTING_ROWS_COUNT = 2;

        private readonly Tile[,] tiles = new Tile[NUM_ROWS, NUM_COLUMNS];
        
        public Board()
        {

            for(int i=0; i< NUM_ROWS; i++)
            {
                for (int j = 0; j < NUM_COLUMNS; j++)
                {
                    bool isReachableTile = i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0;
                    bool isPlayerOneTile = i < STARTING_ROWS_COUNT;
                    bool isPlayerTwoTile = i >= NUM_ROWS - STARTING_ROWS_COUNT;
                    Draught draught = null;
                    if (isReachableTile && (isPlayerOneTile || isPlayerTwoTile))
                    {
                        draught = new Draught(isPlayerOneTile ? PlayerType.PlayerOne : PlayerType.PlayerTwo);
                    }
                    Tile tile = isReachableTile ? new ReachableTile(i, j, draught) : new Tile(i, j);
                    Console.WriteLine("Tile created rechable: " + (tile is ReachableTile ? "YES" : "NO") + ", draught: " + draught + "  row: "+ tile.row + ", column: "+ tile.column);
                    this.tiles[i, j] = tile;
                }
            }
        }

        public Tile this[int row, int column]
        {
            get {
                if(row < NUM_ROWS && column < NUM_COLUMNS)
                    return this.tiles[row, column];
                return null;
            }
            
        }

        public int PlayerStartingDraughtsCount()
        {
            return (NUM_COLUMNS / 2) * STARTING_ROWS_COUNT;

        }

        //helpers
        public int getTileBoardIndex(Tile tile)
        {
            int boardIndex = 0;

            boardIndex = (tile.row * NUM_COLUMNS) + tile.column;

            return boardIndex;
        }
    }
}
