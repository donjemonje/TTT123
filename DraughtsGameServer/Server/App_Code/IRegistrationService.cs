using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRegistrationService" in both code and config file together.
[ServiceContract]
public interface IRegistrationService
{
    [OperationContract]
    [WebGet(UriTemplate = "CreateGroup/{name}/{gameId}")]
    string CreateGroup(string name,string gameId);
    [OperationContract]
    [WebInvoke(UriTemplate = "UpdateGroup/{name}")]
    void UpdateGroup(string groupId, string name);
    [OperationContract]
    [WebInvoke(UriTemplate = "AddPlayer/{groupId}/{playerName}/{playerIDCard}")]
    void AddPlayer(string groupId, string playerName, string playerIdCard);
    [OperationContract]
    [WebInvoke(UriTemplate = "UpdatePalyer/{id}/{playerName}/{playerIdCard}/{groupName}")]
    void UpdatePalyer(string id, string playerName, string playerIdCard,string groupName);
    [OperationContract]
    [WebInvoke(UriTemplate = "DeletePlayer/{id}")]
    void DeletePlayer(string id);
    [OperationContract]
    [WebGet(UriTemplate = "RegistrationGame")]
    string RegistrationGame();
    [OperationContract]
    [WebGet(UriTemplate = "DeleteGame/{id}")]
    void DeleteGame(string id);
    [OperationContract]
    [WebGet]
    List<PlayerInfo> GetPlayersInfo();
    [OperationContract]
    [WebGet]
    List<GameInfo> GetGameInfo();
    [OperationContract]
    [WebGet(UriTemplate = "GetGameInfoByPlayerId/{playerIdCardNo}")]
    List<GameInfo> GetGameInfoByPlayerId(string playerIdCardNo);
    [OperationContract]
    [WebGet(UriTemplate = "GetPlayerInfoByGameId/{gameId}")]
    List<PlayerInfo> GetPlayerInfoByGameId(string gameId);
    [OperationContract]
    [WebGet]
    List<PlayerInfo> GetPlayerStatistics();
}


[DataContract(Name = "PlayerInfo", Namespace ="")]
public class PlayerInfo
{
    [DataMember]
    public int PlayerId { get; set; }
    [DataMember]
    public string IdCardNo { get; set; }
    [DataMember]
    public string PlayerName { get; set; }
    [DataMember]
    public string GroupName { get; set; }
    [DataMember]
    public DateTime CreatedDate { get; set; }
    [DataMember]
    public int NoOfGames { get; set; }
}

[DataContract(Name = "GameInfo", Namespace = "")]
public class GameInfo
{
    [DataMember]
    public int GameId { get; set; }
    [DataMember]
    public string GroupsName { get; set; }
    [DataMember]
    public string WinnerGroup { get; set; }
    [DataMember]
    public DateTime CreatedDate { get; set; }
}