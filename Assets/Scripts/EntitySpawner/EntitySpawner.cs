﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    //subclass to store data about the enemy
    public class Enemy
    {
        private GameObject enemy;        //variable to store the Enemy type
        private Transform pos;           //varibale to store the spawn position
        private int spawnCount;          //store how many times this Entity has been spawned

        public Enemy(GameObject prefab, Transform position)
        {
            //construct the Entity
            enemy = prefab;
            pos = position;
            spawnCount = 0;      //intialize amount of spawns to 0
        }

        //Spawn a single entity
        public void Spawn()
        {
            Instantiate(enemy, pos);
            spawnCount += 1;
        }

        //Spawn a single entity given a new position, overides the intial one temporarily
        public void Spawn(Transform tempPos)
        {
            Instantiate(enemy, tempPos);
            spawnCount += 1;
        }

    }

    //available to level designers
    [SerializeField] List<GameObject> prefabs;      //Entities that are in the Spawn Group
    [SerializeField] List<Transform> positions;     //Locations to spawn each Entity
    [SerializeField] Boolean spawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean spawnnTrigger;         //Spawn Entities when player enters collider trigger
    [SerializeField] Boolean shufflePositions;      //shuffle the positions each time an Entity is spawned


    Collider2D colliderTrigger;

    //made automatically with prefabs and positions
    List<Enemy> entities;


    // Start is called before the first frame update
    void Start()
    {
        
        entities = new List<Enemy>();   //instantiate the entities list

        //create the list of Entities matching the prefabs with the corresponding positions
        for(int x = 0; x < prefabs.Count; x++)
        {
            entities.Add(new Enemy(prefabs[x], positions[x]));
        }

        if (spawnOnStart)   //if designer sets this to true
            SpawnGroup();
    }

    //spawns the entity SpawnGroup
    void SpawnGroup()
    {
        if (shufflePositions)    //if the deisgner chose to shuffle positions
        {
            positions = Shuffle(positions);

            //traverse through the list and spawn each entity with a shuffled position
            for (int x = 0; x < entities.Count; x++)
            {
                entities[x].Spawn(positions[x]);
            }
        }
        else
            foreach (Enemy enemy in entities)
                enemy.Spawn();
    }

    //shuffles 
    private List<Transform> Shuffle(List<Transform> orderedPos)
    {
        List<Transform> shuffPos = new List<Transform>();       //new shuffled positions
        System.Random rand = new System.Random();
        int x;

        while (orderedPos.Count > 0)
        {
            //get a randome int range [0, list size)
            x = rand.Next(orderedPos.Count);

            //assign the random selected Transfor as next element in list
            shuffPos.Add(orderedPos[x]);
            orderedPos.RemoveAt(x);      //remove it so it isnt picked again
            
        }

        return shuffPos;

    }

    //
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            if (spawnnTrigger)
                SpawnGroup();
    }




    // Update is called once per frame
    void Update()
    {
        
    }

}
