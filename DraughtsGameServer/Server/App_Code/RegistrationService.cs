using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegistrationService" in code, svc and config file together.

public class RegistrationService : IRegistrationService
{
    public void AddPlayer(string groupId, string playerName, string playerIDCard)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var player = new Player {
                    GroupId = Int32.Parse(groupId),
                    Name = playerName,
                    IDCardNumber = playerIDCard,
                    CreateDate = DateTime.Now
                };

                db.Players.InsertOnSubmit(player);
                db.SubmitChanges();
            }
        }
        catch(Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public string CreateGroup(string name,string gameId)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var playerNo = db.Groups.Where(x => x.GameId == Int32.Parse(gameId)).Count() + 1;
                var group = new Group { Name = name,GameId = Int32.Parse(gameId),PlayerNo = playerNo, CreateDate = DateTime.Now };
                db.Groups.InsertOnSubmit(group);
                db.SubmitChanges();

                return group.Id.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public void DeleteGame(string id)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var game = db.Games.Where(x => x.Id == Int32.Parse(id)).FirstOrDefault();
                var groupsId = db.Groups.Where(x=>x.GameId == game.Id).Select(x => x.Id).ToList();
                var gameMoves = db.GameMoves.Where(x => groupsId.Contains(x.GroupId)).ToList();
                var players = db.Players.Where(x => groupsId.Contains(x.GroupId)).ToList();
                var groups = db.Groups.Where(x => groupsId.Contains(x.Id)).ToList();

                db.Players.DeleteAllOnSubmit(players);
                db.GameMoves.DeleteAllOnSubmit(gameMoves);
                db.Groups.DeleteAllOnSubmit(groups);
                db.Games.DeleteOnSubmit(game);

                db.SubmitChanges();
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public void DeletePlayer(string playerIdCard)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var player = db.Players.Where(x => x.IDCardNumber == playerIdCard).ToList();
                db.Players.DeleteAllOnSubmit(player);
                db.SubmitChanges();
            }
        }
        catch(Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public List<GameInfo> GetGameInfo()
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var games = db.Games.Select(x => new GameInfo { GameId = x.Id, GroupsName = String.Join(" vs ",x.Groups.Select(y=>y.Name).ToList()) , WinnerGroup = x.WinnerGroupName, CreatedDate = x.CreateDate }).ToList();
                return games;
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public List<GameInfo> GetGameInfoByPlayerId(string playerIdCardNo)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var groupsId = db.Players.Where(x => x.IDCardNumber == playerIdCardNo).Select(x => x.GroupId);
                var games = db.Groups
                    .Where(x=> groupsId.Contains(x.Id))
                    .Select(x=>x.Game)
                    .Select(x => new GameInfo { GameId = x.Id, GroupsName = String.Join(" vs ", x.Groups.Select(y => y.Name).ToList()), WinnerGroup = x.WinnerGroupName, CreatedDate = x.CreateDate }).ToList();
                return games;
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public List<PlayerInfo> GetPlayerInfoByGameId(string gameId)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var id = Int32.Parse(gameId);
                var groupsId = db.Groups.Where(x => x.GameId == id).Select(x => x.Id);
                var players = db.Players.Where(x=>groupsId.Contains(x.GroupId)).Select(x => new PlayerInfo { PlayerId = x.Id, IdCardNo = x.IDCardNumber, PlayerName = x.Name, GroupName = x.Group.Name, CreatedDate = x.CreateDate }).ToList();
                return players;
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }

    public List<PlayerInfo> GetPlayersInfo()
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var players = db.Players.Select(x => new PlayerInfo { PlayerId = x.Id, IdCardNo = x.IDCardNumber, PlayerName = x.Name, GroupName = x.Group.Name, CreatedDate = x.CreateDate }).ToList();
                return players;
            }
        }
        catch(Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }

    public List<PlayerInfo> GetPlayerStatistics()
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var players = db.Players.GroupBy(x=> new { x.Name ,x.IDCardNumber }).Select(x => new PlayerInfo { IdCardNo = x.Key.IDCardNumber, PlayerName = x.Key.Name,NoOfGames = x.Count() }).ToList();
                return players;
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
    public string RegistrationGame()
    {
        try
        {
            using (var db = new GameDataContext())
            {
                Game game = null;

                if (db.Games.Any(x => !x.IsFull))
                {
                    game = db.Games.Where(x => !x.IsFull).FirstOrDefault();
                    game.IsFull = true;
                }
                else // New Game
                {
                    game = new Game
                    {
                        IsFull = false,
                        CreateDate = DateTime.Now
                    };
                    db.Games.InsertOnSubmit(game);
                }

                db.SubmitChanges();
                return game.Id.ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
    public void UpdateGroup(string groupId, string name)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var group = db.Groups.Where(x => x.Id == Int32.Parse(groupId)).FirstOrDefault();
                group.Name = name;
                db.SubmitChanges();
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }

    public void UpdatePalyer(string id, string playerName, string playerIdCard,string groupName)
    {
        try
        {
            using (var db = new GameDataContext())
            {
                var player = db.Players.Where(x => x.Id == Int32.Parse(id)).FirstOrDefault();
                player.Name = playerName;
                player.IDCardNumber = playerIdCard;
                db.SubmitChanges();

                UpdateGroup(player.Group.Id.ToString(), groupName);
            }
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message);
        }
    }
}
