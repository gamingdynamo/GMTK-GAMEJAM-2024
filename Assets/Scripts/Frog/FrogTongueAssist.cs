using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrogTongueAssist : MonoBehaviour
{
    private BoxCollider[] colids;
    private Transform closestTarget = null;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Tonguable")) { return; }
        if (closestTarget == null)
        {
            closestTarget = other.transform;
        }
        else
        {
            if (Vector3.Angle(transform.forward, other.transform.position - transform.position) < Vector3.Angle(transform.forward, closestTarget.position - transform.position))
            {
                closestTarget = other.transform;
            }
        }
    }
    public void SetAimAssistRotation(Vector3 forwardDir)
    {
        transform.forward = forwardDir;
    }

    private void SetAimAssist(int level)
    {
        float halfColidDistance = FrogBehaviour.Instance.FrogScripObj.FrogTongueMaxLength / FrogBehaviour.Instance.FrogScripObj.AimAssistColliderNumber / 2.0f;
        for (int i = 0; i < colids.Length; i++)
        {
            colids[i].center = (1.0f + 2.0f * i) * halfColidDistance * Vector3.forward;
            float colidZsize = 2.0f * halfColidDistance;
            float colidXYsize = 2.0f * (1.0f + 2.0f * i) * halfColidDistance * Mathf.Tan(FrogBehaviour.Instance.FrogScripObj.AimAssistAngle * 0.5f * Mathf.Deg2Rad);
            colids[i].size = new Vector3(colidXYsize, colidXYsize, colidZsize);
        }
    }

    public void InitBoxColliders()
    {
        for (int i = 0; i < FrogBehaviour.Instance.FrogScripObj.AimAssistColliderNumber; i++)
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
        }
        colids = GetComponents<BoxCollider>();
        SetAimAssist(0);
    }

    public Tonguable LockInTarget()
    {
        return InAngles(closestTarget) ? closestTarget.GetComponent<Tonguable>() : null;
    }
    
    private bool InAngles(Transform trans)
    {
        if (trans == null) { return false; }
        return Vector3.Angle(FrogBehaviour.Instance.AimPosition() - transform.position, trans.position - transform.position) <= FrogBehaviour.Instance.FrogScripObj.AimAssistAngle * 0.5f;
    }
}
