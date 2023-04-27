using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugFlag : MonoBehaviour
{
    [SerializeField]
    private bool isShow = true;
    [SerializeField]
    private Color color = Color.white;

    private void Update()
    {
        if (!isShow) return;

        Flag[] nextFlags =
            GetComponent<Flag>().GetNextFlags();

        foreach (Flag flag in nextFlags)
            Debug.DrawLine(
                transform.position,
                flag.transform.position,
                color);
    }
}
