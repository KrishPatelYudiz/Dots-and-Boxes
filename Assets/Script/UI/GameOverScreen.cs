using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : BasePopUp
{
    [SerializeField] Image _winnerIconeImage;
    [SerializeField] PlayerController playerController;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    public override void ActivatePopUp()
    {
        _winnerIconeImage.sprite =  playerController.GetWinner().iconeSprite;
        onGameOver?.Invoke();
        base.ActivatePopUp();
    }

}
