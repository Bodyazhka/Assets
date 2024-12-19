using UnityEngine;

[ExecuteAlways]
public class UpdateShaderScale : MonoBehaviour
{
    public Material material;

    void Update()
    {
        if (material != null)
        {
            Vector3 scale = transform.localScale;
            material.SetVector("_Scale", new Vector4(scale.x, scale.y, 1.0f, 1.0f));
        }
    }
}
