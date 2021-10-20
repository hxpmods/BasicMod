using BasicMod.GameHooks;
using Npc.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.Factories
{
    public class QuestFactory
    {
        public static List<ModQuestMaker> allQuests = new List<ModQuestMaker>();
        public static EventHandler<EventArgs> onQuestsPreGenerate;
        public static EventHandler<EventArgs> onQuestsPostGenerate;
        public static EventHandler<EventArgs> onFactionsPreGenerate;
        public static bool doClearQuests = false;

        public static void Awake()
        {
            NpcManagerEvents.onInitiateScriptableObjects += (_, e) =>
            {

                if (doClearQuests) ClearQuestTemplates();


                EventArgs a = new EventArgs();
                onFactionsPreGenerate?.Invoke(null, a);

                EventArgs b = new EventArgs();
                onQuestsPreGenerate?.Invoke(null, b);
                GenerateQuests();
                AddQuests();

                EventArgs c = new EventArgs();
                onQuestsPostGenerate?.Invoke(null, c);

            };

            GameHooks.InitiatePotionEffectsEvent.OnInitiate += (_, e) =>
            {
                ConfigureQuests();
            };
        }

        public static void GenerateQuests()
        {
            foreach (ModQuestMaker quest in allQuests)
            {
                quest.GenerateQuest();
            }
        }

        public static void BuffChanceForNecromancer()
        {

            foreach (NpcTemplate template in NpcTemplate.allNpcTemplates) {

                if(template.name.EndsWith("Quests")){

                    foreach (PartContainerGroup<NonAppearancePart> partGroup in template.groupsOfContainers)
                    {

                        foreach (PartContainer<NonAppearancePart> partContainer in partGroup.partsInGroup)
                        {

                            NpcTemplate container = partContainer.part as NpcTemplate;
                            if (container != null)
                            {
                                Debug.Log($"Buffing chance for necromancer in {template.name} template.");
                                if (container.name.StartsWith("Necro")) {
                                    partContainer.chanceBtwParts *= 4f;
                                }

                            }
                        }

                    }

                }
            }
      
        }

        public static void AddQuests()
        {
            foreach (NpcTemplate template in NpcTemplate.allNpcTemplates)
            {
                if (template.name.EndsWith("Quests"))
                {


                   //Debug.Log(template.name);
                    PartContainerGroup<NonAppearancePart> potion_requests = template.groupsOfContainers[0];
                    
                    var factionQuest = from quest in allQuests
                                       where quest.faction + "Quests" == template.name
                                       select quest;

                    var modPartContainer = new List<PartContainer<NonAppearancePart>>();
                    foreach (ModQuestMaker quest in factionQuest)
                    {

                        var container = new PartContainer<NonAppearancePart>();
                        container.part = quest.quest;
                        
                        modPartContainer.Add(container);
                    }

                    //potion_requests.partsInGroup = (PartContainer<NonAppearancePart>[])potion_requests.partsInGroup.Concat(modPartContainer.ToArray());

                    modPartContainer.AddRange(potion_requests.partsInGroup);
                    potion_requests.partsInGroup = modPartContainer.ToArray();

                    foreach (PartContainer<NonAppearancePart> container in potion_requests.partsInGroup)
                    {
                        //For now all base game
                        container.chanceBtwParts = 100.0f / potion_requests.partsInGroup.Count();
                    }

                    //LogQuests(potion_requests);

                }
            }
        }

        public static void LogQuests(PartContainerGroup<NonAppearancePart> partContainerGroup)
        {
            Debug.Log(partContainerGroup.groupName);
            Debug.Log(partContainerGroup.groupChance);
            foreach (PartContainer<NonAppearancePart> part in partContainerGroup.partsInGroup)
            {
                Debug.Log(part.part.name);
                Debug.Log(part.chanceBtwParts);
            }


        }
        public static void ConfigureQuests()
        {
            foreach (ModQuestMaker quest in allQuests)
            {
                quest.ConfigureQuest();
            }
        }

        public static void ClearQuestTemplates()
        {
            foreach (NpcTemplate template in NpcTemplate.allNpcTemplates)
            {
                if (template.name.EndsWith("Quests"))
                {
                    //LogTemplate(template);
                    PartContainerGroup<NonAppearancePart> potion_requests = template.groupsOfContainers[0];

                    List<PartContainer<NonAppearancePart>> templatesToReAdd = new List<PartContainer<NonAppearancePart>>();

                    foreach (PartContainer<NonAppearancePart> partContainer in potion_requests.partsInGroup)
                    {
                        NpcTemplate container = partContainer.part as NpcTemplate;
                        if (container != null)
                        {
                            templatesToReAdd.Add(partContainer);
                        }
                    }

                    if (templatesToReAdd.Count() > 0 ){
                        potion_requests.partsInGroup = templatesToReAdd.ToArray();
                    }
                    else {
                        potion_requests.partsInGroup = new PartContainer<NonAppearancePart>[] { };
                    }
                    
                    
                    /*
                    foreach (PartContainerGroup<NonAppearancePart> partGroup in template.groupsOfContainers)
                    {
                        foreach (PartContainer<NonAppearancePart> partContainer in partGroup.partsInGroup)
                        {

                            if (!partContainer.part.name.EndsWith("Quests"))
                            {
                                partContainer.part =  null;
                            }
                        }
                    }*/
                    //Debug.Log(template.groupsOfContainers[0].groupName);
                }
            }
        }

        public static void LogTemplate(NpcTemplate template, bool iterate = true)
        {


                Debug.Log("Begin logging NpcTemplate " + template);
                
                /*
                if (template.appearance != null)
                {
                Debug.Log("Logging appearance for: " + template);
                LogAppearanceContainer(template.appearance);
                }
                */

                
                Debug.Log("Logging non appearance parts for " + template.name);
                bool flag = false;
                foreach (NonAppearancePart part in template.baseParts)
                {
                    NpcTemplate container = part as NpcTemplate;
                    if (container != null)
                    {
                        Debug.Log("Found non random container " + container.name + " in " + template.name);
                        if (iterate) LogTemplate(container);
                    }
                    else
                    {
                        Debug.Log(part);
                        flag = true;
                    }
                }
                if (!flag) Debug.Log("No non appearance parts found for " + template.name);

                Debug.Log("Logging random non appearance parts for " + template.name);
                foreach (PartContainerGroup<NonAppearancePart> partGroup in template.groupsOfContainers)
                {
                    Debug.Log(partGroup.groupName);
                    Debug.Log("Chance: " + partGroup.groupChance);
                    foreach (PartContainer<NonAppearancePart> partContainer in partGroup.partsInGroup)
                    {   
                        NpcTemplate container = partContainer.part as NpcTemplate;
                        if (container != null)
                        {
                            Debug.Log(container);
                            Debug.Log("Found random container " + container.name+  " in " + template.name);
                            Debug.Log(container.spawnChance);
                            if (iterate) LogTemplate(container);


                           if (container.name.Contains("Citizen")) LogTemplate(container);

                        }
                        else
                        {
                            Debug.Log(partContainer.part);
                        }

                    }

                }
                Debug.Log("Finished logging random non appearance parts for " + template.name);
                Debug.Log("\n");
        }

        public static void LogAppearanceContainer(AppearanceContainer appearanceContainer)
        {

            Debug.Log("Logging aboveHairFeature1 parts");
            foreach (PartContainer<Npc.Parts.Appearance.Accessories.AccessoryAboveHair> part in appearanceContainer.aboveHairFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging aboveHairFeature2 parts");
            foreach (PartContainer<Npc.Parts.Appearance.Accessories.AccessoryAboveHair> part in appearanceContainer.aboveHairFeature2.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging behindBodyFeature1 parts");
            foreach (var part in appearanceContainer.behindBodyFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging behindBodyFeature2 parts");
            foreach (var part in appearanceContainer.behindBodyFeature2.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging body parts");
            foreach (var part in appearanceContainer.body.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging bodyFeature1 parts");
            foreach (var part in appearanceContainer.bodyFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging bodyFeature2 parts");
            foreach (var part in appearanceContainer.bodyFeature2.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging breastSize parts ");
            foreach (var part in appearanceContainer.breastSize.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging eyes parts ");
            foreach (var part in appearanceContainer.eyes.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging face parts ");
            foreach (var part in appearanceContainer.face.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging faceFeature1");
            foreach (var part in appearanceContainer.faceFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging faceFeature2");
            foreach (var part in appearanceContainer.faceFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging hairstyle");
            foreach (var part in appearanceContainer.hairstyle.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging handBackFeature1");
            foreach (var part in appearanceContainer.handBackFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging handBackFeature2");
            foreach (var part in appearanceContainer.handBackFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging handFrontFeature1");
            foreach (var part in appearanceContainer.handFrontFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging handFrontFeature2");
            foreach (var part in appearanceContainer.handFrontFeature2.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging hat");
            Debug.Log(appearanceContainer.hat.groupChance);
            foreach (var part in appearanceContainer.hat.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging shortHairFeature1");
            foreach (var part in appearanceContainer.shortHairFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging shortHairFeature2");
            foreach (var part in appearanceContainer.shortHairFeature2.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging skullShape");
            foreach (var part in appearanceContainer.skullShape.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging skullShapeFeature1");
            foreach (var part in appearanceContainer.skullShapeFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging skullShapeFeature2");
            foreach (var part in appearanceContainer.skullShapeFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }

            Debug.Log("Logging skullShapeFeature3");
            foreach (var part in appearanceContainer.skullShapeFeature1.partsInGroup)
            {
                Debug.Log(part.part.name);
            }
        }

        public static NpcTemplate CreateEmptyNpcTemplate(string name)
        {
            var template = ScriptableObject.CreateInstance<NpcTemplate>();
            template.name = name;
            return template;
        }

        public static void AddTemplateToOtherTemplateContainerGroup(NpcTemplate toAdd, NpcTemplate addTo)
        {
            var partContainer = new PartContainer<NonAppearancePart>();
            partContainer.part = toAdd;

            var newPartsInGroup = new List<PartContainer<NonAppearancePart>>();
            newPartsInGroup.Add(partContainer);
            newPartsInGroup.AddRange(addTo.groupsOfContainers[0].partsInGroup);

            addTo.groupsOfContainers[0].partsInGroup = newPartsInGroup.ToArray();

            //Rebalance parts
            foreach(var part in addTo.groupsOfContainers[0].partsInGroup)
            {
                part.chanceBtwParts = 100.0f / addTo.groupsOfContainers[0].partsInGroup.Count();
            }


        }

    }
}
