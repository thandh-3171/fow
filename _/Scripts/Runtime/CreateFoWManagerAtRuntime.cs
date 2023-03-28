using FOW.Remaster.Core;
using FOW.Remaster.Feature;
using UnityEngine;

namespace FOW.Remaster.Runtime
{
    public class CreateFoWManagerAtRuntime : MonoBehaviour
    {
        #region public vars
        public int Width = 128;
        public int Height = 128;
        #endregion

        #region private vars
        private FogOfWarManager manager;
        #endregion

        #region mono
        private void OnEnable()
        {
            CreateFoWManager();
            CreateFogReceiver();
        }
        #endregion

        #region private methods
        private void CreateFoWManager()
        {
            //Step 1 Prepare Resistance map
            //Here you can add your Level data!
            //If there's no height or static blocker or modifier, it can be left completely black
            Color[] c = new Color[Width * Height];

            for (int i = 0; i < c.Length; i++)
            {
                c[i] = new Color(0f, 0f, 0f, 1f);       //R = Hard blockers, G = Height Blockers, B = Modifiers, A = Empty (May be used in future updates)
            }

            //Create the FoW Manager
            //Only one manager per scene!
            GameObject g = new GameObject("Fog of War Manager");
            manager = g.AddComponent<FogOfWarManager>();

            //Set the Resistance Map
            manager.SetResistenceMapData(c);

            //Forward Settings:
            //In order to get the style right, simply create a test scene and change the 
            //settings you want to have. Then forward them here.
            //Here's an example setting, most common for startegy games
            manager.fogAlignment = FogOfWar.FogAlignment.Horizontal;
            manager.LevelWidth = Width;
            manager.LevelHeight = Height;
            manager.RevealOpacities = new Color(1f, .5f, .1f, 1f);
            manager.StartEndHeight = new Vector2(0.0f, 5.0f);
            manager.FogTextureFilterMode = FilterMode.Bilinear;
            manager.BlurFog = FogOfWar.FogQuality.On;
            manager.ManualMode = true;


            //Initialize to trigger the execution of the changes
            manager.InitializeMaps();


        }
        private void CreateFogReceiver()
        {
            //Add a second facation. Faction 0 is default. Needs to be called AFTER InitializeMaps();
            manager.AddFaction();

            //Create Fake Terrain
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Quad);
            g.transform.position = new Vector3(Width / 2f, 0f, Height / 2f);
            g.transform.localScale = new Vector3(Width, Height, 1f);
            g.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            Material mat = new Material(Shader.Find("UFoW/Standard"));
            g.GetComponent<MeshRenderer>().material = mat;

            //Add Revealer of Faction 1
            GameObject R1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            R1.transform.position = new Vector3(Width / 2f, 0f, Height / 2f);
            R1.name = "Revealer 1";
            FOWCharacterUnit EU1 = R1.AddComponent<FOWCharacterUnit>();
            EU1.Faction = FogOfWar.Players.Player00;

            //Add Revealer of Faction 2
            GameObject R2 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            R2.transform.position = new Vector3(Width / 2f, 0f, Height / 2f);
            R2.name = "Revealer 2";
            FOWCharacterUnit EU2 = R2.AddComponent<FOWCharacterUnit>();
            EU2.Faction = FogOfWar.Players.Player01;
        }
        #endregion
    }
}