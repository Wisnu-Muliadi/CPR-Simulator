using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI interactionText;
    public GameObject interactionUI;
    public GameObject[] helpUI;

    // for Mouse 
    private readonly List<bool> _showingMouse = new();

    public void MouseDisplayAdd(bool add)
    {
        if (add)
        {
            _showingMouse.Add(true);
        }
        else
        {
            if (_showingMouse.Count != 0)
            {
                _showingMouse.RemoveAt(_showingMouse.Count - 1);
            }
        }
        MouseDisplayUpdate();
    }
    public void MouseDisplayUpdate()
    {
        if (_showingMouse.Count != 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus) MouseDisplayUpdate();
    }
}
