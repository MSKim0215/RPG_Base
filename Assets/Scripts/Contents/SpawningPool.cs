using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [Header("���� ������ ���� ��")]
    [SerializeField] private int monsterCount = 0;

    [Header("�����ؾ��ϴ� ���� ��")]
    [SerializeField] private int keepMonsterCount = 0;

    private Vector3 spawnPos;             // ���� ��ġ
    private float spawnRadius = 15f;      // ���� ��ġ������ ����
    private float spawnTime = 5f;         // ���� �ð�
    private int reserveCount;             // ���� ������ ���� ��

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

            // TODO: �� �� �ִ��� üũ
            NavMeshPath path = new NavMeshPath();
            if (nav.CalculatePath(randPos, path)) break;
        }

        monster.transform.position = randPos;
        reserveCount--;
    }
}