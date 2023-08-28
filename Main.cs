using System.IO;
using ModLoader;
using ModLoader.Helpers;
using TMPro;
using UnityEngine;

namespace CNTansMod
{
    public class Main : Mod
    {
        //override mod info
        public override string ModNameID => "cntransmod";
        public override string DisplayName => "Chinese Support Mod";
        public override string Author => "AstarLC";
        public override string MinimumGameVersionNecessary => "1.5.9.8";
        public override string Description => "Load custom fonts to support chinese in the game";
        public override string ModVersion => "v0.1";

        //font assets
        public TMP_FontAsset bookAsset;
        public TMP_FontAsset heavyAsset;
        private const string exeLaunchPath = "Mods/CNSupportMod/FontAB/font-asset";
        private const string steamLaunchPath = "Spaceflight Simulator Game/Mods/CNSupportMod/FontAB/font-asset";
        private const string fontBookName = "FuturaPTBook SDF", fontHeavyName = "FuturaPTHeavy SDF";

        public override void Load()
        {
            Debug.Log(Path.Combine(Directory.GetCurrentDirectory(), steamLaunchPath));
            //Load Asset
            AssetBundle fontAsset = AssetBundle.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), steamLaunchPath));
            if(fontAsset == null)
            {
                fontAsset = AssetBundle.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), exeLaunchPath));
            }

            //Load failed
            if(fontAsset == null)
            {
                ModError("Load font asset failed.\nHmmm...Maybe you need to check whether folder 'FontAB' and file 'font-asset' exist in CNSupportMod folder.");
                return;
            }

            //Load TMP_FontAsset
            //bookAsset = fontAsset.LoadAsset<TMP_FontAsset>("HYFuturaPT SDF.asset");
            //heavyAsset = fontAsset.LoadAsset<TMP_FontAsset>("HYTMRFuturaPT-Heavy SDF.asset");
            heavyAsset = bookAsset = fontAsset.LoadAsset<TMP_FontAsset>("HYQH-70S SDF.asset");//Sample Font

            //Load failed
            if(bookAsset == null)
            {
                ModLog("Font(Book) failed to load");
            }

            if (heavyAsset == null)
            {
                ModLog("Font(Heavy) failed to load");
            }

            //fontAsset.Unload(false);
            
            ResetAllText();

            //Add Scene Delegate
            SceneHelper.OnSceneLoaded += () =>
            {
                ResetAllText();
            };
        }

        //Output mod log
        public void ModLog(string message)
        { 
            Debug.Log("CNSupportMod: " + message); 
        }

        public void ModError(string message)
        {
            Debug.LogError("CNSupportMod Error: " + message);
        }

        //Reset all TMPro_UGUI in the same scene
        public void ResetAllText()
        {
            //Get all TMP UGUI
            TextMeshProUGUI[] uguiComps = GameObject.FindObjectsOfType<TextMeshProUGUI>();

            //Reset all TMP UGUI font with font name
            foreach (TextMeshProUGUI uguiComp in uguiComps)
            {
                string uguiFontName = uguiComp.font.name;
                //ModLog(uguiFontName);
                //Reset font if font asset exist
                if (uguiFontName == fontBookName && bookAsset != null)
                {
                    uguiComp.font = bookAsset;
                }
                else if (uguiFontName == fontHeavyName && heavyAsset != null)
                {
                    uguiComp.font = heavyAsset;
                }
            }
        }
    }
}
