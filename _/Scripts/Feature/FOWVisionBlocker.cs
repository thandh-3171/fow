using FOW.Remaster.Core;
using UnityEngine;

namespace FOW.Remaster.Feature
{
    public class FOWVisionBlocker : MonoBehaviour
    {
        #region public vars.
        public FogOfWar.Players Faction = 0;
        public MeshRenderer meshRenderer;
        public float TurnOffDelay = 0.0f;
        #endregion

        #region private vars.
        private bool Hide = false;
        private float StartTime = 0;
        #endregion

        #region mono
        public void OnEnable()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            FogOfWar.RegisterVisionBlocker(gameObject);

            if (FogOfWar.IsPositionRevealedByFaction(transform.position, FogOfWar.RevealFaction))
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
        private void Update()
        {
            if (Hide)
            {
                if (Time.time >= StartTime + TurnOffDelay)
                    meshRenderer.enabled = false;
            }

            if (Faction != FogOfWar.RevealFaction)
            {
                if (FogOfWar.IsPositionRevealedByFaction(transform.position, FogOfWar.RevealFaction))
                {
                    if (Hide)
                    {
                        Hide = false;
                    }
                    meshRenderer.enabled = true;

                }
                else
                {
                    if (!Hide)
                    {
                        StartTime = Time.time;
                        Hide = true;
                    }
                }
            }
            else
            {
                if (!meshRenderer.enabled)
                {
                    meshRenderer.enabled = true;
                }
            }
        }
        public void OnDisable()
        {
            FogOfWar.UnRegisterVisionBlocker(gameObject);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up, "Blocker.png", true);
        }
#endif
        #endregion
    }
}