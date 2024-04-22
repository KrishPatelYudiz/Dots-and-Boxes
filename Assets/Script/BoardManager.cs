using System.Collections;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BoardGenerator boardGeneratorPrefab;
    private BoardGenerator boardGenerator;
    private Box[,] board;
    public static int[,] lineData;
    private bool[,] boardMap = {
        {false, false, true, true, false, false},
        {false, true, true, true, true, false},
        {true, true, true, true, true, true},
        {true, true, true, true, true, true},
        {false, true, true, true, true, false},
        {false, false, true, true, false, false},
    };
    
    public delegate void TurnChangeEvent();
    public static event TurnChangeEvent OnTurnChange;

    public delegate void TakeInputEvent(bool state);
    public static event TakeInputEvent OnTakeInput;
    public delegate void ComputerInputEvent();
    public static event ComputerInputEvent OnComputerInput;
    
    public static BoardManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        OnTakeInput += HandleTakeInputState;
        OnComputerInput += HandleComputerInputState;
    }

    public static void TriggerTakeInput(bool state) => OnTakeInput?.Invoke(state);
    public static void TriggerComputerInput() => OnComputerInput?.Invoke();
    
    private void HandleTakeInputState(bool state) {

        StopAllCoroutines();
        
        if (state)
            StartCoroutine(HandleTakeInput());

    }
    private void HandleComputerInputState() {
        StopAllCoroutines();
        StartCoroutine(ComputerInput());
    }

    public void DestroyBoard() {
        if (boardGenerator != null) {
            Destroy(boardGenerator.gameObject);
        }
    }

    public void GenerateNewBoard() {
        DestroyBoard();
        boardGenerator = Instantiate(boardGeneratorPrefab, transform).GetComponent<BoardGenerator>();
        board = boardGenerator.MakeNewBoard(boardMap,out lineData);
    }
    
    private IEnumerator HandleTakeInput() {
        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                var raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (raycastHit.collider != null) {
                    var line = raycastHit.collider.gameObject.GetComponent<Line>();
                    if (line != null && !line.isSelected) {
                        line.isSelected = true;
                        AudioManager.instance.Play(SoundName.LineFill);

                        UpdateBoard();
                    }
                }
            }
            yield return null;
        }
    }
    private IEnumerator ComputerInput() {
        
        float time = Random.Range(1.0F,3.0F);
        while (time > 0)
        {
            time -= Time.deltaTime;   
            yield return null;
        }
        int dimension = board.GetLength(0);
        for (int row = 0; row < dimension; row++) {
            for (int col = 0; col < dimension; col++) {
                if (board[row, col] != null && !board[row, col].IsComplete) {
                    board[row, col].FillOneLine();
                    AudioManager.instance.Play(SoundName.LineFill);
                    UpdateBoardForComputer();
                    yield break;
                }
            }
        }
    }
    private bool UpdateBoard() {
        bool isFill = false;
        int dimension = board.GetLength(0);
        for (int row = 0; row < dimension; row++) {
            for (int col = 0; col < dimension; col++) {
                if (board[row, col] != null && !board[row, col].IsComplete && board[row, col].CheckComplete()) {
                    isFill = true;
                }
            }
        }

        if (!isFill) {
            OnTurnChange?.Invoke();
            return true;
        }

        if (CheckIfBoardIsComplete()) {
            UiManager.instance.OpenPopUp(GamePopUp.GameOver);
            return true;
        }
        return false;
    }
    private void UpdateBoardForComputer() {
       if (UpdateBoard())
       {
        return;
       }
        HandleComputerInputState();
    }

    private bool CheckIfBoardIsComplete() {
        int dimension = board.GetLength(0);
        for (int row = 0; row < dimension; row++) {
            for (int col = 0; col < dimension; col++) {
                if (board[row, col] != null && !board[row, col].IsComplete) {
                    return false;
                }
            }
        }
        return true;
    }
    

}
