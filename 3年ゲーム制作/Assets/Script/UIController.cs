using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public InputActionAsset inputActions;

    private void OnEnable()
    {
        var uiMap = inputActions.FindActionMap("UI");
        uiMap.Enable();
    }

    private void OnDisable()
    {
        var uiMap = inputActions.FindActionMap("UI");
        uiMap.Disable();
    }
}
