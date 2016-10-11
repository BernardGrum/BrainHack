using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public float SpawnTime;
    public PlanetController GoodPlanet;
    public PlanetController BadPlanet;
    public BoxCollider2D SpawnBounds;
    public float Tolerance;

    private List<PlanetController> Planets; 

    void Awake()
    {
        Planets = new List<PlanetController>(10);
    }
    void Start()
    {
        BeginLevel();
    }

    public void BeginLevel()
    {
        InvokeRepeating("Spawn", SpawnTime, SpawnTime);
    }

    void Update(){
        foreach(PlanetController planet in Planets)
        {
            if (!planet.Spawned)
            {
                GameObject.Destroy(planet.gameObject);
                Planets.Remove(planet);
            }
        }
    }


    void Spawn()
    {
        if (Planets.Count >= Planets.Capacity)
            return;

        Vector3 planetPos = GetRandomPlanetPosition();
        PlanetController prefab = Random.Range(0, 2) == 0 ? GoodPlanet : BadPlanet;
        PlanetController planet = GameObject.Instantiate(prefab, transform, false) as PlanetController;
        planet.transform.localPosition = planetPos;
        Planets.Add(planet);

    }

    Vector3 GetRandomPlanetPosition()
    {
        Vector2 pos;
        bool valid;
        do
        {
            float maxX = SpawnBounds.bounds.extents.x;
            float maxY = SpawnBounds.bounds.extents.y;
            pos = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));

            valid = true;
            foreach (PlanetController planet in Planets)
            {
                if (Vector2.Distance(planet.transform.position, pos) < Tolerance)
                {
                    valid = false;
                    break;
                }
            }
        } while (!valid);

        return pos;
    }
}
