using UnityEngine;

public class BasePopUp : MonoBehaviour
{
    Canvas canvas;
    public GamePopUp popUp;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();

    }
    public virtual void ActivatePopUp()
    {
        canvas.enabled = true;
    }

    public virtual void DeactivatePopUp()
    {
        canvas.enabled = false;
    }

    public virtual void TackInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UiManager.instance.ClosePopUp();
        }
    }
}
