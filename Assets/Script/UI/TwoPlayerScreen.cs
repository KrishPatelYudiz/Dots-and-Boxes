using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowPlayerScreen : BaseScreen
{
    [SerializeField] TMP_Text _player1PointText;
    [SerializeField] TMP_Text _player2PointText;
    [SerializeField] Image _player1FilledImage;
    [SerializeField] Image _player2FilledImage;

    [SerializeField] Button _restartButoon;
    [SerializeField] Button _stopButoon;
    [SerializeField] Button _pauseButoon;
    [SerializeField] Player player1 = new Player();
    [SerializeField] Player player2 = new Player();
    Player currentPlayer;

    Coroutine timerCoroutine;
    Animator animator;


    protected override void onAwke()
    {
        base.onAwke();
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        BoardManager.onTurnChange += ChangPlayer;
        Box.onBoxComplet += UpdateBox;
    }

    private void OnDisable() {
        BoardManager.onTurnChange -= ChangPlayer;
        Box.onBoxComplet -= UpdateBox;
    }

    
    private void Start() {
        StartNewGame();
    }
  
    void StartNewGame(){

        GameManager.Instance.GenerateNewBord();
        currentPlayer = player1;
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }
    
    public void ChangPlayer(){
        
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
            animator.SetTrigger("Player2");
        }else
        {
            currentPlayer = player1;
            animator.SetTrigger("Player1");
        }

        ResetTimer(); 
    }

    public Player GetWinner(){

        if (player1.LifeLine == 0)
        {
            return player1;
        }

        if (player2.LifeLine == 0)
        {
            return player2;
        }
           
        return (player1.Point > player2.Point) ? player1 : player2;
    }
   
    void UpdateBox(Box box){
        
        box.spriteRenderer.sprite = currentPlayer.iconeSprite;
        currentPlayer.IncrementPoint();

        _player1PointText.text = player1.Point.ToString(); 
        _player2PointText.text = player2.Point.ToString(); 
        
        ResetTimer();
    }

    void ResetTimer(){

        player1.ResetTime();
        player2.ResetTime();
        StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(TimerCoroutine());

    }
   
    IEnumerator TimerCoroutine(){
        while (currentPlayer.Time > 0)
        {
            _player1FilledImage.fillAmount = player1.Time / 15;
            _player2FilledImage.fillAmount = player2.Time / 15;

            currentPlayer.DecrementTime(Time.deltaTime);
            yield return null;
        }
        ChangPlayer();
    }


}

