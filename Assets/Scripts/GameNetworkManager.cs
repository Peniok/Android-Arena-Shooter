using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab, Player;
    public GameObject[] Spawnpoints;
    void Start()
    {
        int i = Random.Range(0, Spawnpoints.Length);
        Player = PhotonNetwork.Instantiate(PlayerPrefab.name, Spawnpoints[i].transform.position, Quaternion.identity, 0, new object[] { PlayerPrefab.GetComponent<PlayerController>().photonView.ViewID });
    }
    public void SpawningPlayer()
    {
        int i = Random.Range(0, Spawnpoints.Length);
        PhotonNetwork.Destroy(Player);
        Player = PhotonNetwork.Instantiate(PlayerPrefab.name, Spawnpoints[i].transform.position, Quaternion.identity, 0, new object[] { Player.GetComponent<PlayerController>().photonView.ViewID });

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
        Debug.LogFormat("PlayerPrefab (0) entered room", newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("PlayerPrefab (0) left room", otherPlayer.NickName);
    }
}
