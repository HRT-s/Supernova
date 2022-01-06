using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Photon.Pun;

public class Star : MonoBehaviourPunCallbacks{
    private int id;
    public int ID{get{return id;}}

    private void Awake(){
        id = int.Parse(Regex.Replace(this.name,@"[^0-9]",""));
    }

    public void DestroyMe(){
        if(photonView.IsMine){
            PhotonNetwork.Destroy(this.gameObject);
        }else{
            photonView.RequestOwnership();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            DestroyMe();
        }
    }
}
