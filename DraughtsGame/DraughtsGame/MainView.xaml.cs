using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DraughtsGame.Models;
using DraughtsGame.GameService;


//TODO:
// 1. set all constants to defines
// 2. redraw only to changed state.
// 3. first draw - should be created through a loop on draughts not on all tiles
// 4. remove 

namespace DraughtsGame
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window, GameEvents
    {
        private const int DRAUGHT_MARGIN = 10;
        private readonly Size TILE_SIZE = new Size(50, 50);

        private Brush blackBrush;
        private Brush whiteBrush;
        private Brush playerOneBrush;
        private Brush playerTwoBrush;
        private Rectangle[,] cells;
        private Ellipse[,] draughts;

        private Ellipse selectedDraught;
        private Coordinate selectedDraughtStartingCoordinate;

        private Game game;

        private GameServiceClient gameServiceClient;
        private ServerEventsCallback serverEventsCallback;
        private GroupData groupData;
        private int groupId;
        private GroupData rivalGroupData;

        public MainView(int groupId, GroupData groupData, GameServiceClient gameServiceClient, ServerEventsCallback serverEventsCallback)
        {
            game = new Game(this);
            this.groupId = groupId;
            this.gameServiceClient = gameServiceClient;
            this.serverEventsCallback = serverEventsCallback;
            this.groupData = groupData;

            InitializeComponent();
            UpdateGroupsDataForGroup(this.groupData);
            InitGradients();
            ReDraw();

            this.serverEventsCallback.SetDelegate(UpdateStausLabel);
            this.serverEventsCallback.SetDelegate(InitBoard);
            this.serverEventsCallback.SetDelegate(UpdateBoard);

            this.gameServiceClient.Status(groupId);
        }
        

        #region Drawing    
             
  
        private void UpdateGroupsDataForGroup(GroupData groupData)
        {

            Label groupName = (PlayerType)groupData.PlayerNo == PlayerType.PlayerOne ? GroupOneNameLabel : GroupTwoNameLabel;
            Label groupPlayers = (PlayerType)groupData.PlayerNo == PlayerType.PlayerOne ? GroupOnePlayersLabel : GroupTwoPlayersLabel;

            groupName.Content = "Group Name :" + groupData.GroupName;
            StringBuilder groupPlayersNames = new StringBuilder();

            foreach (var player in groupData.PlayersNames)
            {
                groupPlayersNames.Append("Player Name :" + player + Environment.NewLine);
            }

            groupPlayers.Content = groupPlayersNames;
        }

        public void UpdateCurrentGroupLabel()
        {
            if (game.currentPlayer != null)
            {
                currentGroupLabel.Content = "current group: \n" + game.currentPlayer.groupData.GroupName;
            }
            else
            {
                currentGroupLabel.Content = "waiting";
            }
        }

        private void ReDraw()
        {
            canvas.Children.Clear();
            DrawCells();
            DrawDraughts();
            UpdateCurrentGroupLabel();
        }

        private void InitGradients()
        {
            blackBrush = Brushes.Black;
            whiteBrush = Brushes.White;

            GradientStopCollection collection = new GradientStopCollection
                                                    {
                                                        new GradientStop(Colors.Blue, 1),
                                                        new GradientStop(Colors.DarkBlue, 0.6),
                                                        new GradientStop(Colors.AliceBlue, 0)
                                                    };
            playerOneBrush = new RadialGradientBrush(collection);

            collection = new GradientStopCollection
                             {
                                 new GradientStop(Colors.Red, 1),
                                 new GradientStop(Colors.DarkRed, 0.6),
                                 new GradientStop(Colors.OrangeRed, 0)
                             };
            playerTwoBrush = new RadialGradientBrush(collection);
        }

        private void UpdateView()
        {
            canvas.Children.Clear();
            DrawCells();
            DrawDraughts();
        }

        private void DrawCells()
        {
            cells = new Rectangle[8, 4];

            int y = 0;
            for (int i = 0; i < 8; i++)
            {
                int x = 0;
                for (int j = 0; j < 4; j++)
                {
                    cells[i, j] = new Rectangle
                    {
                        Width = TILE_SIZE.Width,
                        Height = TILE_SIZE.Height,
                        Fill = (i + j) % 2 != 0 ? blackBrush : whiteBrush
                    };
                    cells[i, j].MouseMove += DraughtsMouseMove;

                    Canvas.SetLeft(cells[i, j], x);
                    Canvas.SetTop(cells[i, j], y);
                    canvas.Children.Add(cells[i, j]);

                    x += (int)TILE_SIZE.Width;
                }

                y += (int)TILE_SIZE.Height;
            }
        }

        private void DrawDraughts()
        {
            Board board = game.board;
            draughts = new Ellipse[8, 4];
            int z = 1000;
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (game.board[i, j] is ReachableTile && ((ReachableTile)game.board[i, j]).draught != null)
                    {

                        ReachableTile reachableTile = (ReachableTile) game.board[i, j];
                        Draught draught = reachableTile.draught;
                        draughts[i, j] = new Ellipse()
                        {
                            Width = TILE_SIZE.Width - DRAUGHT_MARGIN,
                            Height = TILE_SIZE.Height - DRAUGHT_MARGIN
                        };
                        draughts[i, j].MouseLeftButtonDown += DraughtsMouseDown;
                        draughts[i, j].MouseMove += DraughtsMouseMove;
                        draughts[i, j].MouseUp += DraughtsMouseUp;

                        draughts[i, j].Fill = draught.associatedPlayer == PlayerType.PlayerOne ? playerOneBrush : playerTwoBrush;


                        int row = reachableTile.row;
                        int column = reachableTile.column;
                        Canvas.SetLeft(draughts[i, j], (column * TILE_SIZE.Width) + 5);
                        Canvas.SetTop(draughts[i, j], (row * TILE_SIZE.Height) + 5);
                        Canvas.SetZIndex(draughts[i, j], z);
                        canvas.Children.Add(draughts[i, j]);

                    }
                }
            }
        }

        #endregion 

        #region Helpers

        private Coordinate CoordinatesFromMousePosition(Point point)
        {
            Console.WriteLine("point to calculate position: " + point);
            Coordinate coordinate = new Coordinate((int)Math.Max(0, (point.Y / TILE_SIZE.Height)), (int)Math.Max(0, (point.X / TILE_SIZE.Width)));
            Console.WriteLine("calculated coordinate: " + coordinate);
            return coordinate;
        }

        #endregion 

        #region Mouse Events

        private void DraughtsMouseDown(object o, MouseButtonEventArgs args)
        {
            Ellipse tappedEllipse = (Ellipse)o;
            Coordinate coordinate = CoordinatesFromMousePosition(args.GetPosition(null));
            if (game.IsTileValidForMovmentAtCoordinates(coordinate))
            {
                selectedDraught = tappedEllipse;
                selectedDraughtStartingCoordinate = coordinate;
                Canvas.SetZIndex(selectedDraught, 10000);
            }
        }

        private void DraughtsMouseMove(object o, MouseEventArgs args)
        {
            if (selectedDraught != null && args.LeftButton == MouseButtonState.Pressed)
            {
                
                Point currentPoint = args.GetPosition(canvas);
                Canvas.SetLeft(selectedDraught, currentPoint.X - 15);
                Canvas.SetTop(selectedDraught, currentPoint.Y - 15);

                //TODO bonus add bacground for valid/invalid tiles
            }
        }

        private void DraughtsMouseUp(object o, MouseButtonEventArgs args)
        {
            if (selectedDraught != null && args.LeftButton == MouseButtonState.Released)
            {
                Coordinate nextCoordinate = CoordinatesFromMousePosition(args.GetPosition(null));
                Coordinate prevCoordinate = selectedDraughtStartingCoordinate;
                Canvas.SetZIndex(selectedDraught, 1000);
                selectedDraught = null;

                game.Step(prevCoordinate, nextCoordinate, true);
            }
        }

        #endregion

        #region Game Events

        public void stepEnded(bool validStep, int fromIndex, int toIndex, bool clientStep)
        {
            ReDraw();
            if (validStep && clientStep)
            {
                gameServiceClient.Move(groupId, fromIndex, toIndex, false);
            }
        }

        public void gameEndedWithWinnigPlayer(Player player, int fromIndex, int toIndex, bool clientStep)
        {
            ReDraw();
            MessageBox.Show(player.type.Description() + ", WON !!!");

            if (clientStep)
            {
                gameServiceClient.Move(groupId, fromIndex, toIndex, true);
            }
            
        }

        #endregion

        #region ServerEventsCallback

        private void InitBoard(GroupData rival, bool yourTurn)
        {
            this.rivalGroupData = rival;
            
            GroupData GroupOneData = (PlayerType)rivalGroupData.PlayerNo == PlayerType.PlayerOne ? rivalGroupData : groupData;
            GroupData GroupTwoData = ReferenceEquals(GroupOneData,groupData) ? rivalGroupData : groupData;
            game.SetupGameWith(GroupOneData, GroupTwoData, yourTurn ? groupData : rivalGroupData, groupData);

            UpdateCurrentGroupLabel();
            UpdateGroupsDataForGroup(this.rivalGroupData);


            if (!yourTurn)
            {
                gameServiceClient.WaitingMyTurn(groupId);
            }
        }
        private void UpdateBoard(int moveFrom, int moveTo)
        {
            Coordinate fromCoordinate = new Coordinate(moveFrom / Board.NUM_COLUMNS, moveFrom % Board.NUM_COLUMNS);
            Coordinate toCoordinate = new Coordinate(moveTo / Board.NUM_COLUMNS, moveTo % Board.NUM_COLUMNS);
            game.Step(fromCoordinate, toCoordinate, false);
        }
        private void UpdateStausLabel(string msg)
        {
            //lblStatus.Text = msg;

            ////if (msg == "You Lost")
            ////{
            //    lblTurn.Text = "";
            //    txtFrom.Clear();
            //    txtTo.Clear();
            //    txtGameCode.Clear();
            //    txtFrom.Enabled = false;
            //    txtTo.Enabled = false;
            //    btnMove.Enabled = false;
            //    btnWin.Enabled = false;
            //}
        }

        #endregion


    }
}