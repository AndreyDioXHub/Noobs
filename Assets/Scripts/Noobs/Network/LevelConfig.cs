using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance { get; private set; }

    [Tooltip("Шаг смещения игровых уровней")]
    public float GridStep = 300;
    [Tooltip("Начальная Y-позиция игроков")]
    public float START_PLAYER_YPOS = 78;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null) { 
            Instance = this; 
        } else {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
