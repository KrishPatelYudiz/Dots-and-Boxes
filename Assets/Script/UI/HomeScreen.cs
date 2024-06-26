using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _computerPlayButton;

    private void Start()
    {

        _playButton.onClick.AddListener(OnPlay);
        _computerPlayButton.onClick.AddListener(OnComputerPlay);
    }
     public override void ActivateScreen()
    {
        base.ActivateScreen();
    }
    void OnPlay()
    {
        UiManager.instance.SwitchScreen(GameScreens.Play);
        AudioManager.instance.Play(SoundName.Button);
    }
    void OnComputerPlay()
    {
        UiManager.instance.SwitchScreen(GameScreens.ComputerPlay);
        AudioManager.instance.Play(SoundName.Button);
    }
    void OnExit()
    {
        Application.Quit();
    }
}
