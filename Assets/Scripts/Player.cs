using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Controller2D))]

public class Player : MonoBehaviour {
    Vector3 velocity;
    float gravity = -20;
    float moveSpeed = 6;
    Controller2D controller;
	
    // Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();

	
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity.x = input.x * moveSpeed;

        velocity.y += gravity * Time.deltaTime;
        controller.move(velocity * Time.deltaTime);
	}
}
