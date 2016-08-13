using DraughtsGame.GameService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Models
{
    public interface GameEvents
    {
        void stepEnded(bool validStep, int fromIndex, int toIndex, bool clientStep);
        void gameEndedWithWinnigPlayer(Player player, int fromIndex, int toIndex, bool clientStep);
    }

    public class Game
    {
        private readonly GameEvents listener;

        public readonly Board board;
        public int Id { get; internal set; }

        public Player playerOne { get; internal set; }
        public Player playerTwo { get; internal set; }
        private bool isEndOfGame;
        public Player currentPlayer { get; private set; }

        private Player otherPlayer
        {
            get
            {
                return currentPlayer == playerOne ? playerTwo : playerOne; ;
            }
        }

        public Game(GameEvents listener)
        {
            board = new Board();
            this.listener = listener;
            Console.WriteLine("new game created");
        }

        public void SetupGameWith(GroupData groupOneData, GroupData groupTwoData, GroupData startingGroup, GroupData clientGroupData)
        {
            int playerStartingDraughtsCount = board.PlayerStartingDraughtsCount();
            playerOne = new Player(PlayerType.PlayerOne, playerStartingDraughtsCount, groupOneData, ReferenceEquals(clientGroupData, groupOneData));
            playerTwo = new Player(PlayerType.PlayerTwo, playerStartingDraughtsCount, groupTwoData, ReferenceEquals(clientGroupData, groupTwoData));
            currentPlayer = ReferenceEquals(startingGroup, groupOneData) ? playerOne : playerTwo;
            Console.WriteLine("groups created, firstPlayer is " + currentPlayer.type.Description());
        }

        public bool IsTileValidForMovmentAtCoordinates(Coordinate coordinate)
        {
            bool tileValid = false;

            if (!isEndOfGame && currentPlayer.isClient)
            {
                Tile tile = board[coordinate.row, coordinate.column];
                if (tile is ReachableTile)
                {
                    ReachableTile reachableTile = (ReachableTile)tile;
                    if (reachableTile.draught != null)
                    {
                        tileValid = reachableTile.draught.associatedPlayer == currentPlayer.type;
                    }
                }
                Console.WriteLine("tileValid " + tileValid);
            }
            
            return tileValid;
        }

        public void Step(Coordinate preCord, Coordinate nextCord, bool clientStep)
        {
            ReachableTile prevReachableTile = (ReachableTile) board[preCord.row, preCord.column];
            Draught draught = prevReachableTile.draught;
            Tile nextTile = board[nextCord.row, nextCord.column];

            bool validStep = false;
            int fromIndex = 0;
            int toIndex = 0;
            Player winningPlayer = null;

            if (nextTile is ReachableTile && draught != null)
            {
                ReachableTile nextReachableTile = (ReachableTile)nextTile;
                if (draught.associatedPlayer == PlayerType.PlayerOne && nextReachableTile.row > prevReachableTile.row ||
                    draught.associatedPlayer == PlayerType.PlayerTwo && nextReachableTile.row < prevReachableTile.row)
                {
                    // only two steps are valid: 
                    // 1. one diagonal step 
                    // 2. jump and eat step: two diagonal step over other player draught

                    if (Math.Abs(nextReachableTile.row - prevReachableTile.row) == 1)
                    {// one diagonal step
                        validStep = true;
                    }
                    else if (Math.Abs(nextReachableTile.row - prevReachableTile.row) == 2 && Math.Abs(nextReachableTile.column - prevReachableTile.column) == 2)
                    {// jump step
                        
                        int jumpedTileRow = draught.associatedPlayer == PlayerType.PlayerOne ? prevReachableTile.row + 1 : prevReachableTile.row - 1;
                        int jumpedTileColmun = nextReachableTile.column > prevReachableTile.column ? prevReachableTile.column + 1 : prevReachableTile.column - 1;
                        ReachableTile jumpedTile = (ReachableTile)board[jumpedTileRow, jumpedTileColmun];
                        if (jumpedTile.draught != null)
                        {// jump and eat step

                            validStep = true;
                            jumpedTile.draught = null;
                            otherPlayer.draughtsCount--;
                        }
                    }

                    if (validStep)
                    {
                        fromIndex = board.getTileBoardIndex(prevReachableTile);
                        toIndex = board.getTileBoardIndex(nextReachableTile);

                        prevReachableTile.draught = null;
                        nextReachableTile.draught = draught;

                        EvaluateEndOfGame(nextReachableTile);

                        winningPlayer = isEndOfGame ? currentPlayer : null;

                        currentPlayer = currentPlayer == playerOne ? playerTwo : playerOne;

                        if (winningPlayer != null)
                        {
                            listener.gameEndedWithWinnigPlayer(winningPlayer, fromIndex, toIndex, clientStep);
                        }
                        

                    }
                }
            }

            if(winningPlayer == null)
            {
                listener.stepEnded(validStep, fromIndex, toIndex, clientStep);
            }
            
        }

        public void EvaluateEndOfGame(ReachableTile newestTile)
        {

            // the game is over in 3 situations:
            // 1. player reaches the other player statring line
            // 2. player losses all his draughts
            // 3. player has no valid moves

        
            if (newestTile.draught.associatedPlayer == PlayerType.PlayerOne && newestTile.row == Board.NUM_ROWS - 1 ||
               newestTile.draught.associatedPlayer == PlayerType.PlayerTwo && newestTile.row == 0)
            {//situation 1
                isEndOfGame = true;
            }
            else if(otherPlayer.draughtsCount == 0)
            {// situation 2
                isEndOfGame = true;
            }
            else
            {
                //TODO: need to check situation 3
            }
           
        }
    }
}
