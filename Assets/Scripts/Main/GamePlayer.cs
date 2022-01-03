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
    private string playerKind;
    private RaycastHit2D hit;
    private Vector2 clickedPos;
    private GameObject hitObj;
    private GameObject targetObj;

    private void Start(){
        myTurn = PhotonNetwork.LocalPlayer.ActorNumber-1;
        actionTime = MAX_ACTIONTIME;
        playerKind = (IsMyTurn())?"あなた":"相手";
        turnText.text = $"Player{GameManager.Turn}({playerKind})のターン";
        InitButton();
    }

    private void Update() {
        if(actionTime == 0){
            photonView.RPC("TurnChange",RpcTarget.All);
            actionTime = MAX_ACTIONTIME;
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

    [PunRPC]
    private void TurnChange(){
        GameManager.Turn = (GameManager.Turn+1) % 2;
        playerKind = (GameManager.Turn==myTurn)?"あなた":"相手";
        turnText.text = $"Player{GameManager.Turn}({playerKind})のターン";
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
                rmButton.interactable = true;
                if(hitObj.GetPhotonView().IsMine){
                    burstButton.interactable = true;
                }
            }else{
                putButton.interactable = true;
            }
        }
    }

    private void ButtonClicked(){
        InitButton();
        Destroy(targetObj);
        //targetObj = null;
        actionTime--;
    }

    public void OnPutAction(){
        ButtonClicked();
        board.OnPut(clickedPos);
    }

    public void OnRmAction(){
        ButtonClicked();
        board.OnRemove(hitObj,clickedPos);
    }

    public void OnBurstAction(){
        ButtonClicked();
        board.OnBurst();
    }
}
