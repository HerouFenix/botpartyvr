using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjuster : MonoBehaviour
{
    private OVRCameraRig player;
    private Transform centerEyeAnchor;
    public float offset;
    public float minHeight;
    public float maxHeight;

    [HideInInspector] public bool hasBeenAdjusted;

    IEnumerator Start()
    {
        
        player = FindObjectOfType<OVRCameraRig>();
        centerEyeAnchor = player.transform.Find("TrackingSpace/CenterEyeAnchor");

        while (centerEyeAnchor.position.y <= 0.0f)
        {
            yield return null;
        }


        float newHeight = Mathf.Clamp(centerEyeAnchor.transform.position.y + offset, minHeight, maxHeight);
        

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = new Vector3(rb.position.x, newHeight, rb.position.z);
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newHeight, this.gameObject.transform.position.z);
        }
        else
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newHeight, this.gameObject.transform.position.z);
        }

        hasBeenAdjusted = true;
    }

    public void ResetHeight()
    {

        float newHeight = Mathf.Clamp(centerEyeAnchor.transform.position.y + offset, minHeight, maxHeight);

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log(newHeight);
            rb.position = new Vector3(rb.position.x, newHeight, rb.position.z);
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newHeight, this.gameObject.transform.position.z);
        }
        else
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newHeight, this.gameObject.transform.position.z);
        }

        hasBeenAdjusted = true;
    }


}
