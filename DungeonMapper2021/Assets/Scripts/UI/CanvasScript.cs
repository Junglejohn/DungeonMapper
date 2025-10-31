using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasScript : MonoBehaviour
{

    [SerializeField]
    private Canvas _currentCanvas;
    private Canvas CurrentCanvas { get {
            if (_currentCanvas == null)
            {
                _currentCanvas.GetComponent<Canvas>();
            }
            return _currentCanvas; 
        } 
    }

    private bool _isActive = false;
    public bool IsActive { get { return _isActive; } private set { _isActive = value; } }

    public void ShowCanvas()
    {

        if (IsActive != true)
        {
            IsActive = true;
            O_ShowCanvas();
        }

        if (CurrentCanvas != null)
        {
            CurrentCanvas.enabled = true;
        }
    }

    protected virtual void O_ShowCanvas()
    {

    }

    public void HideCanvas()
    {
        if (CurrentCanvas != null)
        {
            CurrentCanvas.enabled = false;
        }

        if (IsActive)
        {
            IsActive = false;

            O_HideCanvas();
        }
    }

    protected virtual void O_HideCanvas()
    {

    }

}
