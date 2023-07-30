using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGlobals : MonoBehaviour
{
    [Header("Объекты, которые надо ликновать с персонажем")]
    [SerializeField]
    internal Transform PlayerWaterAnchor;

    public static CharacterGlobals Instance;

    // Start is called before the first frame update
    void Awake() {
        Instance = this;
    }

}
