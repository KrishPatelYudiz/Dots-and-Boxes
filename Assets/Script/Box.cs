using UnityEngine;

public class Box : MonoBehaviour
{
    public Line Top;
    public Line Bottom;
    public Line Left;
    public Line Right;  

    public bool IsComplete { get; private set; } = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public delegate void BoxCompleteDelegate(Box box);
    public static event BoxCompleteDelegate OnBoxComplete;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public bool CheckComplete() {
        if (Top.isSelected && Bottom.isSelected && Left.isSelected && Right.isSelected) {
            IsComplete = true;
            OnBoxComplete?.Invoke(this);
            animator.SetTrigger("Start");
            AudioManager.instance.Play(SoundName.BoxFill);
            return true;
        }
        return false;
    }

    // Optional: Method to initialize the Box with its sides.
    public void Initialize(Line top, Line bottom, Line left, Line right) {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public void FillOneLine(){
        if (TryFillLine(Top))
            return;
        if (TryFillLine(Bottom))
            return;
        if (TryFillLine(Left))
            return;
        if (TryFillLine(Right))
            return;
    }
    bool TryFillLine(Line line){
        if (!line.isSelected)
        {
            line.isSelected = true;
            return true;
        }
        return false;
    }
}
