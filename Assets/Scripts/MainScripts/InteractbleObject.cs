using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractbleObject : MonoBehaviour
{

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void DoSomeThing(float timetointeraction)
    {
        StartCoroutine(DoSomeThingCourutine(timetointeraction));
    }

    public virtual IEnumerator DoSomeThingCourutine(float timetointeraction)
    {
        yield return null;
    }

    public virtual void DisabledObject()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        DisabledObject();
    }

    private void OnDisable()
    {
        DisabledObject();
    }
}
