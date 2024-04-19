using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    Canvas canvas;
    public GameScreens screen;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
        onAwke();
    }
    protected virtual void onAwke(){
        
    }
    public virtual void ActivateScreen()
    {
        canvas.enabled = true;
    }

    public virtual void DeactivateScreen()
    {
        canvas.enabled = false;
    }

    public virtual void TakeInput()
    {

    }
}
