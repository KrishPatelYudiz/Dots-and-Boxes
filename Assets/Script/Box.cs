using System.Collections.Generic;
using System.Threading;
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
    List<Line> lines = new List<Line>();
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

    public void Initialize(Line top, Line bottom, Line left, Line right) {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
        lines.Add(top);
        lines.Add(bottom);
        lines.Add(left);
        lines.Add(right);
    }
    public int FilledLineCount(){
        int count = 0;
       foreach (var line in lines)
       {
        if (line.isSelected)
        {
            count++;
        }
       }     
       return count;
    }
    public void FillOneLine(){
        foreach (var line in lines){

            if (!line.isSelected)
            {
                line.isSelected = true;
                return;
            }
        }   
        }
}
