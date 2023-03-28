using FOW.Remaster.Core;
using UnityEngine;

namespace FOW.Remaster.Feature
{
    public class FOWCharacterUnit : MonoBehaviour
    {
        #region public vars.
        public float VisionRange = 6f;
        public FogOfWar.Players Faction = 0;
        [Range(0, 255)]
        public int UpVision = 10;
        public MeshRenderer MeshRenderer;
        public float TurnOffDelay = 0.0f;
        #endregion

        #region private vars.
        private Revealer revealer;
        private bool hide = false;
        private float startTime = 0;
        #endregion

        #region mono
        public void Start()
        {
            if (!MeshRenderer)
                MeshRenderer = gameObject.GetComponent<MeshRenderer>();

            revealer = new Revealer(VisionRange,
                Faction,
                UpVision,
                gameObject);

            FogOfWar.RegisterRevealer(revealer);

            if (FogOfWar.IsPositionRevealedByFaction(transform.position, FogOfWar.RevealFaction))
            {
                MeshRenderer.enabled = true;
            }
            else
            {
                MeshRenderer.enabled = false;
            }
        }
        private void Update()
        {
            if (hide)
            {
                if (Time.time >= startTime + TurnOffDelay)
                {
                    MeshRenderer.enabled = false;
                }
            }

            revealer.VisionRange = VisionRange;

            if (Faction != FogOfWar.RevealFaction)
            {
                if (FogOfWar.IsPositionRevealedByFaction(transform.position, FogOfWar.RevealFaction))
                {
                    if (hide)
                    {
                        hide = false;
                    }
                    MeshRenderer.enabled = true;

                }
                else
                {
                    if (!hide)
                    {
                        startTime = Time.time;
                        hide = true;
                    }
                }
            }
            else
            {
                if (!MeshRenderer.enabled)
                {
                    MeshRenderer.enabled = true;
                }
            }
        }
        public void OnDisable()
        {
            if (revealer != null)
                FogOfWar.UnRegisterRevealer(revealer);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position + Vector3.up, "Revealer.png", true);
        }
#endif
        #endregion

        #region public methods
        public void ChangeFaction(FogOfWar.Players _Faction)
        {
            Debug.Log("Changeing to: " + _Faction);
            FogOfWar.UnRegisterRevealer(revealer);
            revealer.Faction = _Faction;
            FogOfWar.RegisterRevealer(revealer);
        }
        #endregion
    }
}