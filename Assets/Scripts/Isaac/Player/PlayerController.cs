﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float movementSpeed;

    Vector3 moveTo;
    CameraController cam;


    private void Start()
    {
        movementSpeed = PlayerStats.instance.MoveSpeed;
        cam = Camera.main.GetComponent<CameraController>();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("a"))
        {
            gameObject.transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
            gameObject.GetComponent<Animator>().SetBool("movingLeft", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);

        }

        if (Input.GetKeyUp("a"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingLeft", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);

        }

        if (Input.GetKey("d"))
        {
            gameObject.transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
            gameObject.GetComponent<Animator>().SetBool("movingRight", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);

        }

        if (Input.GetKeyUp("d"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingRight", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);

        }

        if (Input.GetKey("w"))
        {
            gameObject.transform.Translate(0, movementSpeed * Time.deltaTime, 0);
            gameObject.GetComponent<Animator>().SetBool("movingUp", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);


        }
        if (Input.GetKeyUp("w"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingUp", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);

        }

        if (Input.GetKey("s"))
        {
            gameObject.transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
            gameObject.GetComponent<Animator>().SetBool("movingDown", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);


        }

        if (Input.GetKeyUp("s"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingDown", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);

        }


    }
    //
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorExitZone"))
        {

            ExitZone exitZone = collision.GetComponent<ExitZone>();
            Room roomToSpawn = exitZone.roomToSpawn;
            if (roomToSpawn)
            {
                cam.MoveCameraTo(roomToSpawn.transform.position);
                if (!roomToSpawn.playerEntered)
                {
                    roomToSpawn.playerEntered = true;
                    roomToSpawn.Invoke("StartRoom", 1f);
                }
                this.gameObject.transform.position = exitZone.playerSpawnPosition;
            }
        }
    }
}