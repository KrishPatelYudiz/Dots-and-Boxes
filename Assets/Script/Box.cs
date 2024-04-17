
using UnityEngine;
public class Box : MonoBehaviour{

    public Line top;
    public Line bottom;
    public Line left;
    public Line right;
    
    public bool isComplite = false;

    public SpriteRenderer spriteRenderer;
    [SerializeField] Sprite _playerSprite;  
     public delegate void OnBoxComplet(Box box);
    public static event OnBoxComplet onBoxComplet;
    private void Awake() {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool ChackComplite(){
        if (top.isSelected && bottom.isSelected && left.isSelected && right.isSelected)
        {
            isComplite = true;
            onBoxComplet?.Invoke(this);
            return true;
        }
        return false;
    }
}