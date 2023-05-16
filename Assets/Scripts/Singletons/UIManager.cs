using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UserInterface;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI interactionText;
    public GameObject interactionUI;
    public GameObject[] helpUI;
    public GiveBreathUILogic giveBreathUI;

    // for Mouse 
    private readonly List<bool> _showingMouse = new();
    void Start()
    {
        if (_showingMouse.Count != 0)
        {
            _showingMouse.RemoveAt(_showingMouse.Count - 1);
        }
        MouseDisplayUpdate();
    }

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
