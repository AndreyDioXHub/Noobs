using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ocean : MonoBehaviour
{
    public static Ocean Instance;

    [SerializeField]
    private WaveTimeController _waveTimeController;

    [SerializeField]
    private Transform _playerPosition;
    [SerializeField]
    private Transform _cameraPosition;
    [SerializeField]
    private List<GameObject> _oceanCells = new List<GameObject>();

    [SerializeField]
    private Transform _oceanCellsParent;

    [SerializeField]
    private GerstnerData _waveA;
    [SerializeField]
    private GerstnerData _waveB;
    [SerializeField]
    private GerstnerData _waveC;
    [SerializeField]
    private float _wavesOffcetY = 8;

    [SerializeField]
    private int _missFrame = 10;
    [SerializeField]
    private int _missFrameCur = 0;

    [SerializeField]
    private GameObject _activeOceanCell;

    [SerializeField]
    private Material _oceanMTL;

    public Transform PlayerPosition { get => _playerPosition; set => _playerPosition = value; }
    public Transform CameraPosition { get => _cameraPosition; set => _cameraPosition = value; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        gameObject.SetActive(false);
    }


    void Start()
    {

    }

    public void ActivateNewOceanCell(GameObject activeOceanCell)
    {
        _activeOceanCell.SetActive(false);
        _activeOceanCell = activeOceanCell;
        _activeOceanCell.SetActive(true);

    }


    [ContextMenu("ff")]
    public void ff()
    {
        int k = 0;

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                GameObject cellGO = new GameObject();

                cellGO.name = _oceanCells[k].name;
                cellGO.transform.position = new Vector3(960 - 128 * j, 50, -960 + 128 * i);
                cellGO.transform.localScale = new Vector3(1, 1, 1);
                BoxCollider bc = cellGO.AddComponent<BoxCollider>();
                bc.size = new Vector3(128, 200, 128);
                bc.isTrigger = true;
                OceanCell oc = cellGO.AddComponent<OceanCell>();
                oc.Init(_oceanCells[k]);
                _oceanCells[k].transform.SetParent(cellGO.transform);

                k++;
            }
        }

        /*for (int i = 0; i < _cellsTransform.childCount; i++)
        {
            _oceanCells.Add(_cellsTransform.GetChild(i).gameObject);
        }*/

    }

    void Update()
    {
        float delta = UnderWaterVolume.Instance.Cross.y - _cameraPosition.position.y;
        delta = Mathf.Abs(delta);
        delta = delta > 4 ? 4 : delta;
        _oceanMTL.SetFloat("_WaterProjectionPower", delta / 4);


        delta = UnderWaterVolume.Instance.Cross.y - _cameraPosition.position.y;
        //Debug.Log(delta);
        
        delta = delta > 20 ? 20 : delta;
        delta = delta < 0 ? 0 : delta;

        _oceanMTL.SetFloat("_Deep", delta / 20);

        /*
        _missFrameCur++;

        if (_missFrameCur > _missFrame)
        {
            float x = (_playerPosition.position.x - (_playerPosition.position.x - 64) % 128);
            float z = (_playerPosition.position.z - (_playerPosition.position.z - 64) % 128);
            string cellName = $"Ocean Cell ({x}) ({z})";

            foreach (var oc in _oceanCells)
            {
                oc.SetActive(false);
            }

            var ocell = _oceanCells.Find(o => o.name.Equals(cellName));

            ocell.SetActive(true);
        }*/
    }

    private void OnDrawGizmos()
    {

    }

    public float GetHeightAtPosition(Vector3 position)
    {
        float time = Time.timeSinceLevelLoad;
        Vector3 currentPosition = WaterPosition(position);

        for (int i = 0; i < 3; i++)
        {
            Vector3 diff = new Vector3(position.x - currentPosition.x, 0, position.z - currentPosition.z);
            currentPosition = WaterPosition(diff);
        }

        return currentPosition.y;
    }

    public Vector3 WaterPosition(Vector3 position, float delta = 0)
    {
        float steepnessA = _waveA.steepness;
        float steepnessB = _waveB.steepness;
        float steepnessC = _waveC.steepness;

        if(delta != 0)
        {
            float amplitudeA = _waveA.steepness / (2 * Mathf.PI / _waveA.wavelength);
            float amplitudeB = _waveB.steepness / (2 * Mathf.PI / _waveB.wavelength);
            float amplitudeC = _waveC.steepness / (2 * Mathf.PI / _waveC.wavelength);

            float amplitudeSum = (amplitudeA + amplitudeB + amplitudeC) / 3;

            if (delta > amplitudeSum)
            {
                //Debug.Log($"{ delta} {amplitudeSum} {_wavesOffcetY - delta}");
                return position;
            }

            float amplitudeDif = amplitudeSum - delta;

            steepnessA = _waveA.steepness * amplitudeDif / amplitudeSum;
            steepnessB = _waveB.steepness * amplitudeDif / amplitudeSum;
            steepnessC = _waveC.steepness * amplitudeDif / amplitudeSum;

            //Debug.Log($"{ amplitudeDif / amplitudeSum} {steepnessA} {steepnessB} {steepnessC}");
        }

        Vector3 posA = GerstnerWavePointPosition(position, _waveTimeController.TimeG, _waveA, steepnessA);
        Vector3 posB = GerstnerWavePointPosition(position, _waveTimeController.TimeG, _waveB, steepnessB);
        Vector3 posC = GerstnerWavePointPosition(position, _waveTimeController.TimeG, _waveC, steepnessC);

        Vector3 wavesSum = (posA + posB + posC) / 3;

        return new Vector3(position.x + wavesSum.x, wavesSum.y + _wavesOffcetY - delta, position.z + wavesSum.z);
    }

    public Vector3 WaterNormalInPosition(Vector3 position)
    {
        List<Vector3> pacA = GerstnerWavePointTangentNBinormal(position, _waveTimeController.TimeG, _waveA);
        List<Vector3> pacB = GerstnerWavePointTangentNBinormal(position, _waveTimeController.TimeG, _waveB);
        List<Vector3> pacC = GerstnerWavePointTangentNBinormal(position, _waveTimeController.TimeG, _waveC);

        Vector3 tangentSum = pacA[0] + pacB[0] + pacC[0];
        Vector3 binormalSumm = pacA[1] + pacB[1] + pacC[1];

        Vector3 normal = Vector3.Cross(tangentSum.normalized, binormalSumm.normalized).normalized; 

        //Debug.DrawRay(WaterPosition(position), normal, Color.red);

        return normal;
    }

    public Vector3 GerstnerWavePointPosition(Vector3 position, float time, GerstnerData data, float steepness)
    {
        float k = 2 * Mathf.PI / data.wavelength;

        float a = steepness / k;
        //Debug.Log(a);
        Vector2 d = new Vector2(data.direction.x, data.direction.y);
        d.Normalize();
        // Debug.Log(data.speed);
        float f = k * (Vector2.Dot(d, new Vector2(position.x, position.z)) - data.speed * time);

        Vector3 wavePosition = new Vector3(d.x * a * (float)Math.Cos(f), a * (float)Math.Sin(f), d.y * a * (float)Math.Cos(f));


        return wavePosition;
    }

    public List<Vector3> GerstnerWavePointTangentNBinormal(Vector3 position, float time, GerstnerData data)
    {
        float k = 2 * Mathf.PI / data.wavelength;

        float a = data.steepness / k;
        Vector2 d = new Vector2(data.direction.x, data.direction.y);
        d.Normalize();
        float f = k * (Vector2.Dot(d, new Vector2(position.x, position.z)) - data.speed * time);

        Vector3 tangent = new Vector3(1 - d.x * d.x * (data.steepness * (float)Math.Sin(f)), d.x * (data.steepness * (float)Math.Sin(f)), -d.x * d.y * (data.steepness * (float)Math.Sin(f)));

        Vector3 normal = new Vector3(-d.x * d.y * (data.steepness * (float)Math.Sin(f)), d.y * (data.steepness * (float)Math.Sin(f)), 1 - d.y * d.y * (data.steepness * (float)Math.Sin(f)));

        List<Vector3> package = new List<Vector3>();

        package.Add(tangent);
        package.Add(normal);

        return package;
    }

}

