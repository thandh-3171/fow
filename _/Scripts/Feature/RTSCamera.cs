using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

namespace FOW.Remaster.Feature
{
    public class RTSCamera : MonoBehaviour
    {
        public Camera Cam;
        public Vector4 Bounds;
        public NavMeshAgent Agent;
        private Vector3 LastPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LastPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                transform.Translate(new Vector3(Input.mousePosition.x - LastPos.x, 0, Input.mousePosition.y - LastPos.y) * Time.deltaTime);
            }

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, Bounds.x, Bounds.y);
            pos.z = Mathf.Clamp(pos.z, Bounds.z, Bounds.w);
            transform.position = pos;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit = new RaycastHit();
                if (Physics.Raycast(ray, out rayHit))
                {
                    NavMeshAgent t = rayHit.transform.gameObject.GetComponent<NavMeshAgent>();
                    if (t != null)
                        Agent = t;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit = new RaycastHit();

                if (Physics.Raycast(ray, out rayHit))
                {
                    if (Agent != null && rayHit.transform.tag == "Terrain")
                    {
                        NavMeshHit hit = new NavMeshHit();
                        NavMesh.SamplePosition(rayHit.point, out hit, 2f, 1);
                        Agent.SetDestination(hit.position);
                    }


                }
            }
        }
    }
}