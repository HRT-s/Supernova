using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Board : MonoBehaviourPunCallbacks{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject burst;
    private const int BOARD_SIZE = 10;
    private const int NUM_WIN = 5;
    private int count;
    private Star[,] mass = new Star[BOARD_SIZE,BOARD_SIZE];
    private Star[] checkedStars = new Star[NUM_WIN];
    private GameObject burstObj;

    private void Start(){
        for(int i = 0;i < BOARD_SIZE;i++){
            for(int j = 0;j < BOARD_SIZE;j++){
                mass[i,j] = null;
            }
        }
        for(int i = 0;i < NUM_WIN;i++){
            checkedStars[i] = null;
        }
    }

    private bool Check(Vector2 clickPos){
        int checkTurn = GameManager.Turn;
        int x = (int)clickPos.x;
        int y = (int)clickPos.y;
        if(CheckHorizontal(x,y,checkTurn)) return true;
        if(CheckVertical(x,y,checkTurn)) return true;
        if(CheckRightDiagonal(x,y,checkTurn)) return true;
        if(CheckLeftDiagonal(x,y,checkTurn)) return true;
        return false;
    }

    private void CountingStar(int x, int y){
        checkedStars[count] = mass[y,x];
        count++;
    }
    
    private bool CheckHorizontal(int x, int y, int checkTurn){
        count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN+1;i++){
            if(x+i < 0 || x+i >= BOARD_SIZE) continue;
            if(mass[y,x+i] == null){
                count = 0;
                continue;
            } 
            if(mass[y,x+i].ID == checkTurn) CountingStar(x+i,y);
            else count = 0;
            if(count >= NUM_WIN) return true;
        }
        return false;
    }

    private bool CheckVertical(int x, int y, int checkTurn){
        count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN+1;i++){
            if(y+i < 0 || y+i >= BOARD_SIZE) continue;
            if(mass[y+i,x] == null){
                count = 0;
                continue;
            }
            if(mass[y+i,x].ID == checkTurn) CountingStar(x,y+i);
            else count = 0;
            if(count >= NUM_WIN) return true;
        }
        return false;
    }

    private bool CheckRightDiagonal(int x, int y, int checkTurn){
        count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN+1;i++){
            if(x+i < 0 || x+i >= BOARD_SIZE) continue;
            if(y+i < 0 || y+i >= BOARD_SIZE) continue;
            if(mass[y+i,x+i] == null){
                count = 0;
                continue;
            }
            if(mass[y+i,x+i].ID == checkTurn) CountingStar(x+i,y+i);
            else count = 0;
            if(count >= NUM_WIN) return true;
        }
        return false;
    }

    private bool CheckLeftDiagonal(int x, int y, int checkTurn){
        count = 0;
        for(int i = -NUM_WIN+1;i < NUM_WIN+1;i++){
            if(x+i < 0 || x+i >= BOARD_SIZE) continue;
            if(y-i < 0 || y-i >= BOARD_SIZE) continue;
            if(mass[y-i,x+i] == null){
                count = 0;
                continue;
            }
            if(mass[y-i,x+i].ID == checkTurn) CountingStar(x+i,y-i);
            else count = 0;
            if(count >= NUM_WIN) return true;
        }
        return false;
    }

    private void CallGameEnd(){
        gameManager.GameEndCaller();
    }

    public void OnPut(Vector2 clickPos){
        GameObject star = PhotonNetwork.Instantiate($"{GameManager.Turn}_star",clickPos,Quaternion.identity);
        mass[(int)clickPos.y,(int)clickPos.x] = star.GetComponent<Star>();
        if(Check(clickPos)){
            for(int i = 0;i < NUM_WIN;i++){
                checkedStars[i].Shining();
            }
            Invoke("CallGameEnd",1.0f);
        }
    }

    public void OnRemove(GameObject hitObj){
        Star hitStar = hitObj.GetComponent<Star>();
        hitStar.DestroyMe();
    }

    public void OnBurst(Vector2 clickPos){
        burstObj = Instantiate(burst,clickPos,Quaternion.identity);
        Invoke("BurstEnd",0.05f);
    }

    private void BurstEnd(){
        Destroy(burstObj);
    }
}
