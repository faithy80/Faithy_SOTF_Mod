﻿using Sons.Input;
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
            private static List<ItemData>? itemList;
            private static bool isInitialized = false;
            private static Scene _sonsMainScene;
            private Vector2 scrollPosition = Vector2.zero;

            private void OnGUI()
            {
                // Check GUI visibility
                if (!Settings.isGUIVisible) return;

                // Set GUI colors
                GUI.color = Color.cyan;
                GUI.backgroundColor = Color.grey;

                // Show control buttons
                UIHelper.Begin("Control buttons", 10, 10, 175, 152, 2, 20, 2);

                GUI.color = Color.white;

                if (UIHelper.Button("Show Quick Spawn List"))
                    UpdateListSettings("ShowQuickSpawnList");

                if (UIHelper.Button("Show All Item Spawn List"))
                    UpdateListSettings("ShowSpawnAllItemList");

                if (UIHelper.Button("Show Character Spawn List"))
                    UpdateListSettings("ShowSpawnCharacterList");

                if (Settings.X10 = UIHelper.Button("x10: ", Settings.X10))
                {
                    Settings.X100 = false;
                    Settings.X1000 = false;
                }

                if (Settings.X100 = UIHelper.Button("x100: ", Settings.X100))
                {
                    Settings.X10 = false;
                    Settings.X1000 = false;
                }

                if (Settings.X1000 = UIHelper.Button("x1000: ", Settings.X1000))
                {
                    Settings.X10 = false;
                    Settings.X100 = false;
                }

                // Show corresponding list
                if (Settings.ShowQuickSpawnList)
                    ShowQuickSpawnList();

                if (Settings.ShowSpawnAllItemList)
                    ShowAllItemSpawnList();

                if (Settings.ShowSpawnCharacterList)
                    ShowSpawnCharacterList();
            }

            private void Update()
            {
                RegisterHandlers();

                // Cache SonsMainScene
                if (!_sonsMainScene.isLoaded) _sonsMainScene = SceneManager.GetSceneByName("SonsMain");
            }

            private void ShowMenu()
            {
                // Check if GUI hotkey pressed
                if (Input.GetKeyDown(Plugin.ModMenuKeybind.Value))
                {
                    // Prepare GUI settings
                    Settings.isGUIVisible = !Settings.isGUIVisible;
                    if (Settings.isGUIVisible)
                    {
                        InputSystem.SetState(0, true);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        return;
                    }

                    // Restore previous game control settings
                    if (LocalPlayer.IsInWorld || LocalPlayer.IsInInventory || LocalPlayer.IsConstructing || LocalPlayer.IsInMidAction || LocalPlayer.CurrentView == PlayerInventory.PlayerViews.Hidden)
                    {
                        InputSystem.SetState(0, false);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                }
            }

            public void ShowQuickSpawnList()
            {
                GUI.color = Color.cyan;

                // Weapon Spawner
                UIHelper.Begin("Weapon", 200, 10, 95, 218, 2, 20, 2);

                GUI.color = Color.yellow;

                if (UIHelper.Button("Pistol"))
                    SpawnItem(355, 1);
                if (UIHelper.Button("Revolver"))
                    SpawnItem(386, 1);
                if (UIHelper.Button("Shotgun"))
                    SpawnItem(358, 1);
                if (UIHelper.Button("Rifle"))
                    SpawnItem(361, 1);
                if (UIHelper.Button("AirGun"))
                    SpawnItem(357, 1);
                if (UIHelper.Button("Crossbow"))
                    SpawnItem(365, 1);
                if (UIHelper.Button("Tactical Bow"))
                    SpawnItem(360, 1);
                if (UIHelper.Button("Machete"))
                    SpawnItem(359, 1);
                if (UIHelper.Button("Katana"))
                    SpawnItem(367, 1);

                GUI.color = Color.cyan;

                // Weapon Upgrades Spawner
                UIHelper.Begin("Weapon Upgrades", 300, 10, 165, 174, 2, 20, 2);

                GUI.color = Color.yellow;

                if (UIHelper.Button("AirGun Scope"))
                    SpawnItem(470, 1);
                if (UIHelper.Button("Pistol Rail"))
                    SpawnItem(376, 1);
                if (UIHelper.Button("Pistol Suppressor"))
                    SpawnItem(374, 1);
                if (UIHelper.Button("Shotgun Rail"))
                    SpawnItem(346, 1);
                if (UIHelper.Button("Rifle Rail"))
                    SpawnItem(383, 1);
                if (UIHelper.Button("Crossbow Mod"))
                    SpawnItem(384, 1);
                if (UIHelper.Button("Flashlight Mod"))
                    SpawnItem(378, 1);

                GUI.color = Color.cyan;

                // Ammo Spawner
                UIHelper.Begin("Ammo", 470, 10, 165, 218, 2, 20, 2);

                GUI.color = Color.yellow;

                if (UIHelper.Button("9mm Ammo"))
                    SpawnItem(362, GetSpawnAmount());
                if (UIHelper.Button("Shotgun Buck Ammo"))
                    SpawnItem(364, GetSpawnAmount());
                if (UIHelper.Button("Shotgun Slug Ammo"))
                    SpawnItem(363, GetSpawnAmount());
                if (UIHelper.Button("Rifle Ammo"))
                    SpawnItem(387, GetSpawnAmount());
                if (UIHelper.Button("3D Printed Arrow"))
                    SpawnItem(618, GetSpawnAmount());
                if (UIHelper.Button("Crossbow Ammo"))
                    SpawnItem(368, GetSpawnAmount());
                if (UIHelper.Button("Tactical Bow Ammo"))
                    SpawnItem(373, GetSpawnAmount());
                if (UIHelper.Button("Tracker Dart"))
                    SpawnItem(366, GetSpawnAmount());
                if (UIHelper.Button("Zipline Rope"))
                    SpawnItem(523, GetSpawnAmount());

                GUI.color = Color.cyan;

                // Material Spawner
                UIHelper.Begin("Material", 640, 10, 95, 152, 2, 20, 2);

                GUI.color = Color.yellow;

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
                if (UIHelper.Button("Spring"))
                    SpawnItem(513, 1);
            }

            public void ShowAllItemSpawnList()
            {
                // Create window and call interaction method
                GUI.color = Color.cyan;
                GUI.Window(0, new Rect(200, 10, 300, 1000), (GUI.WindowFunction)ShowAllIDsWindow, "Show All Items");
            }

            public void ShowAllIDsWindow(int windowID)
            {
                // Initialise all item list if the game is loaded
                if (_sonsMainScene.isLoaded)
                {
                    if (!isInitialized)
                    {
                        itemList ??= ItemDatabaseManager.Items;
                        isInitialized = true;
                    }
                }

                // Show all item list is empty
                if (itemList == null || itemList.Count == 0)
                {
                    GUI.color = Color.yellow;
                    GUI.Label(new Rect(5, 15, 300, 1000), "Item list is empty.");
                }
                // Show all item list and set listeners 
                else
                {
                    string buttonLabel;

                    GUI.color = Color.yellow;

                    GUILayout.BeginArea(new Rect(5, 15, 295, 980));
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    GUILayout.BeginVertical();

                    foreach (ItemData item in itemList)
                    {
                        bool isNecessary = true;

                        buttonLabel = $"{item._name} : {item._id}";

                        foreach (string searchItem in Constants.unnecessaryList)
                        {
                            if (buttonLabel.Contains(searchItem))
                            {
                                isNecessary = false;
                                break;
                            }
       
                        }

                        if (isNecessary)
                        {
                            if (GUILayout.Button(buttonLabel))
                                SpawnItem(item._id, GetSpawnAmount());
                        }
                    }

                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();
                }
            }

            private void ShowSpawnCharacterList()
            {
                // Character Spawner
                GUI.color = Color.cyan;
                
                UIHelper.Begin("Character", 200, 10, 95, 64, 2, 20, 2);
                
                GUI.color = Color.yellow;

                if (UIHelper.Button("Kelvin"))
                    SpawnCharacter("Robby");
                if (UIHelper.Button("Virginia"))
                    SpawnCharacter("Virginia");
            }

            private void RegisterHandlers()
            {
                ShowMenu();
            }

            private void SpawnItem(int itemID, int amount)
            {
                // Try to spawn the given item with the given amount. Log error on fail.
                try
                {
                    LocalPlayer.Inventory.AddItem(itemID, amount);
                }
                catch
                {
                    MyLogger.Error("Failed to add item!");
                }
            }

            private void SpawnCharacter(string character)
            {
                string result = character + " 1";

                // Try to spawn character. Log error on fail.
                try
                {
                    Sons.Characters.CharacterManager.Instance.DebugAddCharacter(result);
                }
                catch
                {
                    MyLogger.Error("Failed to spawn character!");
                }
            }

            private int GetSpawnAmount()
            {
                int amount = 1;

                // Adjust amount using the corresponding multiplier
                if (Settings.X10)
                    amount = 10;
                else if (Settings.X100)
                    amount = 100;
                else if (Settings.X1000)
                    amount = 1000;

                return amount;
            }

            private void UpdateListSettings(string settingName)
            {
                Settings.ShowQuickSpawnList = false;
                Settings.ShowSpawnAllItemList = false;
                Settings.ShowSpawnCharacterList = false;

                // Set list visibility by the given list name
                switch (settingName)
                {
                    case "ShowQuickSpawnList":
                    default:
                        Settings.ShowQuickSpawnList = true;
                        break;
                    case "ShowSpawnAllItemList":
                        Settings.ShowSpawnAllItemList = true;
                        break;
                    case "ShowSpawnCharacterList":
                        Settings.ShowSpawnCharacterList = true;
                        break;
                }
            }
        }
    }
}