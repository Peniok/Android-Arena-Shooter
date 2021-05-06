using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject Player;
    public GameObject[] Spawnpoints;
    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0, 6);
        PhotonNetwork.Instantiate(Player.name,Spawnpoints[i].transform.position,Quaternion.identity);
    }
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()//Вызывается когда вышел из комнаты
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player (0) entered room", newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player (0) left room", otherPlayer.NickName);
    }
}
