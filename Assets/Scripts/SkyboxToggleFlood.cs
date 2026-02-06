using UnityEngine;
using UnityEngine.InputSystem;

public class SkyboxToggleFlood : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleFlooded;
    [SerializeField] private SkyboxManager manager;

    private void OnEnable()
    {
        if (toggleFlooded != null)
        {
            toggleFlooded.action.performed += OnToggle;
            toggleFlooded.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleFlooded != null)
        {
            toggleFlooded.action.performed -= OnToggle;
            toggleFlooded.action.Disable();
        }
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
        if (manager != null)
            manager.ToggleFlooded();
    }
}