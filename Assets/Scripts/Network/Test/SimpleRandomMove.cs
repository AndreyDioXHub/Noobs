using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRandomMove : MonoBehaviour
{
    Vector3 nextposition;
    float const_height = 0;

    [SerializeField]
    float _speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        const_height = LevelConfig.Instance.START_PLAYER_YPOS;
        StartCoroutine(GenerateNewPoint());
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.Lerp(transform.position, nextposition, _speed * Time.deltaTime);
    }

    IEnumerator GenerateNewPoint() {
        while (true) {
            nextposition = UnityEngine.Random.insideUnitSphere * 8 + Vector3.forward * 300;
            nextposition.y = const_height;
            yield return new WaitForSeconds(3);
        }
        
    }
}
