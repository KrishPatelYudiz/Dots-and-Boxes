using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _exitButton;

    private void Start()
    {

        _playButton.onClick.AddListener(OnPlay);
    }
     public override void ActivateScreen()
    {
        base.ActivateScreen();
    }
    void OnPlay()
    {

        UiManager.instance.SwitchScreen(GameScreens.Play);
    }
    void OnExit()
    {

        Application.Quit();
    }
}
