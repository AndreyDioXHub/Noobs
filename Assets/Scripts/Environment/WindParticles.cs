using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticles : MonoBehaviour
{
    /// <summary>
    /// Префаб с частицами
    /// </summary>
    [SerializeField]
    GameObject particleTemplate;

    /// <summary>
    /// Разброс по расстоянию от нуля при создании частицы
    /// </summary>
    [SerializeField, Tooltip("Distance for out range from zero")]
    Vector2 StartOffsetRange = new Vector2(25,50);

    /// <summary>
    /// Угол разворота вектора против ветра при генерации частицы
    /// 0 - начальная точка точно в противоположной от направления ветра
    /// 180 - точно по направлению ветра
    /// </summary>
    [SerializeField, Range(0,180)]
    float MaxStartRange = 120f;

    /// <summary>
    /// Количество частиц, одновременно существующих в моменте
    /// </summary>
    [SerializeField]
    int MaxOnTimeParticles = 10;

    /// <summary>
    /// Время до создания следующей частицы
    /// </summary>
    [SerializeField]
    Vector2 RangeGenerateTime = new Vector2(0.5f, 4f);

    bool canEmit = false;
    float emitpause = 5;
    float _currenttime = 0;
    int totalemitted = 0;

    // Start is called before the first frame update
    void Start()
    {
        EmitWindLine();
    }

    // Update is called once per frame
    void Update()
    {
        if(canEmit) {
            EmitWindLine();
            _currenttime = 0;
            canEmit = false;
        } else {
            _currenttime += Time.deltaTime;
        }
        if(_currenttime > emitpause && totalemitted <= MaxOnTimeParticles) {
            canEmit = true;
        }
    }

    private void EmitWindLine(float lifetime = 5f) {
        //Next emmit
        emitpause = Random.Range(RangeGenerateTime.x, RangeGenerateTime.y);
        //Rotation angle
        float rangle = Random.Range(0, MaxStartRange) * (Random.Range(0,2)*2-1);

        float offset = Random.Range(StartOffsetRange.x, StartOffsetRange.y);

        var windline = Instantiate(particleTemplate, transform);
        //Calculate position based on wind
        Vector3 _wind = WorldValues.Instance.Wind.normalized;
        Vector3 _pos = Quaternion.AngleAxis(180-rangle, Vector3.up) * _wind * offset;


        windline.transform.localPosition = _pos;
        windline.GetComponent<WindFly>().OnDestroyObject += ObjectRemoved;
        Destroy(windline, lifetime);
        totalemitted++;
    }

    private void ObjectRemoved() {
        totalemitted--;
    }
}
