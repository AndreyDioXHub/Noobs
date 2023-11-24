using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigger : MonoBehaviour
{
    [SerializeField]
    private GameObject TPSPrefab;
    [SerializeField]
    private GameObject FPSPrefab;
    [SerializeField]
    private Transform _cameraCenterTPS;
    [SerializeField]
    private Transform _cameraCenterFPS;
    [SerializeField]
    private GameObject TPS;
    [SerializeField]
    private GameObject FPS;
    [SerializeField]
    private CameraView _cameraView;


    void Start()
    {
        TPS = Instantiate(TPSPrefab);
        TPS.GetComponent<CinemachineVirtualCamera>().Follow = _cameraCenterTPS;
        FPS = Instantiate(FPSPrefab);
        FPS.GetComponent<CinemachineVirtualCamera>().Follow = _cameraCenterFPS;
        _cameraView.Init(FPS, TPS);
        InputSchemeSwitcher.Instance.Init(FPS.GetComponent<CinemachineInputProvider>(), TPS.GetComponent<CinemachineInputProvider>());
        InputSchemeSwitcher.Instance.RequestingEnvironmentData();
    }


    void Update()
    {
        
    }
}
