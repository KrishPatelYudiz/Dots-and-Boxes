using System.Collections.Generic;
using UnityEngine;
public enum GameScreens
{
    Play,
    Home
}
public enum GamePopUp
{
    GameOver,
}

public class UiManager : MonoBehaviour
{
    [SerializeField] BaseScreen _currentScreen;
    public static UiManager instance;
    [SerializeField] List<BaseScreen> _screens;
    [SerializeField] List<BasePopUp> _popUps;
    Stack<BasePopUp> PopUp = new Stack<BasePopUp>();

    private void Start()
    {
        instance = this;
        _currentScreen.ActivateScreen();
    }

    private void Update()
    {
        if (PopUp.Count != 0)
        {

            PopUp.Peek().TackInput();
        }

        else
        {
            _currentScreen.TackInput();

        }

    }
    public void SwitchScreen(GameScreens screen)
    {
        foreach (BaseScreen baseScreen in _screens)
        {
            if (baseScreen.screen == screen)
            {
                baseScreen.ActivateScreen();
                _currentScreen.DeactivateScreen();
                _currentScreen = baseScreen;
            }

        }
    }
    public void OpenPopUp(GamePopUp popUp)
    {
        foreach (BasePopUp basePopUp in _popUps)
        {
            if (basePopUp.popUp == popUp)
            {
                PopUp.Push(basePopUp);
                basePopUp.ActivatePopUp();
            }

        }
    }
    public void ClosePopUp() {
        if (PopUp.Count != 0)
            PopUp?.Pop().DeactivatePopUp();
    }

}