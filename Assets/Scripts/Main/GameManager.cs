using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks{
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject explosionPanel;
    [SerializeField] private Color winnerColor;
    [SerializeField] private Color loserColor;
    private static bool isFinish;
    public static bool IsFinish{get{return isFinish;}}
    private static int turn;
    public static int Turn{get{return turn;} set{turn = value;}}
    private bool isConnect;

    private void Start(){
        errorPanel.SetActive(false);
        endPanel.SetActive(false);
        explosionPanel.SetActive(false);
        turn = 0;
        isFinish = false;
        isConnect = true;
    }

    private void Update(){
        if(!isConnect || isFinish) return;
        if(!PhotonNetwork.OfflineMode && PhotonNetwork.CurrentRoom.PlayerCount < 2){
            errorPanel.SetActive(true);
        }
    }

    public void GameEndCaller(){
        photonView.RPC("SupernovaExplosion",RpcTarget.All);
    }

    [PunRPC]
    private void SupernovaExplosion(){
        explosionPanel.SetActive(true);
        Invoke("GameEnd",3.0f);
    }

    private void GameEnd(){
        explosionPanel.SetActive(false);
        endPanel.SetActive(true);
        TextMeshProUGUI tmpro = endPanel.GetComponentInChildren<TextMeshProUGUI>();
        if(PhotonNetwork.OfflineMode){
            string winner = (turn==0)?"先手":"後手";
            tmpro.text = $"{winner} WIN!!";
            tmpro.color = winnerColor;
            Text btnText = endPanel.GetComponentInChildren<Text>();
            btnText.text = "タイトルへ";
        }else{
            if(turn == PhotonNetwork.LocalPlayer.ActorNumber-1){
                tmpro.text = "YOU WIN!!";
                tmpro.color = winnerColor;
            }else{
                tmpro.text = "YOU LOSE...";
                tmpro.color = loserColor;
            }
        }
        isFinish = true;
    }

    public void OnEnterButtonClicked(){
        isConnect = false;
        if(PhotonNetwork.OfflineMode){
            PhotonNetwork.OfflineMode = false;
            SceneManager.LoadSceneAsync("Title");
        }
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause){
        SceneManager.LoadSceneAsync("Title");
    }
}
