using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineRotation : MonoBehaviour
{
    [SerializeField]
    private Vector2 maxRotateSpeed = Vector2.zero;
    CinemachinePOV lookBase;
    [SerializeField]
    CinemachineVirtualCamera vcam;

    [SerializeField]
    float cval = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        lookBase = vcam.GetCinemachineComponent<CinemachinePOV>();
        if( lookBase == null ) {
            this.enabled = false;
        }
    }

    private void Start() {
        lookBase = vcam.GetCinemachineComponent<CinemachinePOV>();
    }

    // Update is called once per frame
    void Update()
    {
        bool adsView = false;

        if (AdsButtonView.Instance != null)
        {
            adsView = AdsButtonView.Instance.Parent.activeSelf;
        }

        if (AdsScreen.IsActive || AdsButtonView.IsActive || CheckPointManager.Instance.IsWin || SettingScreen.IsActive || ChatTexts.IsActive)// || TutorialCanvas.Instance.gameObject.activeSelf)// || BlockCountManager.Instance.BlocksCount == 0)
        {
            if (lookBase != null)
            {
                lookBase.m_VerticalAxis.m_MaxSpeed = 0;
                lookBase.m_HorizontalAxis.m_MaxSpeed = 0;
            }

        }
        else
        {
            if (lookBase != null)
            {
                lookBase.m_VerticalAxis.m_MaxSpeed = cval * maxRotateSpeed.y;
                lookBase.m_HorizontalAxis.m_MaxSpeed = cval * maxRotateSpeed.x;
            }

        }
    }

    public void SetSensitivity(float val) 
    {
        if(lookBase != null) 
        {
            cval = val;
            lookBase.m_VerticalAxis.m_MaxSpeed = val * maxRotateSpeed.y;
            lookBase.m_HorizontalAxis.m_MaxSpeed = val * maxRotateSpeed.x;
        }
    }
}
