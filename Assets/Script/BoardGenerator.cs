using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private GameObject _BoxPrefab;

    private float _lineSize = 0.83f;  
    private Line[,] gridLines;  
    int[,] lineList;
    public float offset = 0;
    float HalfBoxSize => 2.5f - offset;

    bool[,] board;
    public Box[,] board_box;


    public Box[,] MakeNewBoard(bool[,] board,out int[,] lineData)
    {
        this.board = board;
        MakeLineTable();
        InitializeBoard();  
        MakeBoxs();
        lineData = this.lineList; 
        return board_box;
    }
    void MakeBoxs(){
         int boardLength = board.GetLength(0);
         board_box = new Box[boardLength,boardLength];
         for(int row = 0; row < boardLength;row++)
        {
            for(int col = 0;col <boardLength;col++){
                if (board[row,col])
                {
                  GameObject boxObj = Instantiate(_BoxPrefab,transform);
                  boxObj.name = "(" + row + "_" + col +")";
                  Box box = boxObj.GetComponent<Box>();
                  box.Top =  gridLines[row*2,col] ;
                  box.Bottom =  gridLines[2 * row + 2,col]  ;
                  box.Left =  gridLines[2 * row + 1,col]  ;
                  box.Right =  gridLines[2 * row + 1,col +1] ;
                   
                  boxObj.transform.position = new Vector3(box.Top.transform.position.x,box.Left.transform.position.y);
                  
                  board_box[row,col] = box;
                }
                
            }
        }
    }
    void MakeLineTable(){
        int boardLength = board.GetLength(0);
        int rows = 2*board.GetLength(0)+1;  
        int cols = board.GetLength(0)+1;  

        lineList = new int[rows,cols];
        for (int row = 0; row < rows; row++)
            for (int col = 0; col < cols; col++)
                lineList[row,col] = -1;

        for(int row = 0; row < boardLength;row++)
        {
            for(int col = 0;col <boardLength;col++){
                if (board[row,col])
                {
                    
                    lineList[row*2,col] = 0;
                    
                    
                    lineList[2 * row + 2,col] = 0;
                 
                    
                    lineList[2 * row + 1,col] = 0;
                 
                    
                    lineList[2 * row + 1,col +1] = 0;
                }
                
            }
        }
        
        for(int row = 0; row < boardLength;row++)
        {
            for(int col = 0;col <boardLength;col++){
                if (!board[row,col])
                {
                    
                    lineList[row*2,col] = lineList[row*2,col] == 0 ? 1 : -1;
                    
                    
                    lineList[2 * row + 2,col] = lineList[2 * row + 2,col] == 0 ? 1 : -1;
                 
                    
                    lineList[2 * row + 1,col] = lineList[2 * row + 1,col] == 0 ? 1 : -1;
                 
                    
                    lineList[2 * row + 1,col +1] = lineList[2 * row + 1,col +1] == 0 ? 1 : -1;
                }
                
            }
        }
        for (int ind = 0; ind < rows; ind++)
        {
            if (ind % 2 == 1 )
            {    
            lineList[ind,0] = lineList[ind,0] == 0 ? 1 : -1;            
            lineList[ind,cols-1] = lineList[ind,cols-1] == 0 ? 1 : -1;            
            }
        }
        for (int ind = 0; ind < cols; ind++)
        {
            lineList[0,ind] = lineList[0,ind] == 0 ? 1 : -1;            
            lineList[rows-1,ind] = lineList[rows-1,ind] == 0 ? 1 : -1;            
            
        }
    }
    public void PrintList(List<List<int>> list)
    {
        
        foreach (List<int> sublist in list)
        {
            
            string s ="";
            foreach (var item in sublist)
            {
                s += item.ToString();
            }
            print(s);
        }
    }
    
    void InitializeBoard()
    {
        int rows = 2*board.GetLength(0)+1;  
        int cols = board.GetLength(0)+1;  
        gridLines = new Line[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            
            for (int column = 0; column < cols ; column++)
            {
                if (lineList[row,column] > -1)
                {     
                    GameObject line = Instantiate(_linePrefab, transform);
                    line.name = $"({row}, {column})";  
                    gridLines[row,column] = line.GetComponent<Line>();
                    gridLines[row,column].row = row;
                    gridLines[row,column].col = column;
                     
                    if (row % 2 == 0)
                    {   
                        line.transform.localPosition = new Vector3((column * _lineSize) + _lineSize / 2 - HalfBoxSize, -((row / 2) * _lineSize) + HalfBoxSize);
                        line.transform.localEulerAngles = new Vector3(0, 0, 90);
                    }
                    else
                    {
                        line.transform.localPosition = new Vector3((column * _lineSize) - HalfBoxSize, -((row / 2) * _lineSize) - _lineSize / 2 + HalfBoxSize);
                    }
                    if (lineList[row,column] == 1)
                    {
                        line.SetActive(false);
                    }
                }
        
            }
        }
    }

    
    public void ClearBoard()
    {
        foreach (var line in gridLines)
        {
            Destroy(line);
        }
    }
}
