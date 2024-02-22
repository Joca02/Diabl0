using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject monsterPrefab;


    public GameObject dragonSpawner;
    public GameObject dragonPrefab;
    public static int totalMonsters;
    public void GenerateMonsters()
    {
        totalMonsters=1;
        Instantiate(monsterPrefab, spawners[0].transform.position, Quaternion.identity);    //prvi monster koji je kao uvod u igricu
        if (spawners.Length > 1)
        {

            for (int i = 1; i < spawners.Length; i++)
            {
                float spawnX = spawners[i].transform.position.x;
                float spawnZ = spawners[i].transform.position.z;
                int numberOfMonstersToSpawn = Random.Range(4, 10);
                totalMonsters += numberOfMonstersToSpawn;
                for (int j = 0; j < numberOfMonstersToSpawn; j++)
                {
                    //generisem random vrednosti za x i z koje tako da se monsteri stvore blizu spawna
                    float x = Random.Range(spawnX-6, spawnX+6);
                    float z = Random.Range(spawnZ-6, spawnZ+6);

                    Instantiate(monsterPrefab, new Vector3(x, 0, z), Quaternion.identity);
                }

            }
        }
        Instantiate(dragonPrefab, dragonSpawner.transform.position, Quaternion.Euler(0,-180f,0));//da bi dragon bio rotiran ka meni, da mu ne gledam d
    }

   
}
