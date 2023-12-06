using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Newtonsoft.Json;
using Random = UnityEngine.Random;
using YG;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public UnityEvent<string> OnSkinInfoChanged = new UnityEvent<string>();
    public UnityEvent<int> OnHairChanged = new UnityEvent<int>();
    public UnityEvent OnHasPesel = new UnityEvent();
    public int HairSelectedIndex { get => _hairSelectedIndex; }
    public static List<Color> Colors
    {
        get
        {
            return Instance._colors;
        }
    }
    public static List<Texture2D> Faces
    {
        get
        {
            return Instance._faces;
        }
    }

    public static SkinsInfo Info
    {
        get
        {
            return Instance._info;
        }
    }

    private Material _playerMTL;
    private Material _playerEditMTL;

    [SerializeField]
    private List<GameObject> _males = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _females = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _pomni = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _menuComponents = new List<GameObject>();


    [SerializeField]
    private GameObject _maleActive;
    [SerializeField]
    private GameObject _femaleActive;
    [SerializeField]
    private GameObject _pomniActive;
    [SerializeField]
    private GameObject _buySexButton;

    [SerializeField]
    private List<Renderer> _bodysMale = new List<Renderer>();
    [SerializeField]
    private List<Renderer> _bodysFemale = new List<Renderer>();

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

    private void Awake()
    {
        Instance = this; 
        OnHairChanged = new UnityEvent<int>();
    }

    void Start()
    {
        Debug.Log("delegates GetSkinInfo Start");
        StartCoroutine(OnSceeneLoadedCoroutine());
    }

    IEnumerator OnSceeneLoadedCoroutine()
    {
        yield return null;// new WaitForSeconds(0.1f);

        if (SceneManager.GetActiveScene().name.Equals("NoobLevelObstacleCourseNetwork"))
        {
            while(RobloxController.Instance == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        Load();
    }

    public void Load()
    {
        PlayerSave.Instance.ExecuteMyDelegateInQueue(GetSkinInfo);
    }

    private void OnDestroy()
    {
        Instance = null;
        _info = null;
    }

    public void GetSkinInfo()
    {
        Debug.Log("delegates GetSkinInfo");

        string infoJSON = YandexGame.savesData.USER_SKIN_KEY; 

        _playerMTL = new Material(_bodysMale[0].material);
        _playerEditMTL = new Material(_bodysMale[0].material);

        if (string.IsNullOrEmpty(infoJSON))
        {
            for (int i = 0; i < 12; i++)
            {
                _info.bodyColors.Add(false);
                _info.hairColors.Add(false);
                _info.tshirtColors.Add(false);
                _info.pantsColors.Add(false);
                _info.shoesColors.Add(false);
            }

            _info.sexes = new List<bool>() { true, true, false };
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


            _info.eqyuipedSex = 0;

            _info.eqyuipedBody = bodyindex;
            _info.eqyuipedTshirt = tshirtindex;
            _info.eqyuipedPants = pantsindex;
            _info.eqyuipedShoes = shoesindex;
            _info.eqyuipedFace = faceindex;

            _info.eqyuipedHair = hairsindex;
            _info.eqyuipedHairColor = hairColorsindex;

            infoJSON = JsonConvert.SerializeObject(_info);

            YandexGame.savesData.USER_SKIN_KEY = infoJSON;
            PlayerSave.Instance.Save();

        }
        else
        {
            _info = JsonConvert.DeserializeObject<SkinsInfo>(infoJSON);
            OnSkinInfoChanged?.Invoke(infoJSON);
        }

        foreach (var maleRenderer in _bodysMale)
        {
            maleRenderer.material = _playerMTL;
        }

        foreach (var femaleRenderer in _bodysFemale)
        {
            femaleRenderer.material = _playerMTL;
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

        _hairSelectedIndex = _info.eqyuipedHair;

        OnHairChanged?.Invoke(_info.eqyuipedHair);

        if (_info.hairs[4])
        {
            OnHasPesel?.Invoke();
        }

        _equipedHairIndex = _info.eqyuipedHair;


        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }

        _playerMTL.SetColor("_BodyColor", _colors[_info.eqyuipedBody]);
        _playerMTL.SetColor("_TshirtColor", _colors[_info.eqyuipedTshirt]);
        _playerMTL.SetColor("_PentsColor", _colors[_info.eqyuipedPants]);
        _playerMTL.SetColor("_ShoesColor", _colors[_info.eqyuipedShoes]);
        _playerMTL.SetTexture("_FaceMask", _faces[_info.eqyuipedFace]);

        _playerEditMTL.SetColor("_BodyColor", _colors[_info.eqyuipedBody]);
        _playerEditMTL.SetColor("_TshirtColor", _colors[_info.eqyuipedTshirt]);
        _playerEditMTL.SetColor("_PentsColor", _colors[_info.eqyuipedPants]);
        _playerEditMTL.SetColor("_ShoesColor", _colors[_info.eqyuipedShoes]);
        _playerEditMTL.SetTexture("_FaceMask", _faces[_info.eqyuipedFace]);

    }

    [ContextMenu("ClearPrefs")]
    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void Update()
    {
        
    }

    public void EquipPesel()
    {
        _equipedHairIndex = 4;
        _info.eqyuipedHair = 4;
        _info.hairs[4] = true;
        _hair.Init(_info.hairs, _info.eqyuipedHair);
        OnHairChanged?.Invoke(_equipedHairIndex);
        CloseSkinsPanel();
        string infoJSON = JsonConvert.SerializeObject(_info);
        YandexGame.savesData.USER_SKIN_KEY = infoJSON;
        PlayerSave.Instance.Save();

    }

    public void OpenSkinsPanel()
    {
        _playerEditMTL.SetColor("_BodyColor", _playerMTL.GetColor("_BodyColor"));
        _playerEditMTL.SetColor("_TshirtColor", _playerMTL.GetColor("_TshirtColor"));
        _playerEditMTL.SetColor("_PentsColor", _playerMTL.GetColor("_PentsColor"));
        _playerEditMTL.SetColor("_ShoesColor", _playerMTL.GetColor("_ShoesColor"));
        _playerEditMTL.SetTexture("_FaceMask", _playerMTL.GetTexture("_FaceMask"));

        foreach (var male in _bodysMale)
        {
            male.material = _playerEditMTL;
        }

        foreach (var female in _bodysFemale)
        {
            female.material = _playerEditMTL;
        }
    }

    public void CloseSkinsPanel()
    {
        foreach (var male in _bodysMale)
        {
            male.material = _playerMTL;
        }

        foreach (var female in _bodysFemale)
        {
            female.material = _playerMTL;
        }

        OnHairChanged?.Invoke(_equipedHairIndex);
        _hairSelectedColor = _colors[_info.eqyuipedHairColor];
        
        for (int i = 0; i < _hairMaterials.Count; i++)
        {
            _hairMaterials[i].SetColor("_MainColor", _hairSelectedColor);
        }

        string infoJSON = JsonConvert.SerializeObject(_info);

        YandexGame.savesData.USER_SKIN_KEY = infoJSON;
        PlayerSave.Instance.Save();
        OnSkinInfoChanged?.Invoke(infoJSON);
    }

    public void CheckAvaleble(int index)
    {
        if (_info.sexes[index])
        {
            _info.eqyuipedSex = index;

            _buySexButton.SetActive(false);

            foreach (var male in _males)
            {
                male.SetActive(index == 0);
            }

            foreach (var female in _females)
            {
                female.SetActive(index == 1);
            }

            foreach (var pomni in _pomni)
            {
                pomni.SetActive(index == 2);
            }

            _maleActive.SetActive(index == 0);
            _femaleActive.SetActive(index == 1);
            _pomniActive.SetActive(index == 2);
        }
        else
        {
            _buySexButton.SetActive(true);

            foreach (var male in _males)
            {
                male.SetActive(index == 0);
            }

            foreach (var female in _females)
            {
                female.SetActive(index == 1);
            }

            foreach (var pomni in _pomni)
            {
                pomni.SetActive(index == 2);
            }

            _maleActive.SetActive(index == 0);
            _femaleActive.SetActive(index == 1);
            _pomniActive.SetActive(index == 2);
        }
    }

    public void SetInfo(string json)
    {
        _info = JsonConvert.DeserializeObject<SkinsInfo>(json);
    }

    public void CloseSex()
    {
        EquipSex(_info.eqyuipedSex);
    }

    public void EquipSex(int index)
    {
        _info.eqyuipedSex = index;
        _buySexButton.SetActive(false);

        foreach (var male in _males)
        {
            male.SetActive(index == 0);
        }

        foreach(var female in _females)
        {
            female.SetActive(index == 1);
        }

        foreach(var pomni in _pomni)
        {
            pomni.SetActive(index == 2);
        }

        foreach(var menuComponent in _menuComponents)
        {
            menuComponent.SetActive(index != 2);
        }

        _maleActive.SetActive(index == 0);
        _femaleActive.SetActive(index == 1);
        _pomniActive.SetActive(index == 2);
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

        OnHairChanged?.Invoke(index);
    }

    public void HairBuy(int index)
    {
        _info.hairs[index] = true;

        if (_info.hairs[4])
        {
            OnHasPesel?.Invoke();
        }
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

    public List<bool> sexes = new List<bool>();
    public List<bool> bodyColors = new List<bool>();
    public List<bool> hairs = new List<bool>();
    public List<bool> hairColors = new List<bool>();
    public List<bool> tshirtColors = new List<bool>();
    public List<bool> pantsColors = new List<bool>();
    public List<bool> shoesColors = new List<bool>();
    public List<bool> faces = new List<bool>();
}
