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

        //깃발 갯수
        int N = flags.Length;
        adj = new float[N, N];
        //adj = 인접한 노드간의 거리를 저장하는것
        // 2중 for문으로 adj를 전부 INF로 초기화
        for (int i = 0; i < N; ++i)
        {
            for(int j = 0; j < N; ++j)
            {
                //INF 무한 INF * (N + 1) 테이블 초기화 깃발 하나 사라지면 터지는거 방지
                adj[i, j] = INF * (N + 1);
            }
        }
        
        for (int i = 0; i < N; ++i)
        {
            //자기 자신과의 거리는 0
            adj[i, i] = 0f;
            // 각 flags마다 닿을 수 있는 flag의 거리를 저장
            Flag curFlag = flags[i];
            //현재 확인중인 깃발
            Flag[] nextFlags = curFlag.GetNextFlags();
            //현재 확인중인 깃발과 직접 연결된 깃발들
            for (int j = 0; j < nextFlags.Length; ++j)
            {
                int next = GetIndex(nextFlags[j]);

                if (next < 0) continue;

                //두 깃발 거리
                float dist = Vector3.Distance(curFlag.transform.position, nextFlags[j].transform.position);


                // nextFlag[j]와 curFlag의 거리를 구해서 adj에 저장
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

    //깃발의 인덱스 구하기
    private static int GetIndex(Flag flag)
    {
        for (int i = 0; i < flags.Length; ++i)
            if (flags[i] == flag) return i;

        return -1;
    }


    private const float INF = 1000000f;
    private static float[,] adj;

    // 다익스트라 dijkstra
    public static List<Flag> PathFinding(
        Flag _entryFlag, Flag _goalFlag)
    {
        
        // deep copy adj
        float[,] a = (float[,])adj.Clone();
        //N 는 깃발의 갯수
        int N = flags.Length;
        //확인 여부
        bool[] visited = new bool[N];
        //스타트에서 여기까지 가는 최단거리
        float[] distance = new float[N];
        //최단거리를 무한으로 채우기 위한것
        Array.Fill(distance, INF);
        //이전 깃발
        int[] parent = new int[N];

        //시작위치
        int start = GetIndex(_entryFlag);
        distance[start] = 0; //자기 자신은 0거리로 만들기 : 최단거리
        parent[start] = start; //자기 자신의 부모는 자기자신 으로 : 최단경로

        while(true)
        {
            //제일 좋은 후보를 찾는다

            //가장 유력한 정점의 거리와 번호를 저장
            float closet = INF;
            int now = -1;
            
            for(int i = 0; i < N; i++)
            {
                //이미 방문한 정점은 스킵
                if (visited[i])
                    continue;
                //아직 발견된적 없거나, 아직 방문하지 않고 거리만 계산한 노드중에서 가장 잚은 노드를 찾아내기 위한 과정
                if (distance[i] == INF || distance[i] >= closet)
                    continue;

                closet = distance[i];
                now = i;
            }

            //now : 아직 방문하지 않은 노드 중 가장 짧은 거리가 짧은 노드
            if (now == -1)
                break;
            visited[now] = true;

            for (int next = 0; next < N; next++ )
            {
                // 연결되지 않은 정점은 스킵한다
                if (adj[now, next] == -1)
                    continue;

                // 이미 방문한 정점은 스킵
                if (visited[next])
                    continue;

                //now : 1  ,
                //next : 3
                // 새로 조사된 정점의 최단 거리를 계산한다.
                //                      15  +  10
                float nextDist = distance[now] + adj[now, next];
                // 이전까지 누적해서 계산 했던 노드 길이distance[next]보다,
                // 현재 계산하고있는 누적 길이(nextDist)가 더 작다면 next Dist 즉 더 짧은 경로로 변경한다
                if (nextDist < distance[next])
                {
                    distance[next] = nextDist;      //해당 노드의 현재 까지 짧은 거리
                    parent[next] = now;             //해당 노드의 현재까지 짧은 거리의 부모

                    //각 노드에 누적된 거리가 계속 갱신 됨으로 
                }

            }
        }


        // 최단 경로 역추적
        int end = GetIndex(_goalFlag);
        
        if (distance[end] == INF)
        {
            Debug.Log("길을 못 찾음!");
            return new();
        }
        List<Flag> path = new();
        //끝에서부터 지나온 깃발을 경로에 추가
        while(end != start)
        {
            path.Add(flags[end]);
            end = parent[end];
        }
        //시작 깃발을 경로에 추가
        path.Add(flags[start]);
        //거꾸로된 경로를 뒤집기
        path.Reverse();

        return path;
    }

    //색 초기화
    public static void ClearColors()
    {
        foreach (Flag flag in flags) flag.SetColor(Flag.EState.Normal);
    }


}
