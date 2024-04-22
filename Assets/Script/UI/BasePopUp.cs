using UnityEngine;

public class BasePopUp : MonoBehaviour
{
    Canvas canvas;
    public GamePopUp popUp;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
        onAwke();
    }
    protected virtual void onAwke(){

    }
    public virtual void ActivatePopUp()
    {
        canvas.enabled = true;
    }

    public virtual void DeactivatePopUp()
    {
        canvas.enabled = false;
    }

  
}
