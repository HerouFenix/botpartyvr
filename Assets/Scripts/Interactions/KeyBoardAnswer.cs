using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactions
{
    public class KeyBoardAnswer : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onYes;
        [SerializeField] private UnityEvent _onNo;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _onYes.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                _onNo.Invoke();
            }
        }
    }
}