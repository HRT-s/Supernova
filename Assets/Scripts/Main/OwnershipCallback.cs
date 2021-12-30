using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OwnershipCallback : MonoBehaviourPunCallbacks,IPunOwnershipCallbacks
{
    public void OnOwnershipRequest(PhotonView targetView,Player requestingPlayer){

    }

    public void OnOwnershipTransfered(PhotonView targetView,Player previousOwner){
        if(!targetView.IsMine) return;
        PhotonNetwork.Destroy(targetView.gameObject);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView,Player previousOwner){
        
    }
}
