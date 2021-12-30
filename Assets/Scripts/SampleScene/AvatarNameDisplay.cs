using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class AvatarNameDisplay : MonoBehaviourPunCallbacks
{
    private void Start(){
        var nameLabel = GetComponent<TextMeshPro>();
        nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
    }    
}
