using System.Reflection;
using Assets.Code;
using UnityEngine;

namespace UIImprovements
{
    public class Patches
    {
        public static void PatchUIAction_duration() {
            
            Debug.LogWarning("Performing patching for UIE_ActionPerception_setTo");

            //World world, UIScroll_Locs.SortableAction srt, SettlementHuman
            MethodInfo original = typeof(UIE_ActionPerception).GetMethod("setTo", new[] { typeof(World), typeof(UIScroll_Locs.SortableAction), typeof(SettlementHuman) });
            MethodInfo postfix = typeof(PatchesPostfixes).GetMethod("UIE_ActionPerception_setTo_postfix1");

            PatchRegistry.PatchMethod(original,null, postfix);

            //World world, UIScroll_Locs.SortableAction srt
            MethodInfo original2 = typeof(UIE_ActionPerception).GetMethod("setTo", new[] { typeof(World), typeof(UIScroll_Locs.SortableAction) });
            MethodInfo postfix2 = typeof(PatchesPostfixes).GetMethod("UIE_ActionPerception_setTo_postfix2");

            PatchRegistry.PatchMethod(original2,null, postfix2);
        }

        
        public static void PatchUIE_ANPerception_duration() {
            
            Debug.LogWarning("Performing patching for UIE_ANPerception_setTo");

            //World world, UIScroll_Locs.SortableAction srt, SettlementHuman
            MethodInfo original = typeof(UIE_ANPerception).GetMethod("setTo", new[] { typeof(World), typeof(UIScroll_Locs.SortableAN) });
            MethodInfo postfix = typeof(PatchesPostfixes).GetMethod("UIE_ANPerception_setTo_addDurationInfo");

            PatchRegistry.PatchMethod(original,null, postfix);
        }

        public static void PatchUITopLeft_show_turns_for_next_recruitment_point() {
            
            Debug.LogWarning("Performing patching for PatchUITopLeft_show_turns_for_next_recruitment_point");

            //World world, UIScroll_Locs.SortableAction srt, SettlementHuman
            MethodInfo original = typeof(UITopLeft).GetMethod("checkData");
            MethodInfo postfix = typeof(PatchesPostfixes).GetMethod("UITopLeft_checkData");

            PatchRegistry.PatchMethod(original,null, postfix);
        }

        public static void PatchUIE_WorldNation_show_world_pop_percentage() {
            
            Debug.LogWarning("Performing patching for PatchUIE_WorldNation_show_world_pop_percentage");

            //World world, UIScroll_Locs.SortableAction srt, SettlementHuman
            MethodInfo original = typeof(UIE_WorldNation).GetMethod("setTo", new[] { typeof(World), typeof(SocialGroup), typeof(PopupWorldNations) });
            MethodInfo postfix = typeof(PatchesPostfixes).GetMethod("UIE_WorldNation_setTo");

            PatchRegistry.PatchMethod(original,null, postfix);
        }

        public static void PatchGraphicalUnit_show_watched_status() {
            
            Debug.LogWarning("Performing patching for PatchGraphicalUnit_show_watched_status");

            //World world, UIScroll_Locs.SortableAction srt, SettlementHuman
            MethodInfo original = typeof(GraphicalUnit).GetMethod("checkData");
            MethodInfo postfix = typeof(PatchesPostfixes).GetMethod("GraphicalUnit_checkData");

            PatchRegistry.PatchMethod(original,null, postfix);
        }

        public static void PatchUILeftUnit() {
            
            Debug.LogWarning("Performing patching for PatchUILeftUnit");

            //Modify UILeftUnit.setTo to to define and update the contents of the faith button
            MethodInfo original = typeof(UILeftUnit).GetMethod("setTo", new[] {typeof(Unit)});
            MethodInfo postfix = typeof(PatchesPostfixes).GetMethod("UILeftUnit_setTo");
            PatchRegistry.PatchMethod(original,null, postfix);

            
            //Modify UILeftUnit.Update to handle input interaction to show tooltips for faith and home location
            MethodInfo original2 = typeof(UILeftUnit).GetMethod("Update");
            MethodInfo postfix2 = typeof(PatchesPostfixes).GetMethod("UILeftUnit_Update");
            PatchRegistry.PatchMethod(original2,null, postfix2);
        }

        public static void PatchAgentRoster() {
            
            Debug.LogWarning("Performing patching for PatchAgentRoster");

            //Modify UIE_AgentRoster.setTo to set the contents of thew new challenge section added above
            MethodInfo original2 = typeof(UIE_AgentRoster).GetMethod("setTo", new[] {typeof(World), typeof(UA)});
            MethodInfo postfix2 = typeof(PatchesPostfixes).GetMethod("UIE_AgentRoster_setTo");
            PatchRegistry.PatchMethod(original2,null, postfix2);
        }
    }
}
