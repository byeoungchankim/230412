using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private static Flag[] flags;

    private void Awake()
    {
        God();
        outdated = false;
    }

    private void Update()
    {
        if(outdated)
        {
            God();
        }
    }

    private static bool outdated;
    public static void FlagOutdated()
    {
        outdated = true;
    }

    private void God()
    {
        flags = GetComponentsInChildren<Flag>();

        //��� ����
        int N = flags.Length;
        adj = new float[N, N];
        //adj = ������ ��尣�� �Ÿ��� �����ϴ°�
        // 2�� for������ adj�� ���� INF�� �ʱ�ȭ
        for (int i = 0; i < N; ++i)
        {
            for(int j = 0; j < N; ++j)
            {
                //INF ���� INF * (N + 1) ���̺� �ʱ�ȭ ��� �ϳ� ������� �����°� ����
                adj[i, j] = INF * (N + 1);
            }
        }
        
        for (int i = 0; i < N; ++i)
        {
            //�ڱ� �ڽŰ��� �Ÿ��� 0
            adj[i, i] = 0f;
            // �� flags���� ���� �� �ִ� flag�� �Ÿ��� ����
            Flag curFlag = flags[i];
            //���� Ȯ������ ���
            Flag[] nextFlags = curFlag.GetNextFlags();
            //���� Ȯ������ ��߰� ���� ����� ��ߵ�
            for (int j = 0; j < nextFlags.Length; ++j)
            {
                int next = GetIndex(nextFlags[j]);

                if (next < 0) continue;

                //�� ��� �Ÿ�
                float dist = Vector3.Distance(curFlag.transform.position, nextFlags[j].transform.position);


                // nextFlag[j]�� curFlag�� �Ÿ��� ���ؼ� adj�� ����
                adj[i, next] = dist;
            }
        }
        /*
        for (int i = 0; i < N; ++i)
        {
            string t = "";
            for (int j = 0; j < N; ++j)
            {
                t += adj[i, j] + " ";
            }
            Debug.Log(t);
        }
        */
    }

    //����� �ε��� ���ϱ�
    private static int GetIndex(Flag flag)
    {
        for (int i = 0; i < flags.Length; ++i)
            if (flags[i] == flag) return i;

        return -1;
    }


    private const float INF = 1000000f;
    private static float[,] adj;

    // ���ͽ�Ʈ�� dijkstra
    public static List<Flag> PathFinding(
        Flag _entryFlag, Flag _goalFlag)
    {
        
        // deep copy adj
        float[,] a = (float[,])adj.Clone();
        //N �� ����� ����
        int N = flags.Length;
        //Ȯ�� ����
        bool[] visited = new bool[N];
        //��ŸƮ���� ������� ���� �ִܰŸ�
        float[] distance = new float[N];
        //�ִܰŸ��� �������� ä��� ���Ѱ�
        Array.Fill(distance, INF);
        //���� ���
        int[] parent = new int[N];

        //������ġ
        int start = GetIndex(_entryFlag);
        distance[start] = 0; //�ڱ� �ڽ��� 0�Ÿ��� ����� : �ִܰŸ�
        parent[start] = start; //�ڱ� �ڽ��� �θ�� �ڱ��ڽ� ���� : �ִܰ��

        while(true)
        {
            //���� ���� �ĺ��� ã�´�

            //���� ������ ������ �Ÿ��� ��ȣ�� ����
            float closet = INF;
            int now = -1;
            
            for(int i = 0; i < N; i++)
            {
                //�̹� �湮�� ������ ��ŵ
                if (visited[i])
                    continue;
                //���� �߰ߵ��� ���ų�, ���� �湮���� �ʰ� �Ÿ��� ����� ����߿��� ���� ���� ��带 ã�Ƴ��� ���� ����
                if (distance[i] == INF || distance[i] >= closet)
                    continue;

                closet = distance[i];
                now = i;
            }

            //now : ���� �湮���� ���� ��� �� ���� ª�� �Ÿ��� ª�� ���
            if (now == -1)
                break;
            visited[now] = true;

            for (int next = 0; next < N; next++ )
            {
                // ������� ���� ������ ��ŵ�Ѵ�
                if (adj[now, next] == -1)
                    continue;

                // �̹� �湮�� ������ ��ŵ
                if (visited[next])
                    continue;

                //now : 1  ,
                //next : 3
                // ���� ����� ������ �ִ� �Ÿ��� ����Ѵ�.
                //                      15  +  10
                float nextDist = distance[now] + adj[now, next];
                // �������� �����ؼ� ��� �ߴ� ��� ����distance[next]����,
                // ���� ����ϰ��ִ� ���� ����(nextDist)�� �� �۴ٸ� next Dist �� �� ª�� ��η� �����Ѵ�
                if (nextDist < distance[next])
                {
                    distance[next] = nextDist;      //�ش� ����� ���� ���� ª�� �Ÿ�
                    parent[next] = now;             //�ش� ����� ������� ª�� �Ÿ��� �θ�

                    //�� ��忡 ������ �Ÿ��� ��� ���� ������ 
                }

            }
        }


        // �ִ� ��� ������
        int end = GetIndex(_goalFlag);
        
        if (distance[end] == INF)
        {
            Debug.Log("���� �� ã��!");
            return new();
        }
        List<Flag> path = new();
        //���������� ������ ����� ��ο� �߰�
        while(end != start)
        {
            path.Add(flags[end]);
            end = parent[end];
        }
        //���� ����� ��ο� �߰�
        path.Add(flags[start]);
        //�Ųٷε� ��θ� ������
        path.Reverse();

        return path;
    }

    //�� �ʱ�ȭ
    public static void ClearColors()
    {
        foreach (Flag flag in flags) flag.SetColor(Flag.EState.Normal);
    }


}
