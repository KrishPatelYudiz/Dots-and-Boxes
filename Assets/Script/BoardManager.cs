using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
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
      boardMap = GenerateBoardMap(7);
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
        
        float time = Random.Range(1.0F,3f);
        while (time > 0)
        {
            time -= Time.deltaTime;   
            yield return null;
        }
        FillLine();
    }
    void FillLine(){
        int dimension = board.GetLength(0);
        bool ThreeCount = false;
        bool OneCount = false;
        bool ZeroCount = false;
        
        for (int row = 0; row < dimension; row++) {
            for (int col = 0; col < dimension; col++) {
                if (board[row, col] != null && !board[row, col].IsComplete) {
                    int linecount = board[row, col].FilledLineCount();
                    switch (linecount) {
                        case 0:
                            ZeroCount = true;
                            break;
                        case 1:
                            OneCount = true;
                            break;
                        case 3:
                            ThreeCount = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        int count = 2;
        if (ThreeCount)
        {
            count = 3;
        }else if(OneCount)
        {
            count = 1;
            
        
        }else if(ZeroCount)
        {
            count = 0;
            
        }
        print(count);
        for (int row = 0; row < dimension; row++) {
            for (int col = 0; col < dimension; col++) {
                if (board[row, col] != null && !board[row, col].IsComplete && board[row, col].FilledLineCount() == count) {
                    print(row+" " +col);
                    board[row, col].FillOneLine();
                    AudioManager.instance.Play(SoundName.LineFill);
                    UpdateBoardForComputer();
                    return;
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
    
    public  bool[,] GenerateBoardMap(int size)
    {
        bool[,] boardMap = new bool[size, size];

        
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                boardMap[row, col] = true;
            }
        }

        int rows = Random.Range(1, size/2);
        int cols = Random.Range(1, size/2);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                boardMap[row, col] = false;
                boardMap[row, size - (col+1)] = false;
                boardMap[size - (row+1), col] = false;
                boardMap[size - (row+1), size - (col+1)] = false;
            }
            cols = Random.Range(0, cols);
        }
        
        int count = 7;
        while (count > 0)
        {
            int randomRow = Random.Range(2,size-2); 
            int randomCol = Random.Range(2,size-2); 
            count -= canFill(boardMap, randomRow, randomCol) ? 1 : 0;
        }

        RemoveSingaleBox(boardMap,size);
        return boardMap;
        
    }
     bool canFill( bool[,] boardMap , int Row,int Col){

        boardMap[Row,Col] = false;
        for (int row = Row-1; row <= Row+1; row++)
        {
            for (int col = Col-1; col <= Col+1; col++)
            {
                if (boardMap[Row,Col])
                {
                    int count = 0;
                    if(boardMap[row,col+1]) count++;
                    if(boardMap[row,col-1]) count++;
                    if(boardMap[row+1,col]) count++;
                    if(boardMap[row-1,col]) count++;

                    if (count<2)
                    {
                       boardMap[Row,Col] = true;
                       return false;
                    }
                }
                
            }
        }
        return true;
    } 
    void RemoveSingaleBox(bool[,]boardMap,int size){
        for (int row = 1; row < size-1; row++)
        {
            for (int col = 1; col < size-1; col++)
            {
                if (!boardMap[row,col])
                {
                    int count = 0;
                    if(!boardMap[row,col-1]) count++;
                    if(!boardMap[row,col+1]) count++;
                    if(!boardMap[row+1,col]) count++;
                    if(!boardMap[row-1,col]) count++;

                    if (count == 0)
                    {
                       boardMap[row,col] = true;
                    }
                }
            }
        }
    }

}
