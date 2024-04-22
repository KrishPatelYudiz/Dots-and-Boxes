using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Base class for Player Screens with common behavior
public class PlayScreen : BaseScreen
{
    // Common fields
    [SerializeField] protected TMP_Text player1PointText;
    [SerializeField] protected TMP_Text player2PointText;
    [SerializeField] protected Image player1FilledImage;
    [SerializeField] protected Image player2FilledImage;
    [SerializeField] protected Button restartButton;
    [SerializeField] protected Button stopButton;
    [SerializeField] protected Button pauseButton;
    [SerializeField] protected Image pauseImage;
    [SerializeField] protected Sprite pauseIcon;
    [SerializeField] protected Sprite playIcon;
    [SerializeField] protected List<Image> player1LifeLineImages;
    [SerializeField] protected List<Image> player2LifeLineImages;
    [SerializeField] protected PlayerController playerController;

    protected bool isPaused = false;
    protected Coroutine timerCoroutine;
    protected Animator animator;

    protected Player currentPlayer => playerController.currentPlayer;

    protected override void onAwke()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void SubscribeEvents()
    {
        Box.OnBoxComplete += UpdateBox;
        PlayerController.OnPlayerChange += OnPlayerChange;
        GameOverScreen.onGameOver += OnPauseButton;
        pauseButton.onClick.AddListener(OnPauseButton);
        restartButton.onClick.AddListener(RestartGame);
        stopButton.onClick.AddListener(StopGame);
    }

    protected virtual void UnsubscribeEvents()
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
        isPaused = false;
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

    protected virtual void StartNewGame()
    {
        BoardManager.Instance.GenerateNewBoard();
        playerController.RestartGame();
        ResetTimer();
        UpdateState();
        ResetLifeLines();
        UpdatePointLabels();
    }

    protected virtual void OnPlayerChange()
    {
        SwitchAnimation();
        ResetTimer();
    }

    protected void StopGame()
    {
        AudioManager.instance.Play(SoundName.Button);

        UiManager.instance.ClosePopUp();
        UiManager.instance.SwitchScreen(GameScreens.Home);
    }

    protected void SwitchAnimation()
    {
        animator.SetTrigger(playerController.IsCurrentPlayerPlayer1 ? "Player1" : "Player2");
    }

    protected void ResetTimer()
    {
        playerController.ResetTimer();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    protected virtual void UpdateBox(Box box)
    {
        box.spriteRenderer.sprite = currentPlayer.iconeSprite;
        currentPlayer.IncrementPoint();
        UpdatePointLabels();
        ResetTimer();
    }

    protected void UpdatePointLabels()
    {
        player1PointText.text = playerController.Player1Points.ToString();
        player2PointText.text = playerController.Player2Points.ToString();
    }

    protected void OnPauseButton()
    {
        AudioManager.instance.Play(SoundName.Button);

        isPaused = !isPaused;
        UpdateState();
    }

    protected void UpdateState()
    {
        if (isPaused)
        {
            BoardManager.TriggerTakeInput(false);
            pauseImage.sprite = playIcon;
            animator.SetTrigger("idle");
            StopCoroutine(timerCoroutine);
        }
        else
        {
            BoardManager.TriggerTakeInput(true);
            pauseImage.sprite = pauseIcon;
            SwitchAnimation();
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    protected void RestartGame()
    {
        AudioManager.instance.Play(SoundName.Button);
        StartNewGame();
        UiManager.instance.ClosePopUp();
    }

    protected IEnumerator TimerCoroutine()
    {
        while (currentPlayer.Time > 0)
        {
            player1FilledImage.fillAmount = playerController.Player1Time / 15;
            player2FilledImage.fillAmount = playerController.Player2Time / 15;
            currentPlayer.DecrementTime(Time.deltaTime);
            yield return null;
        }
        UpdateLifeLines();
        playerController.ChangePlayer();
    }

    protected void UpdateLifeLines()
    {
        UpdateLifeLineImages(playerController.Player1Lives, player1LifeLineImages);
        UpdateLifeLineImages(playerController.Player2Lives, player2LifeLineImages);
    }

    protected void UpdateLifeLineImages(int lifeCount, List<Image> lifeLineImages)
    {
        for (int i = 0; i < lifeLineImages.Count; i++)
        {
            lifeLineImages[i].enabled = i < lifeCount;
        }
    }

    protected void ResetLifeLines()
    {
        ResetLifeLineImages(player1LifeLineImages);
        ResetLifeLineImages(player2LifeLineImages);
    }

    protected void ResetLifeLineImages(List<Image> lifeLineImages)
    {
        foreach (Image image in lifeLineImages)
        {
            image.enabled = true;
        }
    }
}
