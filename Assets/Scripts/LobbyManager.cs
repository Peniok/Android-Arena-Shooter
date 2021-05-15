using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text LogText;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = "PlayerPrefab" + Random.Range(1000, 9999);
        Log("PlayerPrefab`s name is set to " + PhotonNetwork.NickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Log(string message)
    {
        Debug.Log(message);
        LogText.text += "\n";
        LogText.text += message;

    }
    public override void OnConnectedToMaster()
    {
        Log("Connected to Master");
    }
    // Update is called once per frame
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Log("Joined to Room");

        PhotonNetwork.LoadLevel("Game");
    }
}
