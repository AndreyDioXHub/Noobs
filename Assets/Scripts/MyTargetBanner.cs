/*
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Common;
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTargetBanner : MonoBehaviour
{/*
    private readonly System.Object _syncRoot = new System.Object();
    /private MyTargetView _myTargetView;
    [SerializeField]
    private UInt32 _slotId = 1474896;
    private void Awake()
    {
        // ��������� ������ �������
        MyTargetManager.DebugMode = true;

        // ������� ��������� MyTargetView, ������ 320x50

        // ������� ��������� MyTargetView, ������ 300�250
        // _myTargetView = new MyTargetView(slotId, AdSize.Size300x250);// ������� ��������� MyTargetView
        _myTargetView = new MyTargetView(_slotId, MyTargetView.AdSize.Size728x90);

        // ������������� ����������� �������
        _myTargetView.AdClicked += OnAdClicked;
        _myTargetView.AdLoadFailed += OnAdLoadFailed;
        _myTargetView.AdLoadCompleted += OnAdLoadCompleted;
        _myTargetView.AdShown += OnAdShown;

        // ��������� �������� ������
        _myTargetView.Load();
    }
    private void OnAdClicked(System.Object sender, EventArgs eventArgs) { }

    private void OnAdShown(System.Object sender, EventArgs eventArgs) { }

    private void OnAdLoadFailed(System.Object sender, ErrorEventArgs errorEventArgs) { }

    private void OnAdLoadCompleted(System.Object sender, EventArgs eventArgs)
    {
        // ������ ������� ���������

        // ������������� ������� �� ������
        _myTargetView.X = 0;
        _myTargetView.Y = 0;

        // ��������� ����� ����������
        _myTargetView.Start();
    }

    private void OnDestroy()
    {
        if (_myTargetView == null)
        {
            return;
        }
        lock (_syncRoot)
        {
            if (_myTargetView == null)
            {
                return;
            }
            _myTargetView.Dispose();
            _myTargetView = null;
        }
    }

    void Start()
    {
    }

    void Update()
    {
        
    }*/
}
