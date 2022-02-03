using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownBobbing : MonoBehaviour
{
    //adjust this to change speed
    public float speed = 0.8f;

    //adjust this to change how high it goes
    public float height = 0.15f;

    public float offset = 0;

    public bool startGoingUp = true;

    void Update()
    {
        //get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.position;

        //calculate what the new Y position will be
        float newY;
        if (startGoingUp)
            newY = Mathf.Sin(Time.time * speed);
        else
            newY = -Mathf.Sin(Time.time * speed);

        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, newY * height + offset, pos.z);
    }
}
