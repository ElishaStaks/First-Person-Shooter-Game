using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] m_spawnPoints;

    [Header("References")]
    public GameObject m_monsterPrefab;

    private bool isStopSpawning = false;

    public float m_spawnTime;
    public float m_spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnMonsters", m_spawnTime, m_spawnDelay);
    }

    private void SpawnMonsters()
    {
        Transform rand = m_spawnPoints[Random.Range(0, m_spawnPoints.Length - 1)];
        Instantiate(m_monsterPrefab, rand.position, Quaternion.identity);
        if (isStopSpawning)
        {
            CancelInvoke("SpawnMonsters");
        }
    }
}
