using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterVolume : MonoBehaviour
{
    public static UnderWaterVolume Instance;

    public bool PlayerIsUnderWater { get => _playerIsUnderWater; }

    public Vector3 Cross { get => _cross; }
    public Transform PlayerTransform { get => _playerTransform; set => _playerTransform = value; }
    public Transform CameraTransform { get => _cameraTransform; set => _cameraTransform = value; }

    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private MeshFilter _meshFilter;

    [SerializeField]
    private List<int> _pointsToVerticies = new List<int>();
    [SerializeField]
    private List<PolygonPoints> _poligons = new List<PolygonPoints>();

    private List<Vector3> _polygon = new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
    private List<Vector3> _waterSurface = new List<Vector3>();
    private List<Vector3> _points = new List<Vector3>();

    private Vector3 _cellPosition;
    private Vector3 _cross;

    [SerializeField]
    private int _missFrame = 10;
    private int _missFrameCur = 0;

    [SerializeField]
    private bool _playerIsUnderWater;
    [SerializeField]
    private bool _drawMarkers;

    [SerializeField]
    private Material _underWaterMTL;

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
        gameObject.SetActive(false);    //Disable and wait player
    }

    void Start()
    {

        for (int i = 0; i < 50; i++)
        {
            _waterSurface.Add(Vector3.zero);
        }
    }


    void FixedUpdate()
    {
        _missFrameCur++;

        if (_missFrameCur > _missFrame)
        {
            _missFrameCur = 0;

            _cellPosition = new Vector3(_playerTransform.position.x - _playerTransform.position.x % 2, _playerTransform.position.y - _playerTransform.position.y % 2, _playerTransform.position.z - _playerTransform.position.z % 2);

            _waterSurface[0] = new Vector3(_cellPosition.x - 4, _cellPosition.y, _cellPosition.z - 4);
            _waterSurface[1] = new Vector3(_cellPosition.x - 2, _cellPosition.y, _cellPosition.z - 4);
            _waterSurface[2] = new Vector3(_cellPosition.x, _cellPosition.y, _cellPosition.z - 4);
            _waterSurface[3] = new Vector3(_cellPosition.x + 2, _cellPosition.y, _cellPosition.z - 4);
            _waterSurface[4] = new Vector3(_cellPosition.x + 4, _cellPosition.y, _cellPosition.z - 4);

            _waterSurface[5] = new Vector3(_cellPosition.x - 4, _cellPosition.y, _cellPosition.z - 2);
            _waterSurface[6] = new Vector3(_cellPosition.x - 2, _cellPosition.y, _cellPosition.z - 2);
            _waterSurface[7] = new Vector3(_cellPosition.x, _cellPosition.y, _cellPosition.z - 2);
            _waterSurface[8] = new Vector3(_cellPosition.x + 2, _cellPosition.y, _cellPosition.z - 2);
            _waterSurface[9] = new Vector3(_cellPosition.x + 4, _cellPosition.y, _cellPosition.z - 2);

            _waterSurface[10] = new Vector3(_cellPosition.x - 4, _cellPosition.y, _cellPosition.z);
            _waterSurface[11] = new Vector3(_cellPosition.x - 2, _cellPosition.y, _cellPosition.z);
            _waterSurface[12] = new Vector3(_cellPosition.x, _cellPosition.y, _cellPosition.z);
            _waterSurface[13] = new Vector3(_cellPosition.x + 2, _cellPosition.y, _cellPosition.z);
            _waterSurface[14] = new Vector3(_cellPosition.x + 4, _cellPosition.y, _cellPosition.z);

            _waterSurface[15] = new Vector3(_cellPosition.x - 4, _cellPosition.y, _cellPosition.z + 2);
            _waterSurface[16] = new Vector3(_cellPosition.x - 2, _cellPosition.y, _cellPosition.z + 2);
            _waterSurface[17] = new Vector3(_cellPosition.x, _cellPosition.y, _cellPosition.z + 2);
            _waterSurface[18] = new Vector3(_cellPosition.x + 2, _cellPosition.y, _cellPosition.z + 2);
            _waterSurface[19] = new Vector3(_cellPosition.x + 4, _cellPosition.y, _cellPosition.z + 2);

            _waterSurface[20] = new Vector3(_cellPosition.x - 4, _cellPosition.y, _cellPosition.z + 4);
            _waterSurface[21] = new Vector3(_cellPosition.x - 2, _cellPosition.y, _cellPosition.z + 4);
            _waterSurface[22] = new Vector3(_cellPosition.x, _cellPosition.y, _cellPosition.z + 4);
            _waterSurface[23] = new Vector3(_cellPosition.x + 2, _cellPosition.y, _cellPosition.z + 4);
            _waterSurface[24] = new Vector3(_cellPosition.x + 4, _cellPosition.y, _cellPosition.z + 4);

            for (int i = 0; i < 25; i++)
            {
                _waterSurface[i] = Ocean.Instance.WaterPosition(_waterSurface[i]);
            }

            _waterSurface[25] = new Vector3(_waterSurface[0].x, _playerTransform.position.y - 1 , _waterSurface[0].z);
            _waterSurface[26] = new Vector3(_waterSurface[1].x, _playerTransform.position.y - 1, _waterSurface[1].z);
            _waterSurface[27] = new Vector3(_waterSurface[2].x, _playerTransform.position.y - 1, _waterSurface[2].z);
            _waterSurface[28] = new Vector3(_waterSurface[3].x, _playerTransform.position.y - 1, _waterSurface[3].z);
            _waterSurface[29] = new Vector3(_waterSurface[4].x, _playerTransform.position.y - 1, _waterSurface[4].z);
            _waterSurface[30] = new Vector3(_waterSurface[5].x, _playerTransform.position.y - 1, _waterSurface[5].z);
            _waterSurface[31] = new Vector3(_waterSurface[6].x, _playerTransform.position.y - 1, _waterSurface[6].z);
            _waterSurface[32] = new Vector3(_waterSurface[7].x, _playerTransform.position.y - 1, _waterSurface[7].z);
            _waterSurface[33] = new Vector3(_waterSurface[8].x, _playerTransform.position.y - 1, _waterSurface[8].z);
            _waterSurface[34] = new Vector3(_waterSurface[9].x, _playerTransform.position.y - 1, _waterSurface[9].z);
            _waterSurface[35] = new Vector3(_waterSurface[10].x, _playerTransform.position.y - 1, _waterSurface[10].z);
            _waterSurface[36] = new Vector3(_waterSurface[11].x, _playerTransform.position.y - 1, _waterSurface[11].z);
            _waterSurface[37] = new Vector3(_waterSurface[12].x, _playerTransform.position.y - 1, _waterSurface[12].z);
            _waterSurface[38] = new Vector3(_waterSurface[13].x, _playerTransform.position.y - 1, _waterSurface[13].z);
            _waterSurface[39] = new Vector3(_waterSurface[14].x, _playerTransform.position.y - 1, _waterSurface[14].z);
            _waterSurface[40] = new Vector3(_waterSurface[15].x, _playerTransform.position.y - 1, _waterSurface[15].z);
            _waterSurface[41] = new Vector3(_waterSurface[16].x, _playerTransform.position.y - 1, _waterSurface[16].z);
            _waterSurface[42] = new Vector3(_waterSurface[17].x, _playerTransform.position.y - 1, _waterSurface[17].z);
            _waterSurface[43] = new Vector3(_waterSurface[18].x, _playerTransform.position.y - 1, _waterSurface[18].z);
            _waterSurface[44] = new Vector3(_waterSurface[19].x, _playerTransform.position.y - 1, _waterSurface[19].z);
            _waterSurface[45] = new Vector3(_waterSurface[20].x, _playerTransform.position.y - 1, _waterSurface[20].z);
            _waterSurface[46] = new Vector3(_waterSurface[21].x, _playerTransform.position.y - 1, _waterSurface[21].z);
            _waterSurface[47] = new Vector3(_waterSurface[22].x, _playerTransform.position.y - 1, _waterSurface[22].z);
            _waterSurface[48] = new Vector3(_waterSurface[23].x, _playerTransform.position.y - 1, _waterSurface[23].z);
            _waterSurface[49] = new Vector3(_waterSurface[24].x, _playerTransform.position.y - 1, _waterSurface[24].z);

            FindPolygon();
            FindCross();

            _playerIsUnderWater = _cameraTransform.position.y < _cross.y;
            /*
            bool playerIsUnderWaterPlus = _cameraTransform.position.y - 1 < _cross.y;


            if (playerIsUnderWaterPlus && _sendState)
            {
                _state.ChangeState(State.underwater);
                _sendState = false;
            }

            if (!playerIsUnderWaterPlus && !_sendState)
            {
                _state.ChangeState(State.stay);
                _sendState = true;
            }*/

            _points.Clear();
            _points = new List<Vector3>();


            for (int i = 0; i < _pointsToVerticies.Count; i++)
            {
                _points.Add(_waterSurface[_pointsToVerticies[i]] - _playerTransform.position);
            }

            transform.position = _playerTransform.position;

            _meshFilter.mesh.vertices = _points.ToArray();

            _meshFilter.mesh.RecalculateNormals();
            _meshFilter.mesh.RecalculateTangents();
            _meshFilter.mesh.RecalculateBounds();

        }

    }

    private void LateUpdate()
    {
        _renderer.enabled = _cameraTransform.position.y - 0.5f < _cross.y;

        float delta = Cross.y - _cameraTransform.position.y;
        
        delta = delta > 20 ? 20 : delta;
        delta = delta < 0 ? 0 : delta;

        _underWaterMTL.SetFloat("_Deep", delta / 20);
    }

    public void FindPolygon()
    {
        bool playerInPolygon = false;

        for (int i=0; i< _poligons.Count; i++)
        {
            playerInPolygon = _playerTransform.position.x > _waterSurface[_poligons[i].i0].x && _playerTransform.position.x < _waterSurface[_poligons[i].i1].x &&
                _playerTransform.position.z > _waterSurface[_poligons[i].i0].z && _playerTransform.position.z < _waterSurface[_poligons[i].i2].z;

            if (playerInPolygon)
            {
                _polygon[0] = _waterSurface[_poligons[i].i0];
                _polygon[1] = _waterSurface[_poligons[i].i1];
                _polygon[2] = _waterSurface[_poligons[i].i2];
                _polygon[3] = _waterSurface[_poligons[i].i3];
                i = _poligons.Count;
            }
        }
    }

    public void FindCross()
    {
        float y0 = ((transform.position.z - _polygon[0].z) * (_polygon[2].y - _polygon[0].y) / (_polygon[2].z - _polygon[0].z)) + _polygon[0].y;
        float y1 = ((transform.position.z - _polygon[1].z) * (_polygon[3].y - _polygon[1].y) / (_polygon[3].z - _polygon[1].z)) + _polygon[1].y;
        float y2 = ((transform.position.x - _polygon[0].x) * (_polygon[1].y - _polygon[0].y) / (_polygon[1].x - _polygon[0].x)) + _polygon[0].y;
        float y3 = ((transform.position.x - _polygon[2].x) * (_polygon[3].y - _polygon[2].y) / (_polygon[3].x - _polygon[2].x)) + _polygon[2].y;

        _cross = _playerTransform.position;

        _cross.y = (y0 + y1 + y2 + y3) / 4;
    }

    //y = (x - xa)(yb - ya)/(xb - xa) + ya

    private void OnDrawGizmos()
    {
        if (_drawMarkers)
        {
            if (_waterSurface.Count > 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    Gizmos.DrawCube(_waterSurface[i], new Vector3(0.1f, 0.1f, 0.1f));
                }

                for (int i = 0; i < _polygon.Count; i++)
                {
                    Gizmos.color = new Color(0, 1, 0, 1f);
                    Gizmos.DrawCube(_polygon[i], new Vector3(0.1f, 0.1f, 0.1f));
                }

                Gizmos.color = new Color(1, 1, 0, 1f);
                Gizmos.DrawCube(_cross, new Vector3(0.1f, 0.1f, 0.1f));
            }
        }
    }

}

