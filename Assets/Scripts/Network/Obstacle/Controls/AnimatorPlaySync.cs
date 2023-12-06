using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlaySync : MonoBehaviour
{

    bool isstarted = false;

    Animator myAnimator;
    public float normalizedTime = 0;

    // Start is called before the first frame update
    void Start() {
        myAnimator = GetComponent<Animator>();
        if(myAnimator == null ) {
            Debug.LogWarning("Animator does not exist on gameObject. Component will be disabled");
            enabled = false;
            return;
        }
        RegisterAnimator();
        isstarted = true;
    }

    private void RegisterAnimator() {
        if(!NetworkClient.active) {
            this.enabled = false;
            return; 
        }
        EtalonAnimationSync.Instance.TimeLoopUpdate += OnAnimTimeUpdated;
        //myAnimator TODO Get Total time of clip
    }

    private void OnAnimTimeUpdated(float atime) {
        
    }

    private void UnregisterAnimator() {
        if(EtalonAnimationSync.Instance != null) {
            EtalonAnimationSync.Instance.TimeLoopUpdate -= OnAnimTimeUpdated;
        }
    }


    private void OnEnable() {
        if (isstarted) {
            RegisterAnimator();
            SyncTime(Time.unscaledTime - EtalonAnimationSync.Instance.LastTimeUpdateCall);
        }
    }

    private void SyncTime(float timeOvered) {
        //Set current time of animation clip
        myAnimator.Play(currentAnimationName(), 0, timeOvered);
    }

    private void OnDisable() {
        UnregisterAnimator();
    }

    private void OnDestroy() {
        UnregisterAnimator();
    }


    string currentAnimationName() {
        var currAnimName = "";
        foreach (AnimationClip clip in myAnimator.runtimeAnimatorController.animationClips) {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(clip.name)) {
                currAnimName = clip.name.ToString();
            }
        }

        return currAnimName;

    }
}

