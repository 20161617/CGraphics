using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NewWorkMgr : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
    
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.CreateRoom("virtualWrld", new RoomOptions { MaxPlayers = 5 });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        StartCoroutine(this.CreatePlayer());
    }
    IEnumerator CreatePlayer()
    {
        PhotonNetwork.Instantiate("cube", Vector3.zero, Quaternion.identity);
        yield return null;
    }
    // Update is called once per frame

}
