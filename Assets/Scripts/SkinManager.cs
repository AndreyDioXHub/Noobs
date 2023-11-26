using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class SkinManager : MonoBehaviour
{
    [SerializeField]
    private Material _playerMTL;
    [SerializeField]
    private Material _playerEditMTL;

    [SerializeField]
    private Renderer _body;

    [SerializeField]
    private Color _bodyColor;
    [SerializeField]
    private Color _hairSelectedColor;
    [SerializeField]
    private Color _hairEquipedColor;
    [SerializeField]
    private Color _tshirtColor;
    [SerializeField]
    private Color _pantsColor;
    [SerializeField]
    private Color _shoesColor;

    [SerializeField]
    private List<Texture2D> _faces = new List<Texture2D>();
    [SerializeField]
    private HairColorModel[] _elements;
    [SerializeField]
    private List<Material> _hairMaterials = new List<Material>();

    [SerializeField]
    private int _equipedHairIndex = 0;

    [SerializeField]
    private SkinsInfo _info = new SkinsInfo();

    [SerializeField]
    private Palitra _bodyPalitra;
    [SerializeField]
    private Palitra _hairPalitra;
    [SerializeField]
    private Palitra _tshirtPalitra;
    [SerializeField]
    private Palitra _pantsPalitra;
    [SerializeField]
    private Palitra _shoesPalitra;
    [SerializeField]
    private HairSelected _hairSelected;
    [SerializeField]
    private FaceSelect _faceSelect;

    [SerializeField]
    private List<Color> _colors = new List<Color>();


    void Start()
    {
        string infoJSON = PlayerPrefs.GetString("user_skin", "");

        if (string.IsNullOrEmpty(infoJSON))
        {
            for(int i=0; i<12; i++)
            {
                _info.bodyColors.Add(false);
                _info.hairColors.Add(false);
                _info.tshirtColors.Add(false);
                _info.pantsColors.Add(false);
                _info.shoesColors.Add(false);
            }


            int bodyindex = 0;// Random.Range(0, 12);
            _info.bodyColors[bodyindex] = true;

            int hairColorsindex = Random.Range(1, 12);
            _info.hairColors[hairColorsindex] = true;
            _hairSelectedColor = _colors[hairColorsindex];

            int tshirtindex = Random.Range(1, 12);
            _info.tshirtColors[tshirtindex] = true;

            int pantsindex = Random.Range(1, 12);
            _info.pantsColors[pantsindex] = true;

            int shoesindex = Random.Range(1, 12);
            _info.shoesColors[shoesindex] = true;

            for (int i = 0; i < 8; i++)
            {
                _info.hairs.Add(false);
            }

            for (int i = 0; i < 16; i++)
            {
                _info.faces.Add(false);
            }

            int hairsindex = Random.Range(0, 8);
            _info.hairs[hairsindex] = true;
            int faceindex = Random.Range(0, 16);
            _info.faces[faceindex] = true;

            _playerMTL.SetColor("_BodyColor", _colors[bodyindex]);
            _playerMTL.SetColor("_TshirtColor", _colors[tshirtindex]);
            _playerMTL.SetColor("_PentsColor", _colors[pantsindex]);
            _playerMTL.SetColor("_ShoesColor", _colors[shoesindex]);
            _playerMTL.SetTexture("_FaceMask", _faces[faceindex]);

            _info.eqyuipedSex = 0;

            _info.eqyuipedBody = bodyindex;
            _info.eqyuipedTshirt = tshirtindex;
            _info.eqyuipedPants = pantsindex;
            _info.eqyuipedShoes = shoesindex;
            _info.eqyuipedFace = faceindex;

            _info.eqyuipedHair = hairsindex;
            _info.eqyuipedHairColor = hairColorsindex;

            infoJSON = JsonConvert.SerializeObject(_info);
            PlayerPrefs.SetString("user_skin", infoJSON);

        }
        else
        {
            _info = JsonConvert.DeserializeObject<SkinsInfo>(infoJSON);
        }

        _bodyPalitra.Init(_info.bodyColors);
        _hairPalitra.Init(_info.hairColors);
        _tshirtPalitra.Init(_info.tshirtColors);
        _pantsPalitra.Init(_info.pantsColors);
        _shoesPalitra.Init(_info.shoesColors);

        _hairSelected.Init(_info.hairs);
        _faceSelect.Init(_info.faces);

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_info.eqyuipedHair);
        }

        _equipedHairIndex = _info.eqyuipedHair;


        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }


    }

    void Update()
    {
        
    }

    public void OpenSkinsPanel()
    {
        _playerEditMTL.SetColor("_BodyColor", _playerMTL.GetColor("_BodyColor"));
        _playerEditMTL.SetColor("_TshirtColor", _playerMTL.GetColor("_TshirtColor"));
        _playerEditMTL.SetColor("_PentsColor", _playerMTL.GetColor("_PentsColor"));
        _playerEditMTL.SetColor("_ShoesColor", _playerMTL.GetColor("_ShoesColor"));
        _playerEditMTL.SetTexture("_FaceMask", _playerMTL.GetTexture("_FaceMask"));
        _body.material = _playerEditMTL;
    }

    public void CloseSkinsPanel()
    {
        _body.material = _playerMTL;

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(_equipedHairIndex);
        }

        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairEquipedColor);
        }

    }


    public void SelectBodyColor(Color bodyColor)
    {
        _bodyColor = bodyColor;
        _playerEditMTL.SetColor("_BodyColor", _bodyColor);
    }

    public void EquipBodyColor(Color bodyColor)
    {
        _bodyColor = bodyColor;
        _playerMTL.SetColor("_BodyColor", _bodyColor);
    }

    public void SelectHairColor(Color color)
    {
        _hairSelectedColor = color;
        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }
    }

    public void EquipHairColor(Color color)
    {
        _hairEquipedColor = color;
    }

    public void SelectHair(int index)
    {

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i].Select(index);
        }
    }

    public void EquipHair(int index)
    {
        _equipedHairIndex = index;
    }
    public void SelectFace(int index)
    {
        _playerEditMTL.SetTexture("_FaceMask", _faces[index]);
    }

    public void EquipFace(int index)
    {
        _playerMTL.SetTexture("_FaceMask", _faces[index]);
    }

    public void SelectTShirtColor(Color color)
    {
        _tshirtColor = color;
        _playerEditMTL.SetColor("_TshirtColor", _tshirtColor);
    }

    public void EquipTShirtColor(Color color)
    {
        _tshirtColor = color;
        _playerMTL.SetColor("_TshirtColor", _tshirtColor);
    }

    public void SelectPantsColor(Color color)
    {
        _pantsColor = color;
        _playerEditMTL.SetColor("_PentsColor", _pantsColor);
    }

    public void EquipPantsColor(Color color)
    {
        _pantsColor = color;
        _playerMTL.SetColor("_PentsColor", _pantsColor);
    }

    public void SelectShoesColor(Color color)
    {
        _shoesColor = color;
        _playerEditMTL.SetColor("_ShoesColor", _shoesColor);
    }

    public void EquipShoesColor(Color color)
    {
        _shoesColor = color;
        _playerMTL.SetColor("_ShoesColor", _shoesColor);
    }

}


[Serializable]
public class SkinsInfo
{
    public int eqyuipedSex = 0;
    public int eqyuipedBody = 0;
    public int eqyuipedHair = 0;
    public int eqyuipedHairColor = 0;
    public int eqyuipedTshirt = 0;
    public int eqyuipedPants = 0;
    public int eqyuipedShoes = 0;
    public int eqyuipedFace = 0;

    public List<bool> bodyColors = new List<bool>();
    public List<bool> hairs = new List<bool>();
    public List<bool> hairColors = new List<bool>();
    public List<bool> tshirtColors = new List<bool>();
    public List<bool> pantsColors = new List<bool>();
    public List<bool> shoesColors = new List<bool>();
    public List<bool> faces = new List<bool>();
}
