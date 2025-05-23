using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRevolver : InteractbleObject
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void DoSomeThing(float timetointeraction)
    {
        StartCoroutine(DoSomeThingCourutine(timetointeraction));
    }

    public override IEnumerator DoSomeThingCourutine(float timetointeraction)
    {
        yield return new WaitForSeconds(timetointeraction);
    }
}
