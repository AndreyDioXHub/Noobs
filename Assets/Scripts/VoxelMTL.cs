using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMTL : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        
       
    }

    public void Init(Color color)
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock.SetColor("_Color", color);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
