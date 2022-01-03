using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks{
    [SerializeField] private GameObject errorPanel;
    private static int turn;
    public static int Turn{get{return turn;} set{turn = value;}}
    private bool isConnect;

    private void Start(){
        errorPanel.SetActive(false);
        isConnect = true;
    }

    private void Update(){
        if(!isConnect) return;
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2){
            errorPanel.SetActive(true);
        }
    }

    private void GameEnd(){
        //Implement later...
    }

    public void OnEnterButtonClicked(){
        isConnect = false;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause){
        SceneManager.LoadSceneAsync("Title");
    }
}
