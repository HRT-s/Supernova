using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Controller : MonoBehaviourPunCallbacks{
    [SerializeField] private GameObject redstar;
    [SerializeField] private GameObject bluestar;
    [SerializeField] private GameObject turnTextObj;
    private int turn;
    private int myTurn;
    private string playerKind;
    private TextMeshProUGUI turnText;
    private RaycastHit2D hit;

    private void Start(){
        turn = 0;
        myTurn = PhotonNetwork.LocalPlayer.ActorNumber-1;
        turnText = turnTextObj.GetComponent<TextMeshProUGUI>();
        playerKind = (turn==myTurn)?"あなた":"相手";
        turnText.text = $"Player{turn}({playerKind})のターン";
    }

    private void Update(){
        //myTurn = turn;
        if(turn == myTurn && Input.GetMouseButtonDown(0)){
            Vector3 clickPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(clickPos);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin,(Vector2)ray.direction);
            if(hit){
                GameObject hitObj = hit.transform.gameObject;
                PhotonView hitView = hitObj.GetComponent<PhotonView>();
                if(hitView.IsMine){
                    PhotonNetwork.Destroy(hitObj);
                }else{
                    hitView.RequestOwnership();
                }
            }else{
                clickPos.z = 10f;
                clickPos = Camera.main.ScreenToWorldPoint(clickPos);
                clickPos.x = Mathf.Round(clickPos.x);
                clickPos.y = Mathf.Round(clickPos.y);
                string prefab = (turn==0)?redstar.name:bluestar.name;
                //Instantiate(redstar,clickPos,Quaternion.identity);
                PhotonNetwork.Instantiate(prefab,clickPos,Quaternion.identity);
            }
            photonView.RPC("TurnChange",RpcTarget.All);
        }
    }

    [PunRPC]
    private void TurnChange(){
        turn = (turn+1) % 2;
        playerKind = (turn==myTurn)?"あなた":"相手";
        turnText.text = $"Player{turn}({playerKind})のターン";
    }
}
