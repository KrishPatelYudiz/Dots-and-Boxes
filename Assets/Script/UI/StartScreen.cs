using UnityEngine;
using UnityEngine.UI;

public class StartScreen : BaseScreen
{
    [SerializeField] Button _playButton;

    private void Start()
    {

        _playButton.onClick.AddListener(OnPlay);
    }
     public override void ActivateScreen()
    {
        AudioManager.instance.PlayInBackGround(SoundName.BG1);
        base.ActivateScreen();
    }
    void OnPlay()
    {
        AudioManager.instance.Play(SoundName.Button);

        UiManager.instance.SwitchScreen(GameScreens.Home);
    }
    void OnExit()
    {

        Application.Quit();
    }
}
