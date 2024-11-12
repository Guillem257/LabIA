using System.Collections;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Action("MyActions/StealTreasure")]
[Help("Steals the treasure if the cop is not near.")]
public class StealTreasure : BasePrimitiveAction
{
    [InParam("treasure")]
    public GameObject treasure;

    [InParam("cop")]
    public GameObject cop;

    [InParam("dist2Steal")]
    public float dist2Steal = 5f;

    [OutParam("isStolen")]
    public bool isStolen;

    private WaitForSeconds wait = new WaitForSeconds(0.1f);

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(cop.transform.position, treasure.transform.position) > dist2Steal)
        {
            treasure.GetComponent<Renderer>().enabled = false;
            Debug.Log("Stolen");
            isStolen = true;
            return TaskStatus.COMPLETED;
        }
        else
        {
            return TaskStatus.FAILED;
        }
    }
}