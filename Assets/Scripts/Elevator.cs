﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Starting Postion:")]
    [SerializeField] bool startAtTop;
    [SerializeField] bool startAtButtom;

    [SerializeField] Transform top;
    [SerializeField] Transform bottom;
    [SerializeField] float speed;

    PlatformerController playerController;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        playerController = PlatformerController.instance;
        player = playerController.gameObject;

        if (startAtTop)
        {
            transform.position = top.position;
        } 
        else if (startAtButtom)
        {
            transform.position = bottom.position;
        }
    }


    [EasyButtons.Button]
    public void MoveUp()
    {
        //Coroutine that moves the platform to the top slowly 
        StartCoroutine(MoveTowrds(top.position));
    }
    [EasyButtons.Button]
    public void MoveDown()
    {
        //Coroutine that moves the platform to the bottom slowly 
        StartCoroutine(MoveTowrds(bottom.position));
    }


    IEnumerator MoveTowrds(Vector2 targetPos)
    {
        Vector2 currentPos = transform.position;
       
        while (currentPos != targetPos)
        {
            currentPos = transform.position;    
            transform.position = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);   
            
            yield return 0;
        }    

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlatformerController>())
        {
            player.transform.SetParent(transform);
        }

        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlatformerController>())
        {
            player.transform.SetParent(null);
        }

    }
    
}
