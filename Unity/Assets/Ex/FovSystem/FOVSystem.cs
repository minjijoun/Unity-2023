using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVSystem : MonoBehaviour
{
    public float viewRadius;
    public float ViewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine("FindTargetWithDelay", 0.2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTartgetColor(Color.white);
        visibleTargets.Clear();

        Collider[] targetslnViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i < targetslnViewRadius.Length; i++)
        {
            Transform target = targetslnViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < ViewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget , dstToTarget , obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
        visibleTartgetColor(Color.green);
    }

    public Vector3 DirFromAngle(float anglelnDergrees, bool anglelsGlobal)
    {
        if(!anglelsGlobal)
        {
            anglelnDergrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(anglelnDergrees * Mathf.Deg2Rad), 0, Mathf.Cos(anglelnDergrees * Mathf.Deg2Rad));
    }

    void visibleTartgetColor(Color color)
    {
        for(int i = 0; i < visibleTargets.Count; i++)
        {
            visibleTargets[i].GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }
}
