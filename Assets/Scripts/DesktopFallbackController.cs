using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class DesktopFallbackController : MonoBehaviour
{
    [Header("Rig References")]
    [SerializeField] private GameObject xrOriginRoot;
    [SerializeField] private GameObject desktopRigRoot;
    [SerializeField] private Camera desktopCamera;

    [Header("Skybox")]
    [SerializeField] private SkyboxManager skyboxManager;

    [Header("Mouse Look")]
    [SerializeField] private float sensitivity = 0.12f; // größer = schneller
    [SerializeField] private float pitchMin = -85f;
    [SerializeField] private float pitchMax = 85f;

    private bool _isDragging;
    private float _yaw;
    private float _pitch;

    private void Start()
    {
        bool xrPresent = XRSettings.isDeviceActive;

        if (xrOriginRoot != null) xrOriginRoot.SetActive(xrPresent);
        if (desktopRigRoot != null) desktopRigRoot.SetActive(!xrPresent);

        if (!xrPresent && desktopCamera != null)
        {
            Vector3 e = desktopCamera.transform.rotation.eulerAngles;
            _yaw = e.y;
            _pitch = NormalizePitch(e.x);
        }
    }

    private void Update()
    {
        if (XRSettings.isDeviceActive) return;
        if (desktopCamera == null) return;

        var mouse = Mouse.current;
        if (mouse == null) return;
        
        // Scroll Wheel: Next / Previous Skybox
        Vector2 scroll = mouse.scroll.ReadValue();

        if (scroll.y > 0.1f)
        {
            skyboxManager?.Next();
        }
        else if (scroll.y < -0.1f)
        {
            skyboxManager?.Previous();
        }

        // Right click toggles flooded
        if (mouse.rightButton.wasPressedThisFrame)
            skyboxManager?.ToggleFlooded();

        // Left button drag to look
        if (mouse.leftButton.wasPressedThisFrame) _isDragging = true;
        if (mouse.leftButton.wasReleasedThisFrame) _isDragging = false;

        if (_isDragging)
        {
            // delta is in pixels since last frame
            Vector2 delta = mouse.delta.ReadValue();

            _yaw += delta.x * sensitivity;
            _pitch -= delta.y * sensitivity;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

            desktopCamera.transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
    }

    private float NormalizePitch(float xEuler)
    {
        if (xEuler > 180f) xEuler -= 360f;
        return xEuler;
    }
}
