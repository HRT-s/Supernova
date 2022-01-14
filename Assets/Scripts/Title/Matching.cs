using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

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
            matchCanvas.GetComponentInChildren<Button>().interactable = false;
            Invoke("MatchFound",2.0f);
        }
    }

    private void MatchFound(){
        SceneManager.LoadSceneAsync("Main");
    }

    public void ConnectOnline(){
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectLocal(){
        PhotonNetwork.OfflineMode = true;
        MatchFound();
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
