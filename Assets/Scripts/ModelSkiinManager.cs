using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSkiinManager : MonoBehaviour
{

    [SerializeField]
    private Material _playerMTL;

    [SerializeField]
    private List<GameObject> _males = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _females = new List<GameObject>();

    [SerializeField]
    private List<Renderer> _bodysMale = new List<Renderer>();
    [SerializeField]
    private List<Renderer> _bodysFemale = new List<Renderer>();


    [SerializeField]
    private List<HairInfo> _hairInfos = new List<HairInfo>();

    [SerializeField]
    private List<Material> _hairMaterials = new List<Material>();

    void Start()
    {
    }

    public void EquipSkin(SkinsInfo info)
    {
        if (_playerMTL == null)
        {
            _playerMTL = new Material(_bodysMale[0].material);

            foreach (var male in _bodysMale)
            {
                male.material = _playerMTL;
            }

            foreach (var famale in _bodysFemale)
            {
                famale.material = _playerMTL;
            }
        }

        foreach (var male in _males)
        {
            male.SetActive(info.eqyuipedSex == 0);
        }

        foreach (var famale in _females)
        {
            famale.SetActive(info.eqyuipedSex == 1);
        }

        _playerMTL.SetColor("_BodyColor", SkinManager.Colors[info.eqyuipedBody]);
        _playerMTL.SetColor("_TshirtColor", SkinManager.Colors[info.eqyuipedTshirt]);
        _playerMTL.SetColor("_PentsColor", SkinManager.Colors[info.eqyuipedPants]);
        _playerMTL.SetColor("_ShoesColor", SkinManager.Colors[info.eqyuipedShoes]);
        _playerMTL.SetTexture("_FaceMask", SkinManager.Faces[info.eqyuipedFace]);


        foreach(var hi in _hairInfos)
        {
            InitMaterial(hi.materialReference, hi.hairIndexes, hi.hairRenderers, hi.isInitedHairMaterial, out hi.isInitedHairMaterial);
        }

        for(int i=0; i< _hairInfos.Count; i++)
        {
            foreach(var hair in _hairInfos[i].hairs)
            {
                hair.SetActive(info.eqyuipedHair == i);
            }
        }
    }

    private void InitMaterial(Material reference, List<int> indexes, List<Renderer> renderers, bool inBool, out bool outBool )
    {
        if (!inBool)
        {
            Material mtl = new Material(reference);
            _hairMaterials.Add(mtl);
            foreach (var i in indexes)
            {
                foreach (var r in renderers)
                {
                    r.materials[i] = mtl;
                }
            }
        }

        outBool = true;
    }

    void Update()
    {
        
    }
}

[Serializable]
public class HairInfo
{
    public List<GameObject> hairs = new List<GameObject>();
    public List<Renderer> hairRenderers = new List<Renderer>();
    public List<int> hairIndexes = new List<int>();
    public Material materialReference;
    public bool isInitedHairMaterial;
}