[Serializable]
public class GerstnerData
{
    public Vector2 direction;
    public float steepness;
    public float wavelength;
    public float speed;
}

/*
[SerializeField]
private Transform _lod0;
[SerializeField]
private Transform _lod1;
[SerializeField]
private Transform _lod2;
[SerializeField]
private Transform _lod3;

[SerializeField]
private List<GameObject> ls0 = new List<GameObject>();
[SerializeField]
private List<GameObject> ls1 = new List<GameObject>();
[SerializeField]
private List<GameObject> ls2 = new List<GameObject>();
[SerializeField]
private List<GameObject> ls3 = new List<GameObject>();*/

/*
[ContextMenu("CombineOcean")]
public void CombineOcean()
{
    for (int i = 0; i< _lod0.childCount; i++)
    {
        string nameindex = "000";

        if (i < 10)
        {
            nameindex = $"00{i}";
        }
        else if (i < 100)
        {
            nameindex = $"0{i}";
        }
        else if (i < 1000)
        {
            nameindex = $"{i}";
        }

        var l0 = _lod0.GetChild(i).gameObject;
        l0.name = $"Ocean Part LOD0 ({nameindex})";

        var l1 = _lod1.GetChild(i).gameObject;
        l1.name = $"Ocean Part LOD1 ({nameindex})";

        var l2 = _lod2.GetChild(i).gameObject;
        l2.name = $"Ocean Part LOD2 ({nameindex})";

        var l3 = _lod3.GetChild(i).gameObject;
        l3.name = $"Ocean Part LOD3 ({nameindex})";

        ls0.Add(l0);
        ls1.Add(l1);
        ls2.Add(l2);
        ls3.Add(l3);
    }

    for(int i=0; i< ls0.Count; i++)
    {
        string nameindex = "000";

        if (i < 10)
        {
            nameindex = $"00{i}";
        }
        else if (i < 100)
        {
            nameindex = $"0{i}";
        }
        else if (i < 1000)
        {
            nameindex = $"{i}";
        }

        var lodGO = new GameObject();
        lodGO.name = $"Ocean Part ({nameindex})";
        lodGO.transform.SetParent(transform);

        LODGroup lodGr = lodGO.AddComponent<LODGroup>();

        ls0[i].transform.SetParent(lodGO.transform);
        ls0[i].transform.position = Vector3.zero;

        ls1[i].transform.SetParent(lodGO.transform);
        ls1[i].transform.position = Vector3.zero;

        ls2[i].transform.SetParent(lodGO.transform);
        ls2[i].transform.position = Vector3.zero;

        ls3[i].transform.SetParent(lodGO.transform);
        ls3[i].transform.position = Vector3.zero;

        LOD lod0 = new LOD();
        List<Renderer> rend0 = new List<Renderer>();
        rend0.Add(ls0[i].GetComponent<Renderer>());
        lod0.renderers = rend0.ToArray();
        lod0.screenRelativeTransitionHeight = 0.6f;

        LOD lod1 = new LOD();
        List<Renderer> rend1 = new List<Renderer>();
        rend1.Add(ls1[i].GetComponent<Renderer>());
        lod1.renderers = rend1.ToArray();
        lod1.screenRelativeTransitionHeight = 0.4f;

        LOD lod2 = new LOD();
        List<Renderer> rend2 = new List<Renderer>();
        rend2.Add(ls2[i].GetComponent<Renderer>());
        lod2.renderers = rend2.ToArray();
        lod2.screenRelativeTransitionHeight = 0.3f;

        LOD lod3 = new LOD();
        List<Renderer> rend3 = new List<Renderer>();
        rend3.Add(ls3[i].GetComponent<Renderer>());
        lod3.renderers = rend3.ToArray();
        lod3.screenRelativeTransitionHeight = 0.05f;

        List<LOD> lods = new List<LOD>();
        lods.Add(lod0);
        lods.Add(lod1);
        lods.Add(lod2);
        lods.Add(lod3);
        lodGr.SetLODs(lods.ToArray());
        lodGr.RecalculateBounds();
    }
}*/