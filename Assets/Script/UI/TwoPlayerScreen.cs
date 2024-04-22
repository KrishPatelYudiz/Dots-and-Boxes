public class TwoPlayerScreen : PlayScreen
{
    protected override void StartNewGame()
    {
        base.StartNewGame();
        BoardManager.TriggerTakeInput(true);
    }
}