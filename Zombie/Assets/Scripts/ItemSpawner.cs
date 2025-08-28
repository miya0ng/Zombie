using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;
public class ItemSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float range = 10f;
    private int spawnCount = 15;
    Vector3 center = Vector3.zero;
    Vector3 result;
    public Item[] items;

    //GameManager gameManager;
    public void Start()
    {
        CreateItem();
        //for(int i = 0; i< 3; i++)
        //{
        //    Instantiate(items[i], new Vector3(10, 10, 4), Quaternion.identity);
        //}
    }

    // Update is called once per frame
    public void Update()
    {

    }

    private void SpawnWave()
    {

        //CreateItem();

    }

    public bool SpawnPosition(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    public void CreateItem()
    {
        //var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //var zombie = Instantiate(prefab, point.position, point.rotation);
        //zombie.SetUp(zombieDatas[Random.Range(0, zombieDatas.Length)]);
        //zombies.Add(zombie);

        //zombie.OnDeath += () => zombies.Remove(zombie);
        //zombie.OnDeath += () => uiManager.SetWaveInfo(wave, zombies.Count);
        //zombie.OnDeath += () => Destroy(zombie.gameObject, 5f);
        for (int i = 0; i < 15; i++)
        {
            if (SpawnPosition(center, range, out result))
            {
                var pos = result;
                pos.y += 0.5f;
                Instantiate(items[Random.Range(0, items.Length)], pos, Quaternion.identity);
            }
        }
    }
}