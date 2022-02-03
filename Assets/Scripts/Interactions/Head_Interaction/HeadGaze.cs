using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Interactions.Head_Interaction
{
    public class HeadGaze : MonoBehaviour
    {
        
        [SerializeField] private Camera _centerEye;
        [SerializeField] private UnityEvent _onYes;
        [SerializeField] private UnityEvent _onNo;
        [SerializeField] private UnityEvent _onLeave;

        [SerializeField] private Transform _reticle;

        private bool yesTriggered = false;
        private bool noTriggered = false;

        GameObject eventSystem;

        private void Start()
        {
            eventSystem = GameObject.Find("EventSystem");
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 recticlePosition = _reticle.position;

            float distance;

            RaycastHit hitInfo;
            if (Physics.Raycast(_centerEye.transform.position, _centerEye.transform.forward, out hitInfo, 100.0f))
            {

                distance = hitInfo.distance;

                //Debug.Log("Ray hit!");
                if (hitInfo.collider.CompareTag("Yes"))
                {
                    yesTriggered = true;
                    hitInfo.collider.transform.parent.GetComponent<Button>().Select();
                    _onYes.Invoke();
                }
                else if (hitInfo.collider.CompareTag("No"))
                {
                    noTriggered = true;
                    hitInfo.collider.transform.parent.GetComponent<Button>().Select();
                    _onNo.Invoke();
                }
                else
                {
                    if(yesTriggered || noTriggered)
                    {
                        _onLeave.Invoke();
                        yesTriggered = false;
                        noTriggered = false;
                        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    }
                }
            } 
            else
            {
                distance = _centerEye.farClipPlane * 0.95f;
            }

            //Debug.Log(distance);

            Vector3 newPosition = _centerEye.transform.position + _centerEye.transform.forward * distance;
            if ((recticlePosition - newPosition).magnitude > 0.0001)
            {
                _reticle.position = _centerEye.transform.position + _centerEye.transform.forward * distance;
            }
            _reticle.rotation = _centerEye.transform.rotation;
            _reticle.localScale = Vector3.one * distance;

            Debug.DrawRay(_centerEye.transform.position, _centerEye.transform.forward * 100.0f);
        }
    }
}