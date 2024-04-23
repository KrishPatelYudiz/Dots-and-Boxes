
using System.Data;
using UnityEngine;

public class Line : MonoBehaviour
{

    public int row = 0;
    public int col = 0;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite selectedLineSprite;  
    public bool isSelected {
        get{
            if(BoardManager.lineData[row,col] == 1 ){
              return true;
            }
            return false;
        }
        set{
            BoardManager.lineData[row,col] = value? 1 : 0; 
            FillTheColor();
        }
    }

    public void FillTheColor(){
        spriteRenderer.sprite = selectedLineSprite;
        
    }
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
