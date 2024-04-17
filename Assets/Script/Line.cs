
using UnityEngine;

public class Line : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite selectedLineSprite;  
    public bool _isSelected = false;
    public bool isSelected {
    get{
        return _isSelected;
    }
    set{
        _isSelected = value;
        spriteRenderer.sprite = selectedLineSprite;
    }
    }


    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
