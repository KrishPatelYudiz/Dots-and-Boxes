using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPlayerScreen : BaseScreen
{
    [SerializeField] private TMP_Text player1PointText;
    [SerializeField] private TMP_Text player2PointText;
    [SerializeField] private Image player1FilledImage;
    [SerializeField] private Image player2FilledImage;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image pauseImage;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private List<Image> player1LifeLineImages;
    [SerializeField] private List<Image> player2LifeLineImages;
    [SerializeField] private PlayerController playerController;

    private bool isPaused = false;
    private Coroutine timerCoroutine;
    private Animator animator;

    private Player currentPlayer => playerController.currentPlayer;

    protected override void onAwke()
    {
        animator = GetComponent<Animator>();
    }

    private void SubscribeEvents()
    {
        Box.OnBoxComplete += UpdateBox;
        PlayerController.OnPlayerChange += OnPlayerChange;
        GameOverScreen.onGameOver += OnPauseButton;
        pauseButton.onClick.AddListener(OnPauseButton);
        restartButton.onClick.AddListener(RestartGame);
        stopButton.onClick.AddListener(StopGame);
    }

    private void UnsubscribeEvents()
    {
        Box.OnBoxComplete -= UpdateBox;
        PlayerController.OnPlayerChange -= OnPlayerChange;
        GameOverScreen.onGameOver -= OnPauseButton;
        pauseButton.onClick.RemoveListener(OnPauseButton);
        restartButton.onClick.RemoveListener(RestartGame);
        stopButton.onClick.RemoveListener(StopGame);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
        SubscribeEvents();

        BoardManager.TriggerTakeInput(true);
        StartNewGame();
    }

    public override void DeactivateScreen()
    {
        base.DeactivateScreen();
        BoardManager.Instance.DestroyBoard();
        StopAllCoroutines();
        BoardManager.TriggerTakeInput(false);
        UnsubscribeEvents();
    }

    private void StartNewGame()
    {
        BoardManager.Instance.GenerateNewBoard();
        ResetTimer();
        UpdateState();
        ResetLifeLines();
        UpdatePointLabels();
    }

    private void OnPlayerChange()
    {
        if(playerController.IsCurrentPlayerPlayer1){
            BoardManager.TriggerTakeInput(true);

        }
        if(playerController.IsCurrentPlayerPlayer2){
            BoardManager.TriggerTakeInput(false);
            BoardManager.TriggerComputerInput();
        }
        SwitchAnimation();
        ResetTimer();
    }

    private void StopGame()
    {
        AudioManager.instance.Play(SoundName.Button);

        UiManager.instance.ClosePopUp();
        UiManager.instance.SwitchScreen(GameScreens.Home);
    }

    private void SwitchAnimation()
    {
        animator.SetTrigger(playerController.IsCurrentPlayerPlayer1 ? "Player1" : "Player2");
    }
    void ResetTimer(){

        playerController.ResetTimer();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
         timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void UpdateBox(Box box)
    {
        box.spriteRenderer.sprite = currentPlayer.iconeSprite;
        currentPlayer.IncrementPoint();
        UpdatePointLabels();
        ResetTimer();
    }

    private void UpdatePointLabels()
    {
        player1PointText.text = playerController.Player1Points.ToString();
        player2PointText.text = playerController.Player2Points.ToString();
    }

    private void OnPauseButton()
    {
        AudioManager.instance.Play(SoundName.Button);

        isPaused = !isPaused;
        UpdateState();
    }

    
    void UpdateState(){
        if (isPaused){
            BoardManager.TriggerTakeInput(false);
            pauseImage.sprite = playIcon;
            animator.SetTrigger("idle");
            StopCoroutine(timerCoroutine);
        }else
        {
            if (playerController.IsCurrentPlayerPlayer1)
            {
                BoardManager.TriggerTakeInput(true);
            }else{
                BoardManager.TriggerComputerInput();
            }
            pauseImage.sprite = pauseIcon;
            SwitchAnimation();
            StopCoroutine(timerCoroutine);
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            timerCoroutine = StartCoroutine(TimerCoroutine());
    }    
    }
    private void RestartGame()
    {
        AudioManager.instance.Play(SoundName.Button);

        playerController.RestartGame();
        StartNewGame();
        UiManager.instance.ClosePopUp();
    }

    private IEnumerator TimerCoroutine()
    {
        while (currentPlayer.Time > 0)
        {
            player1FilledImage.fillAmount = playerController.Player1Time / 5;
            player2FilledImage.fillAmount = playerController.Player2Time / 5;
            currentPlayer.DecrementTime(Time.deltaTime);
            yield return null;
        }
        UpdateLifeLines();
        playerController.ChangePlayer();
    }

    private void UpdateLifeLines()
    {
        UpdateLifeLineImages(playerController.Player1Lives, player1LifeLineImages);
        UpdateLifeLineImages(playerController.Player2Lives, player2LifeLineImages);
    }

    private void UpdateLifeLineImages(int lifeCount, List<Image> lifeLineImages)
    {
        for (int i = 0; i < lifeLineImages.Count; i++)
        {
            lifeLineImages[i].enabled = i < lifeCount;
        }
    }

    private void ResetLifeLines()
    {
        ResetLifeLineImages(player1LifeLineImages);
        ResetLifeLineImages(player2LifeLineImages);
    }

    private void ResetLifeLineImages(List<Image> lifeLineImages)
    {
        foreach (Image image in lifeLineImages)
        {
            image.enabled = true;
        }
    }
}
