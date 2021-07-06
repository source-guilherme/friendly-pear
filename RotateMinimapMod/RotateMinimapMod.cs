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
        //private ConfigEntry<float> FloatConfig;

        private void Awake()
        {
            //FloatConfig = Config.Bind<float>("Minimap Zoom", "Zoom Values", 1f, new ConfigDescription("This will change the Field of View in the game.", new AcceptableValueRange<float>(0f, 2f)));
            On.Minimap.CenterMap += Minimap_CenterMap;
            On.Minimap.Awake += Minimap_Awake;
            //On.Minimap.UpdateMap += Minimap_UpdateMap;
            Jotunn.Logger.LogInfo("RotateMinimapMod has loaded!");
        }

        //private void Minimap_UpdateMap(On.Minimap.orig_UpdateMap orig, Minimap self, Player player, float dt, bool takeInput)
        //{
        //    self.m_smallZoom = 0.01f * FloatConfig.Value;
        //}

        private void Minimap_Awake(On.Minimap.orig_Awake orig, Minimap self)
        {
            self.m_pinRootSmall.SetParent(self.m_smallRoot.transform);
            self.m_smallMarker.SetParent(self.m_smallRoot.transform);
            self.m_windMarker.SetParent(self.m_smallRoot.transform);
            self.m_smallShipMarker.SetParent(self.m_smallRoot.transform);
        }

        private void Minimap_CenterMap(On.Minimap.orig_CenterMap orig, Minimap self, Vector3 centerPoint)
        {
            self.m_smallRoot.transform.Find("map").rotation = Quaternion.Euler(0, 0, Player.m_localPlayer.m_eye.transform.rotation.eulerAngles.y);
            self.m_pinRootSmall.transform.rotation = Quaternion.Euler(0, 0, Player.m_localPlayer.m_eye.transform.rotation.eulerAngles.y-360);
            self.m_smallMarker.transform.rotation = Quaternion.Euler(0, 0, 0);
            orig(self, centerPoint);
        }
    }
}