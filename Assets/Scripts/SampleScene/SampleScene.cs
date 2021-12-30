using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SampleScene : MonoBehaviourPunCallbacks
{
    void Start(){
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinOrCreateRoom("Room",new RoomOptions(),TypedLobby.Default);
    }

    public override void OnJoinedRoom(){
        var position = new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f));
        PhotonNetwork.Instantiate("Avatar",position,Quaternion.identity);
    }
}
