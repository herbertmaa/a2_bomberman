﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    public Animator animator;
    public float moveSpeed = 5f;
    public NetworkManager networkManager = null;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        networkManager = GameObject.FindWithTag("MainCamera").GetComponent<NetworkManager>();
        if (networkManager == null) Debug.Log("Unable to get a reference of the Network Manager");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                // Keyleft / KeyRight was pressed
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    Vector2 v2 = movePoint.position;
                    BroadcastMovement(v2.x, v2.y);

                }
            } else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {

                // Keydown / Keyup was pressed
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    Vector2 v2 = movePoint.position;              
                    BroadcastMovement(v2.x, v2.y);

                }
            }

            animator.SetBool("Moving", false);
        } else
        {
            animator.SetBool("Moving", true);
        }
    }


    private void BroadcastMovement(float x, float y)
    {
        Debug.Log("player is sending a message: " + x + " " + y);
        string message = "move " + x + " " + y;
        networkManager.BroadCastMessage(message);

    }
}
