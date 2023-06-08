using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControl;
using UnityEngine.Events;

public class ProxyInteractable : MonoBehaviour, IInteractable, ICPRable
{
    public UnityAction InteractAction { get; set; }

    public string GetDescription()
    {
        return "this is proxy";
    }

    public void Interact(CPRMainManager cprMainControl)
    {
        
    }

    public void Interact(PlayerInteraction player)
    {
        
    }
    public void Interact()
    {
        InteractAction?.Invoke();
    }
}
