using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class GamePlayer : MonoBehaviourPunCallbacks{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private GameObject targetPref;
    [SerializeField] private Board board;
    [SerializeField] private Button putButton;
    [SerializeField] private Button rmButton;
    [SerializeField] private Button burstButton;
    private const int MAX_ACTIONTIME = 2;
    private int myTurn;
    private int actionTime;
    private RaycastHit2D hit;
    private Vector2 clickedPos;
    private GameObject hitObj;
    private GameObject targetObj;
    private int rmCT,burstCT;

    private void Start(){
        myTurn = PhotonNetwork.LocalPlayer.ActorNumber-1;
        actionTime = MAX_ACTIONTIME;
        TurnTextChange();
        InitButton();
        rmCT = 0;
        burstCT = 0;
    }

    private void Update() {
        if(GameManager.IsFinish) return;
        if(actionTime == 0){
            photonView.RPC("TurnChange",RpcTarget.All);
            actionTime = MAX_ACTIONTIME;
            rmCT--;
            burstCT--;
        }
        if(IsMyTurn() && Input.GetMouseButtonDown(0)){
            MouseClicked();
        }
    }

    private void InitButton(){
        putButton.interactable = false;
        rmButton.interactable = false;
        burstButton.interactable = false;
    }

    private bool IsMyTurn(){
        return myTurn==GameManager.Turn;
    }

    private void TurnTextChange(){
        string playerKind;
        if(PhotonNetwork.OfflineMode){
            playerKind = (myTurn==0)?"先手":"後手";
        }else{
            playerKind = (IsMyTurn())?"あなた":"相手";
        }
        turnText.text = $"Player{GameManager.Turn}({playerKind})のターン";
    }

    [PunRPC]
    private void TurnChange(){
        GameManager.Turn = (GameManager.Turn+1) % 2;
        if(PhotonNetwork.OfflineMode) myTurn = GameManager.Turn;
        TurnTextChange();
    }
    
    private void MouseClicked(){
        Vector3 clickPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(clickPos);
        hit = Physics2D.Raycast((Vector2)ray.origin,(Vector2)ray.direction);
        clickPos.z = 10f;
        clickPos = Camera.main.ScreenToWorldPoint(clickPos);
        Vector2 clickPos2D = new Vector2(Mathf.Round(clickPos.x),Mathf.Round(clickPos.y));
        if(0 <= clickPos2D.x && clickPos2D.x <= 9 && 0 <= clickPos2D.y && clickPos2D.y <= 9){
            if(targetObj != null) Destroy(targetObj);
            InitButton();
            clickedPos = clickPos2D;
            targetObj = Instantiate(targetPref,clickPos2D,Quaternion.identity);
            if(hit){
                hitObj = hit.transform.gameObject;
                if(rmCT <= 0){
                    rmButton.interactable = true;
                }
                if(hitObj.GetComponent<Star>().ID == myTurn){
                    if(burstCT <= 0){
                        burstButton.interactable = true;
                    }
                }
            }else{
                putButton.interactable = true;
            }
        }
    }

    private void ButtonClicked(){
        InitButton();
        Destroy(targetObj);
        actionTime--;
    }

    public void OnPutAction(){
        ButtonClicked();
        board.OnPut(clickedPos);
    }

    public void OnRmAction(){
        ButtonClicked();
        board.OnRemove(hitObj);
        rmCT = 3 - myTurn;
    }

    public void OnBurstAction(){
        ButtonClicked();
        board.OnBurst(clickedPos);
        burstCT = 3 - myTurn;
    }
}
