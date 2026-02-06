using UnityEngine;
using UnityEngine.InputSystem;

public class SkyboxJoystickInput : MonoBehaviour
{
    [SerializeField] private InputActionReference cycle; // Vector2
    [SerializeField] private SkyboxManager manager;

    [Header("Behavior")]
    [SerializeField] private float threshold = 0.7f;     // wie stark ausschlagen
    [SerializeField] private float repeatCooldown = 0.35f; // Sekunden zwischen Wechseln

    private float _nextAllowedTime;

    private void OnEnable()
    {
        if (cycle != null) cycle.action.Enable();
    }

    private void OnDisable()
    {
        if (cycle != null) cycle.action.Disable();
    }

    private void Update()
    {
        if (cycle == null || manager == null) return;
        if (Time.time < _nextAllowedTime) return;

        Vector2 v = cycle.action.ReadValue<Vector2>();

        // Rechts = Next, Links = Previous
        if (v.x > threshold)
        {
            manager.Next();
            _nextAllowedTime = Time.time + repeatCooldown;
        }
        else if (v.x < -threshold)
        {
            manager.Previous();
            _nextAllowedTime = Time.time + repeatCooldown;
        }
    }
}