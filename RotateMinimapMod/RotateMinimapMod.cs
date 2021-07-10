using BepInEx;
using UnityEngine;
using System.IO;
using System.Collections;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;

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
        private AssetBundle customMask;
        public Sprite RoundSprite { get; private set; }

        private void Awake()
        {
            On.Minimap.Awake += Minimap_Awake;
            On.Minimap.CenterMap += Minimap_CenterMap;
            On.Minimap.UpdatePlayerMarker += Minimap_UpdatePlayerMarker;
            Jotunn.Logger.LogInfo("RotateMinimapMod has loaded!");

            customMask = AssetUtils.LoadAssetBundleFromResources("finalmask", Assembly.GetExecutingAssembly());
            RoundSprite = customMask.LoadAsset<Sprite>("roundmask");
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
            self.m_pinRootSmall.SetParent(self.m_smallRoot.transform);
            self.m_smallShipMarker.SetParent(self.m_smallRoot.transform);
            self.m_smallMarker.SetParent(self.m_smallRoot.transform);
            self.m_windMarker.SetParent(self.m_smallRoot.transform);
        }

        private void Minimap_CenterMap(On.Minimap.orig_CenterMap orig, Minimap self, Vector3 centerPoint)
        {
            // Prefix
            Quaternion playerRotation = Player.m_localPlayer.m_eye.rotation;
            self.m_mapImageSmall.transform.rotation = Quaternion.Euler(0, 0, playerRotation.eulerAngles.y);
            self.m_pinRootSmall.rotation = Quaternion.Euler(0, 0, playerRotation.eulerAngles.y);
            foreach (Transform child in self.m_pinRootSmall.transform)
            {
                child.rotation = Quaternion.identity;
            }
            orig(self, centerPoint);
        }
    }
}