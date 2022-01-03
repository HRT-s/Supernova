using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Board : MonoBehaviourPunCallbacks{
    [SerializeField] private GameObject redstar;
    [SerializeField] private GameObject bluestar;
    private const int BOARD_SIZE = 10;
    private const int NUM_WIN = 7;
    private int[,] mass = new int[BOARD_SIZE,BOARD_SIZE];

    private void Start(){
        for(int i = 0;i < BOARD_SIZE;i++){
            for(int j = 0;j < BOARD_SIZE;i++){
                mass[i,j] = -1;
            }
        }
    }

    private bool Check(Vector2 clickPos){
        int checkTurn = GameManager.Turn;
        int x = (int)clickPos.x;
        int y = (int)clickPos.y;
        if(CheckLine(x,y,checkTurn)) return true;
        if(CheckLine(y,x,checkTurn)) return true;
        if(CheckRightDiagonal(x,y,checkTurn)) return true;
        if(CheckLeftDiagonal(x,y,checkTurn)) return true;
        return false;
    }

    private bool CheckLine(int x, int y, int checkTurn){
        int count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN;i++){
            if(x+i < 0 || x+i >= BOARD_SIZE) continue;
            if(mass[y,x+i] == checkTurn) count++;
        }
        if(count >= NUM_WIN) return true;
        return false;
    }

    private bool CheckRightDiagonal(int x, int y, int checkTurn){
        int count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN;i++){
            if(x+i < 0 || x+i >= NUM_WIN) continue;
            if(y+i < 0 || y+i >= NUM_WIN) continue;
            if(mass[y+i,x+i] == checkTurn) count++;
        }
        if(count >= NUM_WIN) return true;
        return false;
    }

    private bool CheckLeftDiagonal(int x, int y, int checkTurn){
        int count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN;i++){
            if(x+i < 0 || x+i >= NUM_WIN) continue;
            if(y+i < 0 || y-i >= NUM_WIN) continue;
            if(mass[y-i,x+i] == checkTurn) count++;
        }
        if(count >= NUM_WIN) return true;
        return false;
    }

    public void OnPut(Vector2 clickPos){
        string prefab = (GameManager.Turn==0)?redstar.name:bluestar.name;
        PhotonNetwork.Instantiate(prefab,clickPos,Quaternion.identity);
        mass[(int)clickPos.y,(int)clickPos.x] = GameManager.Turn;
    }

    public void OnRemove(GameObject hitObj, Vector2 clickPos){
        PhotonView hitView = hitObj.GetComponent<PhotonView>();
        if(hitView.IsMine){
            PhotonNetwork.Destroy(hitObj);
        }else{
            hitView.RequestOwnership();
        }
        mass[(int)clickPos.y,(int)clickPos.x] = -1;
    }

    public void OnBurst(){

    }
}
