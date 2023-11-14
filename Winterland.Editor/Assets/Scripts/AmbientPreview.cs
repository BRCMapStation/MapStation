using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AmbientPreview : MonoBehaviour
{
    public Color LightColor;
    public Color ShadowColor;
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalColor("LightColor", LightColor);
        Shader.SetGlobalColor("ShadowColor", ShadowColor);
    }
}
