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
        //Debug.Log($"<color=cyan>Animator Sync enabled</color>");
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
            Debug.LogWarning("Network not active!");
            this.enabled = false;
            return; 
        }
        //Debug.Log("<color=cyan>Animator Sync subscribe</color>");
        EtalonAnimationSync.Instance.TimeLoopUpdate.AddListener(OnAnimTimeUpdated);
        //myAnimator TODO Get Total time of clip
    }

    private void OnAnimTimeUpdated(float atime) {
        SyncTime(atime);
    }

    private void UnregisterAnimator() {
        //Debug.Log("<color=cyan>Animator Sync UNSUBSCRIBE</color>");
        if(EtalonAnimationSync.Instance != null) {
            EtalonAnimationSync.Instance.TimeLoopUpdate.RemoveListener(OnAnimTimeUpdated);
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
        //Debug.Log($"<color=red>[Animation name:]</color> {currAnimName}");
        return currAnimName;

    }
}

