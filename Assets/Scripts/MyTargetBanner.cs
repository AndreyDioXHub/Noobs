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
        // Включение режима отладки
        MyTargetManager.DebugMode = true;

        // Создаем экземпляр MyTargetView, формат 320x50

        // Создаем экземпляр MyTargetView, формат 300х250
        // _myTargetView = new MyTargetView(slotId, AdSize.Size300x250);// Создаем экземпляр MyTargetView
        _myTargetView = new MyTargetView(_slotId, MyTargetView.AdSize.Size728x90);

        // Устанавливаем обработчики событий
        _myTargetView.AdClicked += OnAdClicked;
        _myTargetView.AdLoadFailed += OnAdLoadFailed;
        _myTargetView.AdLoadCompleted += OnAdLoadCompleted;
        _myTargetView.AdShown += OnAdShown;

        // Запускаем загрузку данных
        _myTargetView.Load();
    }
    private void OnAdClicked(System.Object sender, EventArgs eventArgs) { }

    private void OnAdShown(System.Object sender, EventArgs eventArgs) { }

    private void OnAdLoadFailed(System.Object sender, ErrorEventArgs errorEventArgs) { }

    private void OnAdLoadCompleted(System.Object sender, EventArgs eventArgs)
    {
        // Данные успешно загружены

        // Устанавливаем позицию на экране
        _myTargetView.X = 0;
        _myTargetView.Y = 0;

        // Запускаем показ объявлений
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
