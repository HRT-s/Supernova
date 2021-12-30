using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Matching : MonoBehaviourPunCallbacks{
    [SerializeField] private GameObject selectCanvas;
    [SerializeField] private GameObject matchCanvas;
    private TextAnim textAnim;
    private bool isConnected;

    private void Start(){
        matchCanvas.SetActive(false);
        textAnim = matchCanvas.GetComponentInChildren<TextAnim>();
        isConnected = false;
    }

    private void Update(){
        if(isConnected && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers){
            PhotonNetwork.CurrentRoom.IsOpen = false;
            textAnim.IsMatching = true;
            Invoke("MatchFound",2.0f);
        }
    }

    private void MatchFound(){
        SceneManager.LoadSceneAsync("Main");
    }

    public void Connect(){
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Cancel(){
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause){
        selectCanvas.SetActive(true);
        matchCanvas.SetActive(false);
        isConnected = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message){
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null,roomOptions);
    }

    public override void OnJoinedRoom(){
        selectCanvas.SetActive(false);
        matchCanvas.SetActive(true);
        isConnected = true;
    }
}
