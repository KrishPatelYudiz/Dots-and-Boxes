
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
    float _time = 15;
    
    public Sprite iconeSprite;
    
    public int Point {get{ return _point;}} 
    public int LifeLine {get{ return _life;}} 
    public float Time {get{return _time;}}
    [SerializeField] List<Image> _lifeLineImage;
               
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
        if (_life == 0)
        {
            UiManager.instance.OpenPopUp(GamePopUp.GameOver);
            return;
        }

        _life--;
        _lifeLineImage[_life].enabled = false;                                    
    }
    public void ResetTime(){
        _time = 15;
    }
       
}
