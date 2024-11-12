using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/IsTreasureNear")]
[Help("Checks whether the Treasure is near the Agent.")]
public class IsTreasureNear : ConditionBase
{
    [InParam("agent")]
    public GameObject agent;

    [InParam("treasure")]
    public GameObject treasure;

    [InParam("nearDistance")]
    public float nearDistance = 10f;

    public override bool Check()
    {
        return Vector3.Distance(agent.transform.position, treasure.transform.position) > nearDistance;
    }
}