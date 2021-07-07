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

        private void Awake()
        {
            On.Minimap.CenterMap += Minimap_CenterMap;
            On.Minimap.Awake += Minimap_Awake;
            On.Minimap.UpdatePlayerMarker += Minimap_UpdatePlayerMarker;
            Jotunn.Logger.LogInfo("RotateMinimapMod has loaded!");
        }

        private void Minimap_UpdatePlayerMarker(On.Minimap.orig_UpdatePlayerMarker orig, Minimap self, Player player, Quaternion playerRot)
        {
            if (self.m_mode == Minimap.MapMode.Small)
            {
                self.m_smallMarker.rotation = Quaternion.Euler(0, 0, 0);
                self.m_smallShipMarker.gameObject.SetActive(value: false);
            } else
            {
                
            }
        }

        private void Minimap_Awake(On.Minimap.orig_Awake orig, Minimap self)
        {
            self.m_pinRootSmall.SetParent(self.m_smallRoot.transform);
            self.m_smallMarker.SetParent(self.m_smallRoot.transform);
            self.m_windMarker.SetParent(self.m_smallRoot.transform);
            self.m_smallShipMarker.SetParent(self.m_smallRoot.transform);
        }

        private void Minimap_CenterMap(On.Minimap.orig_CenterMap orig, Minimap self, Vector3 centerPoint)
        {
            self.m_mapImageSmall.transform.rotation = Quaternion.Euler(0, 0, Player.m_localPlayer.m_eye.transform.rotation.eulerAngles.y);
            self.m_pinRootSmall.transform.rotation = Quaternion.Euler(0, 0, Player.m_localPlayer.m_eye.transform.rotation.eulerAngles.y-360);
            orig(self, centerPoint);
        }
    }
}