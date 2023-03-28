using FOW.Remaster.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FOW.Remaster.Feature
{
    public class FOWCharacterSprite : MonoBehaviour
    {
        #region public vars.
        public float VisionRange = 6f;
        public FogOfWar.Players Faction = 0;
        [Range(0, 255)]
        public int UpVision = 0;
        public Image Image;
        public GameObject[] Effects;
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
            if (!Image)
                Image = gameObject.GetComponent<Image>();

            revealer = new Revealer(VisionRange,
                Faction,
                UpVision,
                gameObject);

            FogOfWar.RegisterRevealer(revealer);

            if (FogOfWar.IsPositionRevealedByFaction(transform.position, FogOfWar.RevealFaction))
            {
                Image.enabled = true;
            }
            else
            {
                Image.enabled = false;
            }
        }
        private void Update()
        {
            if (Image.enabled)
            {
                foreach (GameObject g in Effects)
                {
#pragma warning disable 618
                    g.GetComponent<ParticleSystem>().enableEmission = true;

                }
            }
            else
            {
                foreach (GameObject g in Effects)
                {
#pragma warning disable 618
                    g.GetComponent<ParticleSystem>().enableEmission = false;
#pragma warning restore 618
                }
            }

            if (hide)
            {
                if (Time.time >= startTime + TurnOffDelay)
                {
                    Image.enabled = false;
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
                    Image.enabled = true;

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
                if (!Image.enabled)
                {
                    Image.enabled = true;
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
    }
}