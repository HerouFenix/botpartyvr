using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpression : MonoBehaviour
{
    [SerializeField] Material faceMat;
    [SerializeField] Texture[] facialExpression;

    public void Happy()
    {
        faceMat.mainTexture = facialExpression[0];
    }

    public void Sad()
    {
        faceMat.mainTexture = facialExpression[1];

    }

    public void Talking()
    {
        faceMat.mainTexture = facialExpression[2];

    }
}
