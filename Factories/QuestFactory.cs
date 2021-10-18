using BasicMod.GameHooks;
using Npc.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.Factories
{
    class QuestFactory
    {
        public static List<ModQuestMaker> allQuests = new List<ModQuestMaker>();
        public static EventHandler<EventArgs> onQuestsPreGenerate;
        public static bool doClearQuests = false;

        public static void Awake()
        {
            NpcManagerEvents.onInitiateScriptableObjects += (_, e) =>
            {

                if (doClearQuests) ClearQuestTemplates();

               

                EventArgs a = new EventArgs();
                onQuestsPreGenerate?.Invoke(null, a);
                GenerateQuests();
                AddQuests();

                foreach (NpcTemplate template in NpcTemplate.allNpcTemplates)
                {
                    /*
                    if (template.name == "Demo2GroundhogRandom_Npc")
                    {
                        Debug.Log("huh");
                        LogTemplate(template);
                    }
                    */
                    if (template.name == "FemaleQuests")
                    {
                        BuffChanceForNecromancer(template);
                        //LogTemplate(template);
                    }


                }

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

        public static void BuffChanceForNecromancer(NpcTemplate template)
        {

                foreach (PartContainerGroup<NonAppearancePart> partGroup in template.groupsOfContainers)
                {

                    foreach (PartContainer<NonAppearancePart> partContainer in partGroup.partsInGroup)
                    {
                    
                        NpcTemplate container = partContainer.part as NpcTemplate;
                        if (container != null)
                        {
                            if (container.name.StartsWith("Necro")){
                            partContainer.chanceBtwParts = 100f;
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

        public static bool TemplateUsesNecromancyQuests(NpcTemplate template)
        {
            bool flag = false;
            
            if (template.baseParts != null)
            {
                {
                    foreach(NonAppearancePart part in template.baseParts)
                    {
                        if (part != null)
                        {
                            if (part.name.Contains("Quests"))
                            {
                                Debug.Log(part.name);
                            }
                        }
                    }

                }
                
            }

           
            return flag;
        }

        public static void LogTemplate(NpcTemplate template)
        {


                Debug.Log("Begin logging NpcTemplate " + template);


                Debug.Log("Logging non appearance parts for " + template.name);
                bool flag = false;
                foreach (NonAppearancePart part in template.baseParts)
                {
                    NpcTemplate container = part as NpcTemplate;
                    if (container != null)
                    {
                        Debug.Log("Found non random container " + container.name + " in " + template.name);
                    LogTemplate(container);
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
                            LogTemplate(container);
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

    }
}
