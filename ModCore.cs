using System;
using System.Collections.Generic;
using Assets.Code;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace UIImprovements
{
    public class ModCore : Assets.Code.Modding.ModKernel
    {
        bool loaded = false;

        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            Console.WriteLine("Game started");
            Debug.LogWarning("Game Started");
        }

        public override void onModsInitiallyLoaded()
        {
            //TODO Check how to handle mod assembly through the mod configuration menu

            if (loaded) return;
            loaded = true;
            
            Debug.LogWarning("onModsInitiallyLoaded");
            
            Debug.LogWarning("UIE_Action.setTo patched successfully");

            Debug.LogWarning("Starting TCP server");
            TCPCommandListener tcpCommandListener = new TCPCommandListener(1331);
            tcpCommandListener.StartAsync();
            Debug.LogWarning("TCP server is now running!");

            Load();
        }

        public override void beforeMapGen(Map map)
        {
            
            Debug.LogWarning("beforeMap Gen");
        }

        public static void Unload() {
            Console.WriteLine("[UIE] Unloading...");
            PatchRegistry.RevertAllPatches();
            Console.WriteLine("[UIE] Unloaded!");
        }

        public static void Load() {
            Console.WriteLine("[UIE] Loading...");

            Patches.PatchUIAction_duration();
            Patches.PatchUIE_ANPerception_duration();
            Patches.PatchUITopLeft_show_turns_for_next_recruitment_point();
            Patches.PatchUIE_WorldNation_show_world_pop_percentage();
            Patches.PatchGraphicalUnit_show_watched_status();
            Patches.PatchUILeftUnit();
            Patches.PatchAgentRoster();

            //TODO - Dont forget: Log path C:\Users\Utilizador\AppData\LocalLow\FallenOakGames\ShadowsOfForbiddenGods\Player.log
            //TODO - Patch agent info to show the agent/hero/acolyte faith
            //TODO - Show a small overlay on top of the agent's portrait showing the icon and time left for the current action
            //TODO - Create an "army overview" tab showing all army activity (battles and conquests)

            Console.WriteLine("[UIE] Loaded!");
        }
    }
}
