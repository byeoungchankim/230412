using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    private Coroutine testCo = null;

    private void Start()
    {
        //testCo = StartCoroutine(TestCoroutine());
        //StartCoroutine("TestCoroutine2");

        //InvokeRepeating("TestInvoke", 0f, 0f);
    }
    

    private void TestInvoke()
    {
        Debug.Log("Test Invoke");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StopCoroutine(TestCoroutine());
            //StopCoroutine(testCo);
            //StopCoroutine("TestCoroutine2");

            StopAllCoroutines();
        }
    }

    private IEnumerator TestCoroutine()
    {
        while (true)
        {
            Debug.Log("1");
            yield return new WaitForSeconds(1f);
            Debug.Log("2");
            yield return new WaitForSeconds(1f);
            Debug.Log("3");
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator TestCoroutine2()
    {
        while (true)
        {
            Debug.Log("----- 1");
            yield return new WaitForSeconds(1f);
            Debug.Log("----- 2");
            yield return new WaitForSeconds(1f);
            Debug.Log("----- 3");
            yield return new WaitForSeconds(1f);
        }
    }
}
