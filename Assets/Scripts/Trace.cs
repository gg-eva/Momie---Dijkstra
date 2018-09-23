using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour {

    List<Vector3> path = new List<Vector3>();

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        if (path.Count==0)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
            
            var t = Time.time;
            int start = 0;
            int stop = Mathf.Min((int) ((path.Count + 1) * Mathf.Abs(Mathf.Sin(t))), path.Count);

            lineRenderer.positionCount = stop - start;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, path[start + i]);
            }
        }
    }


    public void SetPath(List<Vector3> path)
    {
        this.path = path;
    }
}
