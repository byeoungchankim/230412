using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform destSampleTr = null;

    [Header("-- Flag --")]
    [SerializeField]
    private Flag entryFlag = null;
    [SerializeField]
    private Flag goalFlag = null;
    [SerializeField]
    

    private NavMeshAgent agent = null;

    private List<Flag> flagList = null;
    private bool isMoving = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        flagList =
            FlagManager.PathFinding(entryFlag, goalFlag);

        if (flagList == null ||
            flagList.Count == 1)
        {
            Debug.LogError("PathFinding Failed!");
            return;
        }

        SetNextFlag();
    }

    private IEnumerator MovingCoroutine(Flag _nextFlag)
    {
        agent.destination = _nextFlag.transform.position;
        isMoving = true;
        yield return null;

        while (agent.remainingDistance > 0f)
            yield return null;

        _nextFlag.SetColor(Flag.EState.Passed);
        isMoving = false;

        SetNextFlag();
    }

    private void SetNextFlag()
    {
        if (isMoving) return;
        if (flagList.Count == 0) return;

        Flag nextFlag = flagList[0];
        //StartCoroutine(MovingCoroutine(nextFlag));
        StartCoroutine("MovingCoroutine", nextFlag);
        flagList.RemoveAt(0);
        //flagList.Remove(nextFlag);
    }

    private void Update()
    {
        //Debug.Log(agent.remainingDistance);
        //Debug.Log(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (MousePicking(out Flag picFlag))
            {
                FlagManager.ClearColors();
                entryFlag = goalFlag;
                goalFlag = picFlag;
                flagList =
                    FlagManager.PathFinding(entryFlag, goalFlag);
                //클릭시 깃발 색상
                picFlag.SetColor(Flag.EState.Select);

                if (flagList == null ||
                    flagList.Count == 1)
                {
                    Debug.LogError("PathFinding Failed!");
                    return;
                }

                SetNextFlag();
            }
        }
    }

    //마우스  깃발 클릭
    private bool MousePicking(out Flag _pickFlag)
    {
        _pickFlag = null;
        Vector3 mousePos = Input.mousePosition;
        Ray ray =
            Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.Log(
            //    hitInfo.transform.name +
            //    ": " + hitInfo.point);
            if (hitInfo.collider.CompareTag("Flag"))
            {
                _pickFlag = hitInfo.collider.gameObject.GetComponent<Flag>();
                return true;
            }
        }
        return false;
    }
}
