using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NewWorkMgr : MonoBehaviourPunCallbacks
{
    public GameObject player;
    
    private void Awake()
    {
    
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    // Start is called before the first frame update
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    //포톤 서버에 접속
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom("virtualWrld", new RoomOptions { MaxPlayers = 10 });
    }
    //방에 입장한 후에 플레이어 생성
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        CreateCamera();
        StartCoroutine(this.CreatePlayer());
    }
    IEnumerator CreatePlayer()
    {
        PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);
        yield return null;
        
    }
    public void CreateCamera()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
 

    }
    // Update is called once per frame

}
