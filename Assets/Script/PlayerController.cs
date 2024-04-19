using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player1 = new Player();
    [SerializeField] private Player player2 = new Player();

    public int Player1Points => player1.Point;
    public int Player2Points => player2.Point;
    public float Player1Time => player1.Time;
    public float Player2Time => player2.Time;
    public int Player1Lives => player1.LifeLine;
    public int Player2Lives => player2.LifeLine;
    public bool IsCurrentPlayerPlayer1 => currentPlayer == player1;
    public bool IsCurrentPlayerPlayer2 => currentPlayer == player2;

    public  Player currentPlayer;

    public delegate void PlayerChangeHandler();
    public static event PlayerChangeHandler OnPlayerChange;

    private void OnEnable()
    {
        BoardManager.OnTurnChange += ChangePlayer;
    }

    private void OnDisable()
    {
        BoardManager.OnTurnChange -= ChangePlayer;
    }

    private void Start()
    {
        currentPlayer = player1;
    }

    public void ChangePlayer()
    {
        currentPlayer = currentPlayer == player1 ? player2 : player1;
        OnPlayerChange?.Invoke();
        ResetTimer();
    }

    public Player GetWinner()
    {
        if (player1.LifeLine < 0)
        {
            return player2;
        }

        if (player2.LifeLine < 0)
        {
            return player1;
        }

        return player1.Point > player2.Point ? player1 : player2;
    }

    public void ResetTimer()
    {
        player1.ResetTime();
        player2.ResetTime();
    }

    public void RestartGame()
    {
        player1.ResetPlayer();
        player2.ResetPlayer();
        currentPlayer = player1;
        OnPlayerChange?.Invoke();
    }
}
