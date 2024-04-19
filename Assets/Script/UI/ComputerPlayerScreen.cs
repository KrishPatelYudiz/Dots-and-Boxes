using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPlayerScreen : BaseScreen
{
    [SerializeField] TMP_Text _player1PointText;
    [SerializeField] TMP_Text _player2PointText;
    [SerializeField] Image _player1FilledImage;
    [SerializeField] Image _player2FilledImage;
 
    [SerializeField] Button _restartButton;
    [SerializeField] Button _stopButton;
    [SerializeField] Button _pauseButton;

    [SerializeField] Image _pauseImage;
    [SerializeField] Sprite _pauseIcone;
    [SerializeField] Sprite _playIcone;
    [SerializeField] List<Image> _player1lifeLineImage;
    [SerializeField] List<Image> _player2lifeLineImage;
    [SerializeField] PlayerController playerController;
    Player currentPlayer => playerController.currentPlayer;

    Coroutine timerCoroutine;
    Animator animator;

    bool _isPause = false;


    protected override void onAwke()
    {
        base.onAwke();
        animator = GetComponent<Animator>();
    }
    private void OnEnable() {
        Box.onBoxComplet += UpdateBox;
        PlayerController.onPlayerChange += OnPlayerChange;
        GameOverScreen.onGameOver += OnPauseButton;
        _pauseButton.onClick.AddListener(OnPauseButton);
        _restartButton.onClick.AddListener(RestartGame);
        _stopButton.onClick.AddListener(StopButton);
    }
    private void OnDisable() {
        Box.onBoxComplet -= UpdateBox;
        PlayerController.onPlayerChange -= OnPlayerChange;
        GameOverScreen.onGameOver -= OnPauseButton;

        _pauseButton.onClick.RemoveListener(OnPauseButton);
        _restartButton.onClick.RemoveListener(RestartGame);
        _stopButton.onClick.RemoveListener(StopButton);
    }

    public override void ActivateScreen()
    {
        BoardManager.TackInput(true);
        base.ActivateScreen();
        StartNewGame();
    }
    public override void DeactivateScreen()
    {
        base.DeactivateScreen();
        BoardManager.Instance.DestroyBoard();
        StopAllCoroutines();
        BoardManager.TackInput(false);

    }

    void StartNewGame(){
        StopAllCoroutines();
        BoardManager.Instance.GenerateNewBord();
        ResetTimer();
        SwitchAnimason();
        UpdateState();
    }
    void OnPlayerChange(){
        SwitchAnimason();
        ResetTimer();

    }
    void StopButton(){
        UiManager.instance.SwitchScreen(GameScreens.Home);
    }
    void SwitchAnimason(){
        if(playerController.currentPlayerIsPlayer1){
            
            animator.SetTrigger("Player1");
        }else
        {
            animator.SetTrigger("Player2");
        }
    }
   
    void UpdateBox(Box box){
        
        box.spriteRenderer.sprite = currentPlayer.iconeSprite;
        currentPlayer.IncrementPoint();
        UpdatePointLbl();
        ResetTimer();
    }
    void UpdatePointLbl(){
        _player1PointText.text = playerController.player1Point.ToString(); 
        _player2PointText.text = playerController.player2Point.ToString(); 
    }
    void ResetTimer(){

        playerController.ResetTimer();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
         timerCoroutine = StartCoroutine(TimerCoroutine());
    }
    void OnPauseButton(){
        _isPause = !_isPause;
        UpdateState();                    
    }
    void UpdateState(){
        if (_isPause){
            BoardManager.TackInput(false);
            _pauseImage.sprite = _playIcone;
            animator.SetTrigger("idle");
            StopCoroutine(timerCoroutine);
        }else
        {
            
            _pauseImage.sprite = _pauseIcone;
            SwitchAnimason();
            StopCoroutine(timerCoroutine);
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }    

    }
    void RestartGame(){
        playerController.RestartGame();
        UpdatePointLbl();
        ResetLife();
        StartNewGame();
        UiManager.instance.ClosePopUp();
    }

    IEnumerator TimerCoroutine(){
        while (currentPlayer.Time > 0)
        {
            _player1FilledImage.fillAmount = playerController.player1Time / 5;
            _player2FilledImage.fillAmount = playerController.player2Time / 5;
            currentPlayer.DecrementTime(Time.deltaTime);
            yield return null;
        }
        UpdateLife();
        playerController.ChangPlayer();
    }

    void UpdateLife(){
        
        if (playerController.player1Life < _player1lifeLineImage.Count && playerController.player1Life >= 0)
        {
            _player1lifeLineImage[playerController.player1Life].enabled =false;
        }
        if (playerController.player2Life < _player2lifeLineImage.Count && playerController.player2Life >= 0)
        {
            _player2lifeLineImage[playerController.player2Life].enabled =false;
        }
    }
    void ResetLife(){
        
       foreach (var image in _player1lifeLineImage)
       {
         image.enabled = true;
       }
       foreach (var image in _player2lifeLineImage)
       {
         image.enabled = true;
       }
    }

}

