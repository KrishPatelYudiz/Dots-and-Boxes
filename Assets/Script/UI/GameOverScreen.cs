using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : BasePopUp
{
    [SerializeField] Button _restartButton;
    [SerializeField] Button _homeButton;
    [SerializeField] Image _winnerIconeImage;
    private void Start()
    {

        _restartButton.onClick.AddListener(OnRestart);
        _homeButton.onClick.AddListener(OnHome);
    }
    public override void ActivatePopUp()
    {
        _winnerIconeImage.sprite =  GameManager.Instance.GetWinner().iconeSprite;
        base.ActivatePopUp();
    }
    void OnRestart()
    {
    }
    void OnHome()
    {
        AudioManager.instance.Play(SoundName.ButtonSound);
        //UiManager.instance.SwitchScreen(GameScreens.Home);

    }

}
