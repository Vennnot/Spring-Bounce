using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public float speed = 1f;
    public Direction chosenDirection = 0;
    private Vector3 movement = Vector3.zero;

    public Arrow(int x, float y)
    {
        this.chosenDirection = (Direction) x;
        this.speed = y;
    }

    // Start is called before the first frame update
    void Start()
    {
       RotateInDirection(chosenDirection); 
       movement = MoveInDirection(chosenDirection);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=  speed * Time.deltaTime * movement;
        Despawn();
    }
    
    void Despawn()
    {
        if(transform.position.y < -200 ||transform.position.y > 200 ||transform.position.x < -200 ||transform.position.x > 200 )
        {
            Destroy(gameObject);
        }
    }

    Vector3 MoveInDirection(Direction d)
    {
        Vector3 movement = Vector3.zero;
        switch (chosenDirection)
        {
            case Direction.Up:
                movement= Vector3.up;
                break;
            case Direction.Down:
                movement= Vector3.down;
                break;
            case Direction.Left:
                movement= Vector3.left;
                break;
            case Direction.Right:
                movement= Vector3.right;
                break;
        }

        return movement;
    }

    void RotateInDirection(Direction d)
    {
        switch (chosenDirection)
        {
           case Direction.Up:
               break;
           case Direction.Down:
               transform.Rotate(0,0,180);
               break;
           case Direction.Left:
               transform.Rotate(0,0,90);
               break;
           case Direction.Right:
               transform.Rotate(0,0,270);
               break;
        } 
    }
}
