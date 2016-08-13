using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GameService" in code, svc and config file together.
[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
public class GameService : IGameService
{
    const int maxPlayers = 2;
    public static int playerTurn = 0;
    public static bool turnOfSelected = false;
    public static bool youLost = false;
   
    public GroupData LogIn(int gameCode)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var group = db.Groups.Where(x => x.Id == gameCode).FirstOrDefault();
                var game = db.Games.Where(x => x.Id == group.GameId).FirstOrDefault();

                if (game.InRoomCounter == null)
                    game.InRoomCounter = 1;
                else
                    game.InRoomCounter++;

                db.SubmitChanges();
                return new GroupData { GroupName = group.Name, PlayersNames = group.Players.Select(x => x.Name).ToList(), PlayerNo = group.PlayerNo };
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }

    public void Move(int gameCode, int moveFrom, int moveTo, bool win)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var group = db.Groups.Where(x => x.Id == gameCode).FirstOrDefault();
                ICallBack c = OperationContext.Current.GetCallbackChannel<ICallBack>();

                var move = new GameMove { GroupId = gameCode, MoveFrom = moveFrom, MoveTo = moveTo, CreateDate = DateTime.Now };

                db.GameMoves.InsertOnSubmit(move);
                db.SubmitChanges();

                playerTurn = db.Groups.Where(x => x.GameId == group.GameId && x.Id != gameCode).Select(x => x.PlayerNo).FirstOrDefault();

                if (win)
                {
                    youLost = true;
                    turnOfSelected = false;
                    var game = db.Games.Where(x => x.Id == group.GameId).FirstOrDefault();
                    game.WinnerGroupName = group.Name;
                    db.SubmitChanges();
                }
                else
                {
                    while (!youLost && playerTurn != group.PlayerNo ) { c.Waiting(); }

                    if (youLost)
                    {
                        playerTurn = 0;
                        c.YouLost();
                    }   
                    else
                    {
                        var rivalMove = db.GameMoves.OrderByDescending(p => p.CreateDate).First();
                        c.UpdateBoard(rivalMove.MoveFrom, rivalMove.MoveTo);
                    }
                }
            }
        }
        catch(ObjectDisposedException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public void Status(int gameCode)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var game = db.Groups.Where(x => x.Id == gameCode).Select(x => x.Game).FirstOrDefault();
                var playerNo = db.Groups.Where(x => x.Id == gameCode).Select(x => x.PlayerNo).FirstOrDefault();
                ICallBack c = OperationContext.Current.GetCallbackChannel<ICallBack>();

                while (game.InRoomCounter < maxPlayers)
                {
                    c.Waiting();

                    if (!turnOfSelected)
                        initTurnOf();

                    db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, game);
                }

                var rival = db.Groups
                              .Where(x => x.GameId == game.Id && x.Id != gameCode)
                              .Select(x => new GroupData { GroupName = x.Name, PlayersNames = x.Players.Select(y => y.Name).ToList(), PlayerNo = x.PlayerNo })
                              .FirstOrDefault();

                c.InitBoard(rival, playerNo == playerTurn ? true : false);
            }
        }
        catch (ObjectDisposedException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    private void initTurnOf()
    {
        turnOfSelected = true;
        Random rnd = new Random();
        playerTurn = rnd.Next(1, maxPlayers);
    }

    public void WaitingMyTurn(int gameCode)
    {
        try
        {
            ICallBack c = OperationContext.Current.GetCallbackChannel<ICallBack>();

            using (var db = new GameDataContext())
            {
                var group = db.Groups.Where(x => x.Id == gameCode).FirstOrDefault();

                while (playerTurn != group.PlayerNo) { c.Waiting(); }

                var rivalMove = db.GameMoves.OrderByDescending(p=>p.CreateDate).First();
                c.UpdateBoard(rivalMove.MoveFrom, rivalMove.MoveTo);
            }
        }
        catch (ObjectDisposedException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
}
