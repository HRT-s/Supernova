using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Board : MonoBehaviourPunCallbacks{
    [SerializeField] private GameObject redstar;
    [SerializeField] private GameObject bluestar;

    public void OnPut(Vector2 clickPos){
        string prefab = (GameManager.Turn==0)?redstar.name:bluestar.name;
        //Instantiate(redstar,clickPos,Quaternion.identity);
        PhotonNetwork.Instantiate(prefab,clickPos,Quaternion.identity);
    }

    public void OnRemove(GameObject hitObj){
        PhotonView hitView = hitObj.GetComponent<PhotonView>();
        if(hitView.IsMine){
            PhotonNetwork.Destroy(hitObj);
        }else{
            hitView.RequestOwnership();
        }
    }

    public void OnBurst(){

    }
}
