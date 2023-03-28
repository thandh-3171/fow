using FOW.Remaster.Core;
using FOW.Remaster.Feature;
using UnityEngine;

namespace FOW.Remaster.Faction
{
    public class ChangeFaction : MonoBehaviour
    {
        #region public vars
        public FogOfWarManager FOWManager;
        public FogOfWar.Players FactionToSet = 0;
        #endregion

        #region mono
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Revealing Faction 1");
                FOWManager.ShowFaction(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Revealing Faction 2");
                FOWManager.ShowFaction(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("Revealing Faction 3");
                FOWManager.ShowFaction(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("Revealing Faction 4");
                FOWManager.ShowFaction(3);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit = new RaycastHit();

                if (Physics.Raycast(ray, out rayHit, 100f))
                {
                    FOWCharacterUnit e = rayHit.transform.gameObject.GetComponent<FOWCharacterUnit>();

                    if (e != null)
                    {
                        e.ChangeFaction(FactionToSet);
                    }
                }
            }
        }
        #endregion
    }
}