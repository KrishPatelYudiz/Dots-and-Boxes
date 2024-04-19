
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Player 
{
    int _point = 0;
    int _life = 3;
    float _time = 5;
    
    public Sprite iconeSprite;
    
    public int Point {get{ return _point;}} 
    public int LifeLine {get{ return _life;}  }
    public float Time {get{return _time;}}
    
               
    public void IncrementPoint(){
        _point ++;
    }
    public void DecrementTime(float decTime){
        _time -= decTime;
        if (_time <= 0)
        {
            DecrementLife();
        }
    }
    
    void DecrementLife(){
        _life--;                                  
        if (_life == -1)
        {
            UiManager.instance.OpenPopUp(GamePopUp.GameOver);
        }

    }
    public void ResetTime(){
        _time = 5;
    }

    public void ResetPlayer(){
        ResetTime();
        _life = 3;
        _point = 0;
    }
       
}
