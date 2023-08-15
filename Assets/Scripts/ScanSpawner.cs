using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _voxelPrefab;
    [SerializeField]
    private int _dencity = 128;
    [SerializeField]
    private float _height = 1;
    [SerializeField]
    private int _pixelSpace = 16;
    [SerializeField]
    private float _sencity = 0.1f;

    [SerializeField]
    private List<Texture2D> _scan = new List<Texture2D>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [ContextMenu("Revers")]
    public void Revers()
    {
        _scan.Reverse();
    }
    
    [ContextMenu("Spawn")]
    public void Spawn()
    {
        StartCoroutine(SpawnCoroutine());        
    }

    IEnumerator SpawnCoroutine()
    {
        for (int k = 0; k < _scan.Count; k++)
        {
            for (int i = 0; i < _scan[k].width; i += _pixelSpace)
            {
                for (int j = 0; j < _scan[k].height; j += _pixelSpace)
                {
                    Color color = _scan[k].GetPixel(i, j);

                    if (color.r > _sencity)
                    {
                        yield return new WaitForEndOfFrame();
                        var go = Instantiate(_voxelPrefab);
                        go.transform.position = new Vector3(i / _pixelSpace, k * _height, j / _pixelSpace);
                        go.transform.localScale = new Vector3(1, _height, 1);
                        go.GetComponent<VoxelMTL>().Init(color);
                    }
                }
            }

        }

        /*for (int i = 0; i < _dencity; i++)
        {
            for (int j = 0; j < _dencity; j++)
            {
                for (int k = 0; k < _dencity; k++)
                {
                    yield return new WaitForEndOfFrame();
                    var go = Instantiate(_voxelPrefab);
                    go.transform.position = new Vector3(i * _space, k * _space, j * _space);
                }
            }
        }*/

    }

}
