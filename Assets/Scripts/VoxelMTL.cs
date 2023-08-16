using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMTL : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    private Color _color;
    private void Awake()
    {
        /*
        _materialPropertyBlock = new MaterialPropertyBlock();
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock.SetColor("_Color", _color);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);*/
    }

    public void Init(Color color)
    {
        //_color = color;
        _materialPropertyBlock = new MaterialPropertyBlock();
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock.SetColor("_Color", color);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
