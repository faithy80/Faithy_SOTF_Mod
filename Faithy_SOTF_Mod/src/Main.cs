using Sons.Input;
using UnityEngine;
using TheForest.Utils;
using TheForest.Items.Inventory;
using Sons.Items.Core;
using UnityEngine.SceneManagement;
using Il2CppSystem.Collections.Generic;

namespace Faithy_SOTF_Mod
{
    public class Main
    {
        public class MyMonoBehaviour : MonoBehaviour
        {
            static List<ItemData>? itemList;
            static bool isInitialized = false;
            public static Scene _sonsMainScene;
            private Vector2 scrollPosition = Vector2.zero;
            private Rect windowRect = new Rect(10, 100, 300, 1000);

            private void OnGUI()
            {

                if (!Settings.Visible) return;
                GUI.color = Color.white;

                //Show All ID's
                UIHelper.Begin("All Items", 10, 10, 300, 86, 2, 20, 2);
                Buttons.ShowAllIDs = UIHelper.Button("Show All ID's");
                Settings.X100 = UIHelper.Button("x100: ", Settings.X100);
                Settings.X1000 = UIHelper.Button("x1000: ", Settings.X1000);

                //Toggle ID window
                GUI.backgroundColor = Color.grey;
                if (Settings.ShowItemIDs)
                    windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)ShowAllIDsWindow, "Show ID's");

                //Weapon Spawner
                UIHelper.Begin("Weapon", 310, 10, 100, 152, 2, 20, 2);
                Buttons.Pistol = UIHelper.Button("Pistol");
                Buttons.Revolver = UIHelper.Button("Revolver");
                Buttons.Shotgun = UIHelper.Button("Shotgun");
                Buttons.Rifle = UIHelper.Button("Rifle");
                Buttons.Machete = UIHelper.Button("Machete");
                Buttons.Katana = UIHelper.Button("Katana");

                //Weapon Upgrades Spawner
                UIHelper.Begin("Weapon Upgrades", 410, 10, 165, 152, 2, 20, 2);
                Buttons.PistolRail = UIHelper.Button("Pistol Rail");
                Buttons.PistolSuppressor = UIHelper.Button("Pistol Suppressor");
                Buttons.ShotgunRail = UIHelper.Button("Shotgun Rail");
                Buttons.RifleRail = UIHelper.Button("Rifle Rail");
                Buttons.ScopeMod = UIHelper.Button("Scope Mod");
                Buttons.LaserLightMod = UIHelper.Button("Laser Light Mod");

                //Ammo Spawner
                UIHelper.Begin("Ammo", 580, 10, 165, 86, 2, 20, 2);
                Buttons._9mmAmmo = UIHelper.Button("9mm Ammo");
                Buttons.ShotgunSlugAmmo = UIHelper.Button("Shotgun Slug Ammo");
                Buttons.RifleAmmo = UIHelper.Button("Rifle");

                //Material Spawner
                UIHelper.Begin("Material", 750, 10, 95, 130, 2, 20, 2);
                Buttons.Log = UIHelper.Button("Log");
                Buttons.HalfLog = UIHelper.Button("Half Log");
                Buttons.Plank = UIHelper.Button("Plank");
                Buttons.Stone = UIHelper.Button("Stone");
                Buttons.TurtleShell = UIHelper.Button("Turtle Shell");

                //Character Spawner
                UIHelper.Begin("Character", 850, 10, 95, 64, 2, 20, 2);
                Buttons.Kelvin = UIHelper.Button("Kelvin");
                Buttons.Virginia = UIHelper.Button("Virginia");


                //Show All ID's
                if (Buttons.ShowAllIDs)
                    Settings.ShowItemIDs = !Settings.ShowItemIDs;

                if (Settings.X100 && Settings.X1000)
                {
                    Settings.X100 = !Settings.X100;
                    Settings.X1000 = !Settings.X1000;
                }

                //Weapons functions
                if (Buttons.Pistol)
                    SpawnItem(355, 1);
                if (Buttons.Revolver)
                    SpawnItem(386, 1);
                if (Buttons.Shotgun)
                    SpawnItem(358, 1);
                if (Buttons.Rifle)
                    SpawnItem(361, 1);
                if (Buttons.Machete)
                    SpawnItem(359, 1);
                if (Buttons.Katana)
                    SpawnItem(367, 1);

