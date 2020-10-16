using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NewWorkMgr : MonoBehaviourPunCallbacks
{
    public GameObject model1;
    private GameObject model2;
    private GameObject model3;

    private GameObject player;

    private void Awake()
    {
      
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    // Start is called before the first frame update
    // Use this for initialization
    void Start()
    {
        randomPrefab();
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    void randomPrefab()//임시 캐릭터 랜덤 
    {
        int playerModel = 1;
        //int playerModel = Random.Range(1, 3);
        GameObject empty;
        switch(playerModel)
        {
            case 1:
                empty = model1;
                break;
            case 2:
                empty = model2;
                break;
            case 3:
                empty = model3;
                break;
            default:
                Debug.Log("prefabError");
                empty = null;
                break;
        }
        player = empty;
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
        //GameObject mainCamera = GameObject.FindWithTag("MainCamera");
 

    }
    // Update is called once per frame

}
