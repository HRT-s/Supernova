using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnim : MonoBehaviour{
    private int count;
    private float time;
    TextMeshProUGUI tmp;
    private bool isMatching;
    public bool IsMatching{set{isMatching = value;}}

    private void Start(){
        count = 0;
        time = 0;
        isMatching = false;
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Update(){
        if(isMatching){
            tmp.text = "Match Found";
        }else{
            if(time > 0.5f){
                count = (count+1) % 4;
                tmp.text = "Now Matching" + new string('.',count);
                time = 0;
            }
            time += Time.deltaTime;
        }
    }
}
