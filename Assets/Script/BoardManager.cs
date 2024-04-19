
using System.Collections;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    
    bool[,] board_map ={
        {false,false,true,true,false,false},
        {false,true,true,true,true,false},
        {true,true,true,true,true,true},
        {true,true,true,true,true,true},
        {false,true,true,true,true,false},
        {false,false,true,true,false,false},
    }; 
    public static BoardManager Instance ;

    [SerializeField]BoardGenerator _boardGeneratorPrefab;           
    BoardGenerator boardGenerator;           
    Box[,] board;
    public delegate void OnTurnChange();
    public static event OnTurnChange onTurnChange;
    public delegate void OnTackInput(bool State);
    public static event OnTackInput onTackInput;

    public static void TackInput(bool State){
        onTackInput?.Invoke(State);
    }
   
    Coroutine tackInput;
    


    private void Awake() {
        Instance = this;
        onTackInput+= SetState;
    }
    void SetState(bool State){
        if (tackInput != null )
        {
            StopCoroutine(tackInput);
        }
        if (State)
        {
           tackInput = StartCoroutine(TackInput());
        }
    }
    public void DestroyBoard(){
        if (boardGenerator != null)
        {
            Destroy(boardGenerator.gameObject);
        }
    }       
    public void GenerateNewBord(){
        DestroyBoard();
        boardGenerator = Instantiate(_boardGeneratorPrefab,transform).GetComponent<BoardGenerator>(); 
        board =  boardGenerator.MakeNewBoard(board_map);
        
    }
    
    IEnumerator TackInput() {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    
                if(hit.collider != null)
                {
                Line line = hit.collider.gameObject.GetComponent<Line>();
                    if (line != null)
                    {
                        if (line.isSelected == false)
                        {
                            line.isSelected = true;
                            UpdateBox();
                        }

                    }
                } 
            }
            yield return null;
        }
    }

    void UpdateBox(){
        bool isFill = false; 
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(0); col++)
            {
                if(board[row,col] != null){
                    if(!board[row,col].isComplite ){
                        if(board[row,col].ChackComplite())
                            isFill = true;
                    }
                }
            }   
        }

        if (!isFill){
            onTurnChange?.Invoke();
            return;
        }
        
        if(CheckForComplet()){
            UiManager.instance.OpenPopUp(GamePopUp.GameOver);
        };
        
    }


    bool CheckForComplet(){
         for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(0); col++)
            {
                if(board[row,col] != null){
                    if(!board[row,col].isComplite){
                       return false; 
                    }
                }
            }   
        }
        return true;
    }
}
