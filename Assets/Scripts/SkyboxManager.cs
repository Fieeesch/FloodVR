using UnityEngine;

[System.Serializable]
public class SkyboxVariant
{
    public string name;
    public Material normal;
    public Material flooded;
}

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] private SkyboxVariant[] skyboxes;
    [SerializeField] private int startIndex = 0;
    [SerializeField] private bool startFlooded = false;

    private int _index;
    private bool _flooded;

    private void Start()
    {
        if (skyboxes == null || skyboxes.Length == 0) return;

        _index = Mathf.Clamp(startIndex, 0, skyboxes.Length - 1);
        _flooded = startFlooded;

        Apply();
    }

    public void Next()
    {
        if (skyboxes == null || skyboxes.Length == 0) return;

        _index = (_index + 1) % skyboxes.Length;
        Apply();
    }

    public void Previous()
    {
        if (skyboxes == null || skyboxes.Length == 0) return;

        _index = (_index - 1 + skyboxes.Length) % skyboxes.Length;
        Apply();
    }

    public void ToggleFlooded()
    {
        _flooded = !_flooded;
        Apply();
    }

    private void Apply()
    {
        var entry = skyboxes[_index];
        Material mat = _flooded ? entry.flooded : entry.normal;

        if (mat == null) return;

        RenderSettings.skybox = mat;
        DynamicGI.UpdateEnvironment();
    }
}