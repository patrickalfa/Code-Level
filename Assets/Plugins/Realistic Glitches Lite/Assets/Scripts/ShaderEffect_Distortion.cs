using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderEffect_Distortion : MonoBehaviour
{
    public float intensity = 3;
    public float valueX = 0.5f;
    public Texture displacement;
    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/Distortion"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Intensity", intensity * 0.01f);
        material.SetFloat("_ValueX", valueX);
        material.SetTexture("_Texture", displacement);

        Graphics.Blit(source, destination, material);
    }
}