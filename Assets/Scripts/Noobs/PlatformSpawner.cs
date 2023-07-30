using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _platformPrefab;

    void Start()
    {
        int k = 0;

        for(int y = 1; y < 4; y++)
        {
            for (int i = -20; i < 20; i++)
            {
                for (int j = -20; j < 20; j++)
                {
                    var go = Instantiate(_platformPrefab);

                    go.transform.position = new Vector3(i, y * 10, j);
                    go.name = $"platform {k}";
                    k++;
                }
            }
        }

    }

    void Update()
    {
        
    }
}
