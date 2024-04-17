
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
    [SerializeField]LayerMask _lineLayer;
    [SerializeField]BoardGenerator boardGenerator;           
    Box[,] board;
    public delegate void OnTurnChange();
    public static event OnTurnChange onTurnChange;
                 
    private void Start() {
       board =  boardGenerator.MakeNewBoard(board_map);
    }
    
    private void Update() {
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
