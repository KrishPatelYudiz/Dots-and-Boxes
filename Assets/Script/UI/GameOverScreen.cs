using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : BasePopUp
{
    [SerializeField] Image _winnerIconeImage;
    [SerializeField] PlayerController playerController;
    [SerializeField] Button Backbutton;
    Animator animator;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    protected override void onAwke()
    {
        animator = GetComponent<Animator>();
        Backbutton.onClick.AddListener(OnBackbutton);
    }
    public override void ActivatePopUp()
    {
        AudioManager.instance.Play(SoundName.GameOver);
        _winnerIconeImage.sprite =  playerController.GetWinner().iconeSprite;
        animator.SetTrigger("GameOver");
        onGameOver?.Invoke();
        base.ActivatePopUp();
    }
    void OnBackbutton(){
        UiManager.instance.ClosePopUp();
        UiManager.instance.SwitchScreen(GameScreens.Home);
    }
}