[Serializable]
public class PolygonPoints
{
    public int i0;
    public int i1;
    public int i2;
    public int i3;
}

/*
 GenericPropertyJSON:{"name":"_pointsToVerticies","type":-1,"arraySize":90,"arrayType":"int","children":[{"name":"Array","type":-1,"arraySize":90,"arrayType":"int","children":[{"name":"size","type":12,"val":90},{"name":"data","type":0,"val":0},{"name":"data","type":0,"val":26},{"name":"data","type":0,"val":25},{"name":"data","type":0,"val":1},{"name":"data","type":0,"val":27},{"name":"data","type":0,"val":2},{"name":"data","type":0,"val":28},{"name":"data","type":0,"val":3},{"name":"data","type":0,"val":29},{"name":"data","type":0,"val":4},{"name":"data","type":0,"val":45},{"name":"data","type":0,"val":21},{"name":"data","type":0,"val":20},{"name":"data","type":0,"val":46},{"name":"data","type":0,"val":22},{"name":"data","type":0,"val":47},{"name":"data","type":0,"val":23},{"name":"data","type":0,"val":48},{"name":"data","type":0,"val":24},{"name":"data","type":0,"val":49},{"name":"data","type":0,"val":20},{"name":"data","type":0,"val":40},{"name":"data","type":0,"val":45},{"name":"data","type":0,"val":15},{"name":"data","type":0,"val":35},{"name":"data","type":0,"val":10},{"name":"data","type":0,"val":30},{"name":"data","type":0,"val":5},{"name":"data","type":0,"val":25},{"name":"data","type":0,"val":0},{"name":"data","type":0,"val":49},{"name":"data","type":0,"val":19},{"name":"data","type":0,"val":24},{"name":"data","type":0,"val":44},{"name":"data","type":0,"val":39},{"name":"data","type":0,"val":14},{"name":"data","type":0,"val":9},{"name":"data","type":0,"val":34},{"name":"data","type":0,"val":4},{"name":"data","type":0,"val":29},{"name":"data","type":0,"val":26},{"name":"data","type":0,"val":30},{"name":"data","type":0,"val":25},{"name":"data","type":0,"val":31},{"name":"data","type":0,"val":27},{"name":"data","type":0,"val":35},{"name":"data","type":0,"val":32},{"name":"data","type":0,"val":28},{"name":"data","type":0,"val":36},{"name":"data","type":0,"val":40},{"name":"data","type":0,"val":33},{"name":"data","type":0,"val":29},{"name":"data","type":0,"val":34},{"name":"data","type":0,"val":41},{"name":"data","type":0,"val":45},{"name":"data","type":0,"val":46},{"name":"data","type":0,"val":37},{"name":"data","type":0,"val":38},{"name":"data","type":0,"val":39},{"name":"data","type":0,"val":42},{"name":"data","type":0,"val":47},{"name":"data","type":0,"val":43},{"name":"data","type":0,"val":44},{"name":"data","type":0,"val":48},{"name":"data","type":0,"val":49},{"name":"data","type":0,"val":3},{"name":"data","type":0,"val":9},{"name":"data","type":0,"val":4},{"name":"data","type":0,"val":8},{"name":"data","type":0,"val":2},{"name":"data","type":0,"val":14},{"name":"data","type":0,"val":7},{"name":"data","type":0,"val":1},{"name":"data","type":0,"val":13},{"name":"data","type":0,"val":19},{"name":"data","type":0,"val":6},{"name":"data","type":0,"val":0},{"name":"data","type":0,"val":5},{"name":"data","type":0,"val":18},{"name":"data","type":0,"val":24},{"name":"data","type":0,"val":23},{"name":"data","type":0,"val":12},{"name":"data","type":0,"val":11},{"name":"data","type":0,"val":10},{"name":"data","type":0,"val":17},{"name":"data","type":0,"val":22},{"name":"data","type":0,"val":16},{"name":"data","type":0,"val":15},{"name":"data","type":0,"val":21},{"name":"data","type":0,"val":20}]}]}
 */