using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("CreateTexture")]
    private void CreateTexture()
    {
        int[,] map;
        int x = 1000;
        int y = 1000;

        var texture = new Texture2D(x, y, TextureFormat.ARGB32, false);

        string mapjsonPath = Path.Combine(Application.persistentDataPath, "map.json");
        string maprawjson = File.ReadAllText(mapjsonPath);
        map = JsonConvert.DeserializeObject<int[,]>(maprawjson);

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                texture.SetPixel(i, j, new Color(map[i, j], map[i, j], map[i, j], 1));
            }
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Path.Combine(Application.persistentDataPath, "map.png");
        File.WriteAllBytes(dirPath, bytes);
    }

    [ContextMenu("CreateTextureJSON")]
    private void CreateTextureJSON()
    {
        LayerMask waterNGroundMask = 0;
        int[,] map;
        int x = 1000;
        int y = 1000;

        map = new int[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector3 position = new Vector3(i-500, 100, j-500);
                RaycastHit hit;
                //, waterNGroundMask))
                if (Physics.Raycast(position, -Vector3.up * 1000, out hit, Mathf.Infinity) )
                {
                    // Debug.DrawRay(position, -Vector3.up * hit.distance, Color.yellow);
                    Debug.Log($"Did Hit {hit.collider.gameObject.layer}");
                    switch (hit.collider.gameObject.layer)
                    {
                        case 4:
                            map[i, j] = 0;
                            break;
                        case 3:
                            map[i, j] = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        string pos = JsonConvert.SerializeObject(map);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "map.json"), pos);
    }
}
