using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;

namespace RotateMinimapMod
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class RotateMinimapMod : BaseUnityPlugin
    {
        public const string PluginGUID = "com.source-guilherme.rotateminimapmod";
        public const string PluginName = "RotateMinimapMod";
        public const string PluginVersion = "0.0.1";

        public GameObject minimapMaskContainerPrefab; 

        private void Awake()
        {
            On.Minimap.Awake += Minimap_Awake;
           // On.Minimap.CenterMap += Minimap_CenterMap;
           // On.Minimap.UpdatePlayerMarker += Minimap_UpdatePlayerMarker;

            AssetBundle assetBundle = AssetUtils.LoadAssetBundleFromResources("rotate_minimap", typeof(RotateMinimapMod).Assembly);
            try
            {
                minimapMaskContainerPrefab = assetBundle.LoadAsset<GameObject>("MinimapMask");
            } finally
            {
                assetBundle.Unload(false);
            }
             
            Jotunn.Logger.LogInfo("RotateMinimapMod has loaded!");
        }

        private void Minimap_UpdatePlayerMarker(On.Minimap.orig_UpdatePlayerMarker orig, Minimap self, Player player, Quaternion playerRot)
        {
            if (self.m_mode == Minimap.MapMode.Small)
            {
                self.m_smallMarker.rotation = Quaternion.Euler(0, 0, 0);
                Ship controlledShip = player.GetControlledShip();
                if ((bool)controlledShip)
                {
                    self.m_smallShipMarker.gameObject.SetActive(value: true);
                    float yawShip = controlledShip.GetShipYawAngle();
                    self.m_smallShipMarker.transform.rotation = Quaternion.Euler(0, 0, yawShip);
                }
                else
                {
                    self.m_smallShipMarker.gameObject.SetActive(value: false);
                }
            } else
            {
                orig(self, player, playerRot);
            }
        }

        private void Minimap_Awake(On.Minimap.orig_Awake orig, Minimap self)
        {
            GameObject maskContainer = Object.Instantiate(minimapMaskContainerPrefab, self.m_smallRoot.transform);
            Transform container = maskContainer.transform;
            self.m_mapImageSmall.transform.SetParent(container);
            self.m_smallShipMarker.SetParent(container);
            self.m_smallMarker.SetParent(container);
            self.m_windMarker.SetParent(container);
        }

        private void Minimap_CenterMap(On.Minimap.orig_CenterMap orig, Minimap self, Vector3 centerPoint)
        {
            self.m_mapImageSmall.transform.rotation = Quaternion.Euler(0, 0, Player.m_localPlayer.m_eye.transform.rotation.eulerAngles.y);
            for (int i = 0; i < self.m_pinRootSmall.childCount; i++)
            {
                self.m_pinRootSmall.transform.GetChild(i).transform.rotation = Quaternion.identity;
            } 
            orig(self, centerPoint);
        }
    }
}