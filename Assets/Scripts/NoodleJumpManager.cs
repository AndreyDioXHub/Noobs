using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleJumpManager : MonoBehaviour
{
    public static NoodleJumpManager Instance;

    public int Index { get; private set; }

    [SerializeField]
    private Transform _player;
    [SerializeField]
    private GameObject _platforms;
    [SerializeField]
    private Transform _dieCollider;
    [SerializeField]
    private float _curentPlayerPos;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for(int i=0; i < 3; i++)
        {
            var go = Instantiate(_platforms);
            go.transform.position = new Vector3(0, i*3 + 3, 0);
            int index = Random.Range(0, 4);
            index = index == Index ? Index + 1 : index;
            index = index > 3 ? 0 : index;
            Index = index;
            go.GetComponent<Platforms>().Init(Index);
        }
    }

    void Update()
    {
       // Debug.Log();
        float pos = _player.position.y - _player.position.y % 9;

        if(pos > _curentPlayerPos)
        {
            _curentPlayerPos = pos;
            Debug.Log(_curentPlayerPos);

            for (int i = 0; i < 3; i++)
            {
                var go = Instantiate(_platforms);
                go.transform.position = new Vector3(0, i * 3 + _curentPlayerPos + 3, 0);
                int index = Random.Range(0, 4);
                index = index == Index ? Index + 1 : index;
                index = index > 3 ? 0 : index;
                Index = index;
                go.GetComponent<Platforms>().Init(Index);
            }
        }
    }

    public void SetIndex(Vector3 position)
    {
        _dieCollider.position = position - Vector3.up;
    }
}
