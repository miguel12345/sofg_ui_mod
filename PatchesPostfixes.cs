using System;
using System.Collections.Generic;
using Assets.Code;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UIImprovements
{
    public class PatchesPostfixes
    {

        static void UIE_ActionPerception_setTo_addDurationInfo(UIE_ActionPerception __instance,World world, UIScroll_Locs.SortableAction srt) {
            Utils.PrintUITextProperties(__instance.value);

            //Clone the original value text field

            var durationGameObjectName = "Duration";
            GameObject durationGameObject = null;

            if (__instance.value.transform.parent.Find(durationGameObjectName) == null)
            {
                durationGameObject = GameObject.Instantiate(__instance.value.gameObject, __instance.value.transform.parent);
                durationGameObject.name = "Duration";
                // Get the sibling index of GameObject A
                int originalTextSiblingIndex = __instance.value.gameObject.transform.GetSiblingIndex();

                // Set GameObject B right below GameObject A
                durationGameObject.transform.SetSiblingIndex(originalTextSiblingIndex + 1);
            }

            __instance.value.alignment = TextAnchor.LowerLeft;
            var anchoredPos = __instance.value.GetComponent<RectTransform>().anchoredPosition;
            anchoredPos.x = 5;
            __instance.value.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
            

            durationGameObject.GetComponent<Text>().text = $"{srt.action.getTurnsRequired()} turns";
            Debug.LogWarning($"UIE_Action.setTo was called! {srt.action.getName()}");
        }

        public static void UIE_ActionPerception_setTo_postfix1(UIE_ActionPerception __instance,World world, UIScroll_Locs.SortableAction srt, SettlementHuman set)
        {
            UIE_ActionPerception_setTo_addDurationInfo(__instance,world,srt);
        }

        public static void UIE_ActionPerception_setTo_postfix2(UIE_ActionPerception __instance,World world, UIScroll_Locs.SortableAction srt)
        {
            UIE_ActionPerception_setTo_addDurationInfo(__instance,world,srt);
        }


        public static void UIE_ANPerception_setTo_addDurationInfo(UIE_ANPerception __instance,World world, UIScroll_Locs.SortableAN srt) {

            //Clone the original value text field

            var durationGameObjectName = "Duration";
            GameObject durationGameObject = null;

            if (__instance.value.transform.parent.Find(durationGameObjectName) == null)
            {
                durationGameObject = GameObject.Instantiate(__instance.value.gameObject, __instance.value.transform.parent);
                durationGameObject.name = "Duration";
                // Get the sibling index of GameObject A
                int originalTextSiblingIndex = __instance.value.gameObject.transform.GetSiblingIndex();

                // Set GameObject B right below GameObject A
                durationGameObject.transform.SetSiblingIndex(originalTextSiblingIndex + 1);
            }

            __instance.value.alignment = TextAnchor.LowerLeft;
            var anchoredPos = __instance.value.GetComponent<RectTransform>().anchoredPosition;
            anchoredPos.x = 5;
            __instance.value.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
            

            durationGameObject.GetComponent<Text>().text = $"{srt.action.getTurnsRequired()} turns";
        }

         public static void UITopLeft_checkData(UITopLeft __instance) {

            //TODO The number of turns until the next recruitment point should be displayed in the tooltip, not in the panel directly

                var background = __instance.enthrallments.transform.parent.GetChild(0);
                var borders = __instance.enthrallments.transform.parent.GetChild(1);

                //original height: 180.47
                var backgroundSize = background.GetComponent<RectTransform>().sizeDelta;
                if (backgroundSize.y < 200)
                {
                    backgroundSize.y = 230;
                    background.GetComponent<RectTransform>().sizeDelta = backgroundSize;

                    var bordersSize = borders.GetComponent<RectTransform>().sizeDelta;
                    bordersSize.y = 180;
                    borders.GetComponent<RectTransform>().sizeDelta = bordersSize;
                }

                var rpGo = Utils.CopyAsSiblingIfNotExists(__instance.enthrallments.gameObject,"Next Recruitment Point");

                var rpGoPos = rpGo.transform.position;
                rpGoPos.y = __instance.enthrallments.transform.position.y - (__instance.agents.transform.position.y - __instance.enthrallments.transform.position.y);
                rpGo.transform.position = rpGoPos;

                // Utils.PrintAllSiblingNames(__instance.enthrallments.gameObject);

                var map = __instance.master.world.map;
                var turnRemainder = (map.turn - map.param.mapGen_burnInSteps) % map.param.overmind_enthrallmentUseRegainPeriod;
                var turnsLeft = map.param.overmind_enthrallmentUseRegainPeriod - turnRemainder;

                rpGo.GetComponent<Text>().text = $"Next Recruitment point in {turnsLeft} turns";
         }


        static int cachedWorldPopulation = 0;
        static int cachedWorldPopulationTurn = 0;

         public static void UIE_WorldNation_setTo(UIE_WorldNation __instance,World world, SocialGroup sg, PopupWorldNations p) {
            
            var currentTurn = world.map.turn;
            var overmind = world.map.overmind;

            if(cachedWorldPopulationTurn != currentTurn) {
                cachedWorldPopulation = overmind.computeWorldPopulation();
                cachedWorldPopulationTurn = currentTurn;
            }

            var worldPopulation = cachedWorldPopulation;

		    var nationPopStr = __instance.pop.text;
            if (nationPopStr != "" && !nationPopStr.Contains(" "))
            {
                var nationPopInt = int.Parse(nationPopStr);
                var nationPopPercentage = nationPopInt / (float)worldPopulation;
                var nationPopPercentageStr = (nationPopPercentage * 100).ToString("0") + "%";
                __instance.pop.text = nationPopStr + " (" + nationPopPercentageStr + ")";
            }
            
         }

         public static void GraphicalUnit_checkData(GraphicalUnit __instance) {
                var unit = __instance.unit;
                
                var person = unit.person;

                var watchedGameObject = Utils.CopyAsSiblingIfNotExists(__instance.ringLayer.gameObject,"watched_status");

                // Debug.LogWarning($"GraphicalUnit_checkData: {unitName} {unit.personID}");

                if(person == null || !person.isWatched()) {
                    watchedGameObject.SetActive(false);
                }
                else {
                    watchedGameObject.SetActive(true);

                    var localPos = watchedGameObject.transform.localPosition;

                    localPos.y = 1.0f;
                    localPos.x = 0f;
                    watchedGameObject.transform.localPosition = localPos;

                    var eyeSprite = EventManager.getImg("uiimp.eye.png");

                    var scale = 0.08f;
                    watchedGameObject.transform.localScale = new Vector3(scale,scale,scale);

                    watchedGameObject.GetComponent<SpriteRenderer>().sprite = eyeSprite;
                    watchedGameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }

                
                var notesGameObject = Utils.CopyAsSiblingIfNotExists(__instance.ringLayer.gameObject,"has_notes");

                if (unit.notes == null || unit.notes == "") {
                    notesGameObject.SetActive(false);
                }
                else {
                    notesGameObject.SetActive(true);

                    var localPos = notesGameObject.transform.localPosition;

                    localPos.y = 1.0f;
                    localPos.x = 0f;
                    notesGameObject.transform.localPosition = localPos;

                    var notesSprite = EventManager.getImg("uiimp.notes.png");

                    notesGameObject.transform.localScale = new Vector3(0.2f,0.2f,0.2f);

                    notesGameObject.GetComponent<SpriteRenderer>().sprite = notesSprite;
                    notesGameObject.GetComponent<SpriteRenderer>().color = Color.grey;
                }

                if(watchedGameObject.activeSelf && notesGameObject.activeSelf) {
                    var localPos = watchedGameObject.transform.localPosition;
                    localPos.x = -0.4f;
                    watchedGameObject.transform.localPosition = localPos;

                    localPos = notesGameObject.transform.localPosition;
                    localPos.x = 0.4f;
                    notesGameObject.transform.localPosition = localPos;
                }
         }

         public static void UILeftUnit_setTo(UILeftUnit __instance,Unit unit) {

            if(unit is UA ua){

                var bFamilyView = Utils.FindChildStrict(__instance.textStatGold.transform.parent,"bFamilyView");
                
                //Move Home/Faith/House buttons a bit down

                //House original local position y = 17.97

                var bFamilyViewOriginalY = 17.97f;

                bFamilyView.transform.localPosition = new Vector3(bFamilyView.transform.localPosition.x,bFamilyViewOriginalY - 6f,bFamilyView.transform.localPosition.z);
                
                bFamilyView.GetComponentInChildren<Text>().gameObject.name = "tFamilyView";

                var bFaithView = Utils.CopyAsSiblingIfNotExists(bFamilyView.gameObject,"bFaithView");
                var bFaithViewLocalPos = bFaithView.transform.localPosition;
                bFaithViewLocalPos.y = bFamilyView.transform.localPosition.y + 32f;
                bFaithView.transform.localPosition = bFaithViewLocalPos;

                var bFaithViewButton = bFaithView.GetComponent<Button>();
                bFaithViewButton.targetGraphic = bFaithView.GetComponent<Image>();

                var bFaithViewText = bFaithView.GetComponentInChildren<Text>();

                var homeLocation = unit.map.locations[unit.homeLocation];
                bFaithViewButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
                bFaithViewButton.onClick.RemoveAllListeners();

                HolyOrder homeHolyOrder = null;

                if(homeLocation.settlement is SettlementHuman settlementHuman){
                    homeHolyOrder = settlementHuman.order;
                }

                if(homeHolyOrder != null){
                    bFaithViewButton.interactable = true;
                    bFaithViewText.text = homeHolyOrder.name;
                    bFaithViewButton.onClick.AddListener(() => {
                        Debug.LogWarning($"Clicked on {homeHolyOrder.name}");
                        __instance.world.prefabStore.popHolyOrder(homeHolyOrder);
                    });
                }
                else {
                    bFaithViewButton.interactable = false;
                    bFaithViewText.text = "(No Faith)";
                }

                //Change the family button text to the name of the house
                bFamilyView.GetComponentInChildren<Text>().text = "House "+ua.person.house.name;

                //Move the home location button to be above the faith button
                var bHomeLocation = __instance.textHomeLocation.transform.parent;
                var bHomeLocationLocalPos = bHomeLocation.transform.localPosition;
                bHomeLocationLocalPos.y = bFaithView.transform.localPosition.y + 32f;
                bHomeLocationLocalPos.x = bFaithView.transform.localPosition.x;
                bHomeLocation.transform.localPosition = bHomeLocationLocalPos;

                //Move the gold info to the left

                var goldRT = __instance.textStatGold.GetComponent<RectTransform>();
                var defenceRT = __instance.textStatDefence.GetComponent<RectTransform>();
                var attackRt = __instance.textStatAttack.GetComponent<RectTransform>();
                
                Utils.CopyRectTransform(defenceRT,goldRT);

                var goldText = __instance.textStatGold.GetComponent<Text>();
                goldText.alignment = TextAnchor.MiddleLeft;

                var yDeltaBetweenDefenceAndAttack = defenceRT.localPosition.y - attackRt.localPosition.y;
                goldRT.localPosition = new Vector3(goldRT.localPosition.x,goldRT.localPosition.y + yDeltaBetweenDefenceAndAttack,goldRT.localPosition.z);

                var tFixedGold = Utils.FindChildStrict(goldRT.transform.parent,"tFixedGold");
                var tFixedDefence = Utils.FindChildStrict(goldRT.transform.parent,"tFixedDef");
                var tFixedAttack = Utils.FindChildStrict(goldRT.transform.parent,"tFixedAttack");

                var tFixedGoldRT = tFixedGold.GetComponent<RectTransform>();
                var tFixedDefenceRT = tFixedDefence.GetComponent<RectTransform>();

                Utils.CopyRectTransform(tFixedDefenceRT,tFixedGoldRT);

                var yDeltaBetweenFixedDefenceAndAttack = tFixedDefenceRT.localPosition.y - tFixedAttack.localPosition.y;

                tFixedGoldRT.localPosition = new Vector3(tFixedGoldRT.localPosition.x,tFixedGoldRT.localPosition.y + yDeltaBetweenFixedDefenceAndAttack,tFixedGoldRT.localPosition.z);

                
            }
         }

        public static void UILeftUnit_Update(UILeftUnit __instance) {

            if (__instance.tick % 15 != 0 || __instance.personPopout == null)
            {
                return;
            }

            var considerRaycast = true;

            if (__instance.world.ui.blocker != null)
            {
                if (__instance.world.ui.blocker.GetComponent<PopupEvent>() != null)
                {
                    if (!__instance.world.ui.blocker.GetComponent<PopupEvent>().compressed)
                    {
                        considerRaycast = false;
                    }
                }
                else
                {
                    considerRaycast = false;
                }
            }

            if(!considerRaycast)
                return;

            
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1
            };


            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, list);



            foreach (RaycastResult item in list)
			{
				if (item.gameObject.name == "bFaithView")
				{
					__instance.smallDesc.gameObject.SetActive(value: true);
                    __instance.smallDescTitle.text = "Holy order";
                    __instance.smallDescText.text = "This represents the Holy Order followed by the unit's home location. Acolytes have access to holy actions and their motivation is determined by the Tenets of their faith.";
					break;
				}

                if (item.gameObject.name == "bFamilyView")
				{
					__instance.smallDesc.gameObject.SetActive(value: true);
                    __instance.smallDescTitle.text = "House";
                    __instance.smallDescText.text = "Every unit belongs to a house, comprised of their family members. Performing actions against someone might cause one of their family members to seek revenge.";
					break;
				}
            }
        }
        
        
        static List<int> fixedUIEAgentRosterIstanceIds = new List<int>();
        public static void UIE_AgentRoster_setTo(UIE_AgentRoster __instance,World world, UA agent)
        {
            var agentInstanceId = __instance.GetInstanceID();

            if( ! fixedUIEAgentRosterIstanceIds.Contains(agentInstanceId)) {

                Debug.LogWarning($"Fixing UIE_AgentRoster object");

                var challengeImageGO = Utils.CopyAsSiblingIfNotExists(__instance.bAgent.gameObject,"Challenge_sprite");
                Object.Destroy(challengeImageGO.GetComponent<Button>());

                var agentPortraitSize = __instance.bAgent.GetComponent<RectTransform>().sizeDelta;
                var challengeImageSize = challengeImageGO.GetComponent<RectTransform>().sizeDelta;

                challengeImageSize.x *= 0.5f;
                challengeImageSize.y *= 0.5f;

                challengeImageGO.GetComponent<RectTransform>().sizeDelta = challengeImageSize;

                //Move the object 10 pixels to the right
                challengeImageGO.transform.localPosition = new Vector3(challengeImageGO.transform.localPosition.x + agentPortraitSize.x + 20, challengeImageGO.transform.localPosition.y, challengeImageGO.transform.localPosition.z); 

                fixedUIEAgentRosterIstanceIds.Add(agentInstanceId);
            }

            if(agent.task is Task_PerformChallenge task_PerformChallenge) {
                var sprite = task_PerformChallenge.challenge.getSprite();
                var turnsLeft = ((!(task_PerformChallenge.challenge.getProgressPerTurn(agent, null) < 0.01)) ? ((int)Math.Ceiling((task_PerformChallenge.challenge.getComplexityAfterDifficulty() - task_PerformChallenge.progress) / task_PerformChallenge.challenge.getProgressPerTurn(agent, null))) : 100);
                
                var challengeSpriteGO = Utils.FindChildStrict(__instance.gameObject.transform,"Challenge_sprite");
                challengeSpriteGO.GetComponent<Image>().sprite = sprite;
            }
        }
    }

}