                //Weapons Upgrade functions
                if (Buttons.PistolRail)
                    SpawnItem(376, 1);
                if (Buttons.PistolSuppressor)
                    SpawnItem(374, 1);
                if (Buttons.ShotgunRail)
                    SpawnItem(346, 1);
                if (Buttons.RifleRail)
                    SpawnItem(383, 1);
                if (Buttons.ScopeMod)
                    SpawnItem(377, 1);
                if (Buttons.LaserLightMod)
                    SpawnItem(375, 1);

                //Ammo functions
                if (Buttons._9mmAmmo)
                {
                    var amount = 1;
                    if (Settings.X100)
                        amount = 100;
                    else if (Settings.X1000)
                        amount = 1000;
                    SpawnItem(362, amount);
                }
                if (Buttons.ShotgunSlugAmmo)
                {
                    var amount = 1;
                    if (Settings.X100)
                        amount = 100;
                    else if (Settings.X1000)
                        amount = 1000;
                    SpawnItem(372, amount);
                }
                if (Buttons.RifleAmmo)
                {
                    var amount = 1;
                    if (Settings.X100)
                        amount = 100;
                    else if (Settings.X1000)
                        amount = 1000;
                    SpawnItem(387, amount);
                }

                //Material functions
                if (Buttons.Log)
                    SpawnItem(78, 1);
                if (Buttons.HalfLog)
                    SpawnItem(408, 1);
                if (Buttons.Plank)
                    SpawnItem(395, 1);
                if (Buttons.Stone)
                    SpawnItem(640, 1);
                if (Buttons.TurtleShell)
                    SpawnItem(506, 1);

                //Characters functions
                if (Buttons.Kelvin)
                    SpawnCharacter("Robby");
                if (Buttons.Virginia)
                    SpawnCharacter("Virginia");
            }

            private void Update()
            {
                RegisterHandlers();

                //cache SonsMainScene
                if (!_sonsMainScene.isLoaded) _sonsMainScene = SceneManager.GetSceneByName("SonsMain");
            }

            private void ShowMenu()
            {
                if (Input.GetKeyDown(Plugin.ModMenuKeybind.Value))
                {
                    Settings.Visible = !Settings.Visible;
                    if (Settings.Visible)
                    {
                        InputSystem.SetState(0, true);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        return;
                    }
                    if (LocalPlayer.IsInWorld || LocalPlayer.IsInInventory || LocalPlayer.IsConstructing || LocalPlayer.IsInMidAction || LocalPlayer.CurrentView == PlayerInventory.PlayerViews.Hidden)
                    {
                        InputSystem.SetState(0, false);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                }
            }

            public void ShowAllIDsWindow(int windowID)
            {
                if (_sonsMainScene.isLoaded)
                {
                    if (!isInitialized)
                    {
                        itemList ??= ItemDatabaseManager.Items;
                        isInitialized = true;
                    }
                }

                if (itemList == null || itemList.Count == 0)
                {
                    GUI.Label(new Rect(5, 15, 300, 1000), "Item list is empty.");
                }
                else
                {
                    string buttonLabel;

                    GUILayout.BeginArea(new Rect(5, 15, 295, 980));
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    GUILayout.BeginVertical();

                    foreach (ItemData item in itemList)
                    {
                        buttonLabel = $"{item._name} : {item._id}";
                        if (GUILayout.Button(buttonLabel))
                            SpawnItem(item._id, 1);
                    }

                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
            }

            private void RegisterHandlers()
            {
                ShowMenu();
            }

            private void SpawnItem(int itemID, int amount)
            {
                try
                {
                    LocalPlayer.Inventory.AddItem(itemID, amount);
                }
                catch
                {
                    //Log.LogError("Failed to add item!");
                }
            }

            private void SpawnCharacter(string character)
            {
                string result = character + " 1";

                try
                {
                    Sons.Characters.CharacterManager.Instance.DebugAddCharacter(result);
                }
                catch
                {
                    //Log.LogError("Failed to spawn character!");
                }
            }
        }
    }
}