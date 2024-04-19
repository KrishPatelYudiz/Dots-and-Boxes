using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Player player1 = new Player();
    [SerializeField] Player player2 = new Player();

    public int player1Point=> player1.Point;
    public int player2Point=> player2.Point;
    public float player1Time=> player1.Time;
    public float player2Time=> player2.Time;
    public int player1Life=> player1.LifeLine;
    public int player2Life=> player2.LifeLine;
    public bool currentPlayerIsPlayer1=> currentPlayer == player1;
    public bool currentPlayerIsPlayer2=> currentPlayer == player2;

    public Player currentPlayer;

    public delegate void OnPlayerChange();
    public static event OnPlayerChange onPlayerChange;
    private void OnEnable() {
        BoardManager.onTurnChange += ChangPlayer;

    }
    private void OnDisable() {
        BoardManager.onTurnChange -= ChangPlayer;
    }

    
    private void Start() {
        currentPlayer = player1;
    }
    public void ChangPlayer(){
        
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
        }else
        {
            currentPlayer = player1;
        }
        onPlayerChange?.Invoke();
        ResetTimer(); 
    }
    
    public  Player GetWinner(){

        if (player1.LifeLine == -1)
        {
            return player2;
        }

        if (player2.LifeLine == -1)
        {
            return player1;
        }
           
        return (player1.Point > player2.Point) ? player1 : player2;
    }

    public void ResetTimer(){

        player1.ResetTime();
        player2.ResetTime();

    }
    public void RestartGame(){
        player1.ResetPlayer();
        player2.ResetPlayer();
        currentPlayer = player1;
    }


}

