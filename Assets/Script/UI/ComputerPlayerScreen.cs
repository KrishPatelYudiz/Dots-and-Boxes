public class ComputerPlayerScreen : PlayScreen
{
    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        BoardManager.TriggerTakeInput(true);
    }

    protected override void OnPlayerChange()
    {
        if (playerController.IsCurrentPlayerPlayer1)
        {
            BoardManager.TriggerTakeInput(true);
        }
        else
        {
            BoardManager.TriggerTakeInput(false);
            BoardManager.TriggerComputerInput();
        }

        base.OnPlayerChange();
    }

    protected override void StartNewGame()
    {
        base.StartNewGame();
        BoardManager.TriggerTakeInput(true);
    }
}