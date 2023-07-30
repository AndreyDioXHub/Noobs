using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanPart : MonoBehaviour
{
    [SerializeField]
    private RenderTexture _wavesHeight;
    [SerializeField]
    private float _wavesAmplitude;
    [SerializeField]
    private float _half = 100;
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private Vector3 _boderTop;
    [SerializeField]
    private Vector3 _boderBot;
    [SerializeField]
    private bool _objectInside;
    [SerializeField]
    private int x, z;

    private void Awake()
    {
        _position = transform.position;
        _boderTop = new Vector3(_position.x + _half, _position.y, _position.z + _half);
        _boderBot = new Vector3(_position.x - _half, _position.y, _position.z - _half);
    }

    void Start()
    {
    }

    [ContextMenu("Mesh")]
    public void Mesh()
    {
    }


    void Update()
    {
        
    }

    public float GetHeight(Transform objectTransform, out bool result)
    {
        result = false;
        float height = 0;
        _objectInside = (objectTransform.position.x > _boderBot.x && objectTransform.position.x < _boderTop.x) && (objectTransform.position.z > _boderBot.z && objectTransform.position.z < _boderTop.z);

        if (_objectInside)
        {
            x = (int)(_wavesHeight.width - (_position.x + _half - objectTransform.position.x) * _wavesHeight.width / (_half * 2));
            z = (int)(_wavesHeight.height - (_position.z + _half - objectTransform.position.z) * _wavesHeight.height / (_half * 2));

            Texture2D tex = new Texture2D(_wavesHeight.width, _wavesHeight.height, TextureFormat.RGB24, false);
            RenderTexture.active = _wavesHeight;
            tex.ReadPixels(new Rect(0, 0, _wavesHeight.width, _wavesHeight.height), 0, 0);
            tex.Apply();

            height = tex.GetPixel(x, z).grayscale * _wavesAmplitude + _position.y;

            result = true;
        }
        else
        {
            x = 0;
            z = 0;
        }

        return height;
    }

    public bool ObjectIsUnderWater(Transform objectTransform, out bool result)
    {
        result = false;
        float height = 0;
        bool objectIsUnderWater = false;

        _objectInside = (objectTransform.position.x > _boderBot.x && objectTransform.position.x < _boderTop.x) && (objectTransform.position.z > _boderBot.z && objectTransform.position.z < _boderTop.z);

        if (_objectInside)
        {
            x = (int)(_wavesHeight.width - (_position.x + _half - objectTransform.position.x) * _wavesHeight.width / (_half * 2));
            z = (int)(_wavesHeight.height - (_position.z + _half - objectTransform.position.z) * _wavesHeight.height / (_half * 2));

            Texture2D tex = new Texture2D(_wavesHeight.width, _wavesHeight.height, TextureFormat.RGB24, false);
            RenderTexture.active = _wavesHeight;
            tex.ReadPixels(new Rect(0, 0, _wavesHeight.width, _wavesHeight.height), 0, 0);
            tex.Apply();

            height = tex.GetPixel(x, z).grayscale * _wavesAmplitude + _position.y;
            objectIsUnderWater = objectTransform.position.y < height;
            result = true;
        }

        return objectIsUnderWater;
    }

    public List<Vector3> GetVolumePoints(Transform objectTransform, out bool result)
    {
        result = false;
        List<Vector3> pointsAround = new List<Vector3>();
         
        if (ObjectIsUnderWater(objectTransform, out bool oiuwresult))
        {
            x = (int)(_wavesHeight.width - (_position.x + _half - objectTransform.position.x) * _wavesHeight.width / (_half * 2));
            z = (int)(_wavesHeight.height - (_position.z + _half - objectTransform.position.z) * _wavesHeight.height / (_half * 2));
            
            if((x < 2 && x > _wavesHeight.width - 2) && (z<2 && z> _wavesHeight.height - 2))
            {
                result = false;
                return pointsAround;
            }
            else
            {
                Texture2D tex = new Texture2D(_wavesHeight.width, _wavesHeight.height, TextureFormat.RGB24, false);
                RenderTexture.active = _wavesHeight;
                tex.ReadPixels(new Rect(0, 0, _wavesHeight.width, _wavesHeight.height), 0, 0);
                tex.Apply();

                float xtToxw = 2 * _half / _wavesHeight.width;
                float ztToxw = 2 * _half / _wavesHeight.height;

                float p1x = objectTransform.position.x - xtToxw;
                float p2x = objectTransform.position.x + xtToxw;

                float p1z = objectTransform.position.z - ztToxw;
                float p2z = objectTransform.position.z + ztToxw;


                float p1y = tex.GetPixel(x - 1, z - 1).grayscale * _wavesAmplitude + _position.y + 0.01f;
                float p2y = tex.GetPixel(x - 1, z + 1).grayscale * _wavesAmplitude + _position.y + 0.01f;
                float p3y = tex.GetPixel(x + 1, z + 1).grayscale * _wavesAmplitude + _position.y + 0.01f;
                float p4y = tex.GetPixel(x + 1, z - 1).grayscale * _wavesAmplitude + _position.y + 0.01f;

                Vector3 p1Top = new Vector3(p1x, p1y, p1z);
                Vector3 p2Top = new Vector3(p1x, p2y, p2z);
                Vector3 p3Top = new Vector3(p2x, p3y, p2z);
                Vector3 p4Top = new Vector3(p2x, p4y, p1z);

                Vector3 p1Bot = Vector3.zero;
                Vector3 p2Bot = Vector3.zero;
                Vector3 p3Bot = Vector3.zero;
                Vector3 p4Bot = Vector3.zero;

                Ray rayp1 = new Ray(p1Top, -Vector3.up); 
                RaycastHit[] hitsp1;
                hitsp1 = Physics.RaycastAll(rayp1, 20);

                for (int i = 0; i < hitsp1.Length; i++)
                {
                    if (hitsp1[i].collider.tag.Equals("Terrain"))
                    {
                        p1Bot = new Vector3(p1Top.x, hitsp1[i].point.y + 0.5f, p1Top.z);
                    }
                }

                Ray rayp2 = new Ray(p2Top, -Vector3.up); 
                RaycastHit[] hitsp2;
                hitsp2 = Physics.RaycastAll(rayp2, 20);

                for (int i = 0; i < hitsp2.Length; i++)
                {
                    if (hitsp2[i].collider.tag.Equals("Terrain"))
                    {
                        p2Bot = new Vector3(p2Top.x, hitsp2[i].point.y + 0.5f, p2Top.z);
                    }
                }

                Ray rayp3 = new Ray(p3Top, -Vector3.up); 
                RaycastHit[] hitsp3;
                hitsp3 = Physics.RaycastAll(rayp3, 20);

                for (int i = 0; i < hitsp3.Length; i++)
                {
                    if (hitsp3[i].collider.tag.Equals("Terrain"))
                    {
                        p3Bot = new Vector3(p3Top.x, hitsp3[i].point.y + 0.5f, p3Top.z);
                    }
                }

                Ray rayp4 = new Ray(p4Top, -Vector3.up); 
                RaycastHit[] hitsp4;
                hitsp4 = Physics.RaycastAll(rayp4, 20);

                for (int i = 0; i < hitsp4.Length; i++)
                {
                    if (hitsp4[i].collider.tag.Equals("Terrain"))
                    {
                        p4Bot = new Vector3(p4Top.x, hitsp4[i].point.y + 0.5f, p4Top.z);
                    }
                }

                bool itsCorrect = p1Bot.y < p1Top.y && p2Bot.y < p2Top.y && p3Bot.y < p3Top.y && p4Bot.y < p4Top.y;

                if (itsCorrect)
                {
                    pointsAround.Add(p1Top);
                    pointsAround.Add(p2Top);
                    pointsAround.Add(p3Top);
                    pointsAround.Add(p4Top);
                    pointsAround.Add(p1Bot);
                    pointsAround.Add(p2Bot);
                    pointsAround.Add(p3Bot);
                    pointsAround.Add(p4Bot);

                    result = true;
                    return pointsAround;
                }
                
            }
        }

        return pointsAround;
    }
}
