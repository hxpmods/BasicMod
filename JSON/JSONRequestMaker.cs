using BasicMod.Factories;
using BasicMod.ModObjects;
using Npc.Parts;
using Npc.Parts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/*
CitizenQuests
FemaleQuests
NecromancyQuests
RogueQuests
WarriorQuests
*/

namespace BasicMod
{
    public class JSONRequestMaker
    {
        public string faction { get; set; }
        public string name { get; set; }
        public string text { get; set; }

        public string[] desiredeffects { get; set; }

        public int karma { get; set; }

        public string special { get; set; }

        public NonAppearancePart quest;

        public NonAppearancePart GenerateQuest()
        {

            QuestSystem.Quest new_quest = ScriptableObject.CreateInstance<QuestSystem.Quest>();
            new_quest.name = name;
            new_quest.karmaReward = karma;
            LocalDict.AddKeyToDictionary("quest_text_" + name, text);

            quest = new_quest;

            Debug.Log(special + " " + name);

            if (special == "tinyHead")
            {
                NpcTemplate quest_container = RequestFactory.CreateEmptyNpcTemplate(name+ "Container");

                var modifier = ScriptableObject.CreateInstance<ModNonAppearancePart>();
                modifier.name = "Modifier";
                Debug.Log("We are here");

                quest_container.baseParts = new NonAppearancePart[] { new_quest, modifier};
                quest = quest_container;
                return quest_container;
            }

            return new_quest;


        }

        public void AddEffectsToQuest(QuestSystem.Quest quest)
        {
            if (quest != null)
            {

                List<PotionEffect> effectslist = new List<PotionEffect>();
                //if (desiredeffects == null) desiredeffects = new string[] { "Fire"};
                foreach (string effect in desiredeffects)
                {
                    //Debug.Log(PotionEffect.GetByName(effect));
                    effectslist.Add(PotionEffect.GetByName(effect));
                }

                if (effectslist != null) quest.desiredEffects = effectslist.ToArray();
            }
        }

        public void ConfigureQuest()
        {
            if (quest != null)
            {
                QuestSystem.Quest _quest = quest as QuestSystem.Quest;

                if (_quest != null)
                {
                    AddEffectsToQuest((QuestSystem.Quest)quest);
                    return;
                }

                
                NpcTemplate template = quest as NpcTemplate;
                if(template != null)
                {
                    AddEffectsToQuest((QuestSystem.Quest)template.baseParts[0]);
                }

                

            }
        }

    }

}
