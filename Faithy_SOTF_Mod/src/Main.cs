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

                if (UIHelper.Button("Show All ID's"))
                    Settings.ShowItemIDs = !Settings.ShowItemIDs;
                
                Settings.X100 = UIHelper.Button("x100: ", Settings.X100);
                Settings.X1000 = UIHelper.Button("x1000: ", Settings.X1000);
                if (Settings.X100 && Settings.X1000)
                {
                    Settings.X100 = !Settings.X100;
                    Settings.X1000 = !Settings.X1000;
                }

                //Toggle ID window
                GUI.backgroundColor = Color.grey;
                if (Settings.ShowItemIDs)
                    windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)ShowAllIDsWindow, "Show ID's");

                //Weapon Spawner
                UIHelper.Begin("Weapon", 310, 10, 100, 152, 2, 20, 2);
                if (UIHelper.Button("Pistol"))
                    SpawnItem(355, 1);
                if (UIHelper.Button("Revolver"))
                    SpawnItem(386, 1);
                if (UIHelper.Button("Shotgun"))
                    SpawnItem(358, 1);
                if (UIHelper.Button("Rifle"))
                    SpawnItem(361, 1);
                if (UIHelper.Button("Machete"))
                    SpawnItem(359, 1);
                if (UIHelper.Button("Katana"))
                    SpawnItem(367, 1);

                //Weapon Upgrades Spawner
                UIHelper.Begin("Weapon Upgrades", 410, 10, 165, 152, 2, 20, 2);
                if (UIHelper.Button("Pistol Rail"))
                    SpawnItem(376, 1);
                if (UIHelper.Button("Pistol Suppressor"))
                    SpawnItem(374, 1);
                if (UIHelper.Button("Shotgun Rail"))
                    SpawnItem(346, 1);
                if (UIHelper.Button("Rifle Rail"))
                    SpawnItem(383, 1);
                if (UIHelper.Button("Scope Mod"))
                    SpawnItem(377, 1);
                if (UIHelper.Button("Laser Light Mod"))
                    SpawnItem(375, 1);
               
                //Ammo Spawner
                UIHelper.Begin("Ammo", 580, 10, 165, 86, 2, 20, 2);
                if (UIHelper.Button("9mm Ammo"))
                    SpawnItem(362, GetSpawnAmount());
                if (UIHelper.Button("Shotgun Slug Ammo"))
                    SpawnItem(372, GetSpawnAmount());
                if (UIHelper.Button("Rifle"))
                    SpawnItem(387, GetSpawnAmount());
                
                //Material Spawner
                UIHelper.Begin("Material", 750, 10, 95, 130, 2, 20, 2);
                if (UIHelper.Button("Log"))
                    SpawnItem(78, 1);
                if (UIHelper.Button("Half Log"))
                    SpawnItem(408, 1);
                if (UIHelper.Button("Plank"))
                    SpawnItem(395, 1);
                if (UIHelper.Button("Stone"))
                    SpawnItem(640, 1);
                if (UIHelper.Button("Turtle Shell"))
                    SpawnItem(506, 1);
                
                //Character Spawner
                UIHelper.Begin("Character", 850, 10, 95, 64, 2, 20, 2);
                if (UIHelper.Button("Kelvin"))
                    SpawnCharacter("Robby");
                if (UIHelper.Button("Virginia"))
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

            private int GetSpawnAmount()
            {
                int amount = 1;
                if (Settings.X100)
                    amount = 100;
                else if (Settings.X1000)
                    amount = 1000;

                return amount;
            }
        }
    }
}