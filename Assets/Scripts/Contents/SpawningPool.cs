using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [Header("현재 생성된 몬스터 수")]
    [SerializeField] private int monsterCount = 0;

    [Header("유지해야하는 몬스터 수")]
    [SerializeField] private int keepMonsterCount = 0;

    private Vector3 spawnPos;             // 스폰 위치
    private float spawnRadius = 15f;      // 스폰 위치부터의 영역
    private float spawnTime = 5f;         // 스폰 시간
    private int reserveCount;             // 생성 예약한 몬스터 수

    public void AddMonsterCount(int _value) => monsterCount += _value;
    public void SetKeepMonsterCount(int _count) => keepMonsterCount = _count;

    private void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    private void Update()
    {
        while(reserveCount + monsterCount < keepMonsterCount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    private IEnumerator ReserveSpawn()
    {
        reserveCount++;

        yield return new WaitForSeconds(Random.Range(0, spawnTime));

        GameObject monster = Managers.Game.Spawn(Define.WorldObject.Monster, "BananaMan");
        NavMeshAgent nav = monster.GetOrAddComponent<NavMeshAgent>();
        Vector3 randPos;

        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, spawnRadius);
            randDir.y = 0;
            randPos = spawnPos + randDir;

            // TODO: 갈 수 있는지 체크
            NavMeshPath path = new NavMeshPath();
            if (nav.CalculatePath(randPos, path)) break;
        }

        monster.transform.position = randPos;
        reserveCount--;
    }
}