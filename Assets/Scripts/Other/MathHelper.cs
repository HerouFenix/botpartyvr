using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Other
{
    public static class MathHelper
    {
        public static Vector3 QuadraticBezier(Vector3 p1, Vector3 p2, Vector3 p3, float k)
        {
            k = Mathf.Clamp(k, 0.0f, 1.0f);
            float a = (1.0f - k);
            return a * (a * p1 + k * p2) + k * (a * p2 + k * p3);
        }
    }
}