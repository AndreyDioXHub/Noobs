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
    private GameObject _male;
    [SerializeField]
    private GameObject _female;

    [SerializeField]
    private GameObject _maleActive;
    [SerializeField]
    private GameObject _femaleActive;

    [SerializeField]
    private Renderer _bodyMale;
    [SerializeField]
    private Renderer _bodyFemale;

    [SerializeField]
    private Color _bodyColor;
    [SerializeField]
    private Color _hairSelectedColor;
    [SerializeField]
    private int _hairSelectedIndex;
    [SerializeField]
    private Color _tshirtColor;
    [SerializeField]
    private Color _pantsColor;
    [SerializeField]
    private Color _shoesColor;
    [SerializeField]
    private int _faceSelectedIndex;

    [SerializeField]
    private List<Texture2D> _faces = new List<Texture2D>();
    [SerializeField]
    private HairColorModel[] _elementsMale;
    [SerializeField]
    private HairColorModel[] _elementsFemale;
    [SerializeField]
    private List<Material> _hairMaterials = new List<Material>();

    [SerializeField]
    private int _equipedHairIndex = 0;

    [SerializeField]
    private SkinsInfo _info = new SkinsInfo();

    [SerializeField]
    private SkinWindow _bodyPalitra;
    [SerializeField]
    private SkinWindow _hairPalitra;
    [SerializeField]
    private SkinWindow _tshirtPalitra;
    [SerializeField]
    private SkinWindow _pantsPalitra;
    [SerializeField]
    private SkinWindow _shoesPalitra;
    [SerializeField]
    private SkinWindow _hair;
    [SerializeField]
    private SkinWindow _face;

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

        EquipSex(_info.eqyuipedSex);

        _bodyPalitra.Init(_info.bodyColors, _info.eqyuipedBody);
        _bodyColor = _colors[_info.eqyuipedBody];

        _hairPalitra.Init(_info.hairColors, _info.eqyuipedHairColor);
        _hairSelectedColor = _colors[_info.eqyuipedHairColor];

        _tshirtPalitra.Init(_info.tshirtColors, _info.eqyuipedTshirt);
        _tshirtColor = _colors[_info.eqyuipedTshirt];

        _pantsPalitra.Init(_info.pantsColors, _info.eqyuipedPants);
        _pantsColor = _colors[_info.eqyuipedPants];

        _shoesPalitra.Init(_info.shoesColors, _info.eqyuipedShoes);
        _shoesColor = _colors[_info.eqyuipedShoes];

        _hair.Init(_info.hairs, _info.eqyuipedHair);
        _face.Init(_info.faces, _info.eqyuipedFace);
        _playerMTL.SetTexture("_FaceMask", _faces[_info.eqyuipedFace]);

        for (int i = 0; i < _elementsMale.Length; i++)
        {
            _elementsMale[i].Select(_info.eqyuipedHair);
        }

        for (int i = 0; i < _elementsFemale.Length; i++)
        {
            _elementsFemale[i].Select(_info.eqyuipedHair);
        }

        _equipedHairIndex = _info.eqyuipedHair;


        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }


    }

    [ContextMenu("ClearPrefs")]
    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
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
        _bodyMale.material = _playerEditMTL;
        _bodyFemale.material = _playerEditMTL;
    }

    public void CloseSkinsPanel()
    {
        _bodyMale.material = _playerMTL;
        _bodyFemale.material = _playerMTL;

        for (int i = 0; i < _elementsMale.Length; i++)
        {
            _elementsMale[i].Select(_equipedHairIndex);
        }
        for (int i = 0; i < _elementsFemale.Length; i++)
        {
            _elementsFemale[i].Select(_equipedHairIndex);
        }

        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }

        string infoJSON = JsonConvert.SerializeObject(_info);
        PlayerPrefs.SetString("user_skin", infoJSON);
    }

    public void EquipSex(int index)
    {
        _info.eqyuipedSex = index;

        _male.SetActive(index == 0);
        _female.SetActive(index == 1);

        _maleActive.SetActive(index == 0);
        _femaleActive.SetActive(index == 1);

    }

    public void BodyColorSelect(int index)
    {
        _bodyColor = _colors[index];
        _playerEditMTL.SetColor("_BodyColor", _bodyColor);

    }

    public void BodyColorEquip(int index)
    {
        _bodyColor = _colors[index];
        _playerMTL.SetColor("_BodyColor", _bodyColor);
        _info.eqyuipedBody = index;
    }

    public void BodyColorBuy(int index)
    {
        _info.bodyColors[index] = true;
    }

    public void HairColorSelect(int index)
    {
        _hairSelectedColor = _colors[index];

        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }
    }

    public void HairColorEquip(int index)
    {
        _hairSelectedColor = _colors[index];

        _info.eqyuipedHairColor = index;
    }

    public void HairColorBuy(int index)
    {
        _info.hairColors[index] = true;
    }

    public void HairSelect(int index)
    {
        _hairSelectedIndex = index;

        for (int i = 0; i < _elementsMale.Length; i++)
        {
            _elementsMale[i].Select(index);
        }

        for (int i = 0; i < _elementsFemale.Length; i++)
        {
            _elementsFemale[i].Select(index);
        }
    }

    public void HairBuy(int index)
    {
        _info.hairs[index] = true;
    }

    public void HairEquip(int index)
    {
        _equipedHairIndex = index;
        _info.eqyuipedHair = index;
    }

    public void FaceSelect(int index)
    {
        _faceSelectedIndex = index;
        _playerEditMTL.SetTexture("_FaceMask", _faces[index]);
    }

    public void FaceEquip(int index)
    {
        _playerMTL.SetTexture("_FaceMask", _faces[index]);
        _info.eqyuipedFace = index;
    }
    public void FaceBuy(int index)
    {
        _info.faces[index] = true;
    }



    public void TShirtColorSelect(int index)
    {
        _tshirtColor = _colors[index];
        _playerEditMTL.SetColor("_TshirtColor", _tshirtColor);
    }

    public void TShirtColorEquip(int index)
    {
        _tshirtColor = _colors[index];
        _playerMTL.SetColor("_TshirtColor", _tshirtColor);

        _info.eqyuipedTshirt = index;
    }
    public void TShirtColorBuy(int index)
    {
        _info.hairColors[index] = true;
    }

    public void PantsColorSelect(int index)
    {
        _pantsColor = _colors[index];
        _playerEditMTL.SetColor("_PentsColor", _pantsColor);
    }

    public void PantsColorEquip(int index)
    {
        _pantsColor = _colors[index];
        _playerMTL.SetColor("_PentsColor", _pantsColor);
        _info.eqyuipedPants = index;
    }

    public void PantColorBuy(int index)
    {
        _info.pantsColors[index] = true;
    }

    public void ShoesColorSelect(int index)
    {
        _shoesColor = _colors[index];
        _playerEditMTL.SetColor("_ShoesColor", _shoesColor);
    }

    public void ShoesColorEquip(int index)
    {
        _shoesColor = _colors[index];
        _playerMTL.SetColor("_ShoesColor", _shoesColor);
        _info.eqyuipedShoes = index;
    }
    public void ShoesColorBuy(int index)
    {
        _info.shoesColors[index] = true;
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
