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
    public class ModQuestMaker
    {
        public string faction { get; set; }
        public string name { get; set; }
        public string text { get; set; }

        public string[] desiredeffects { get; set; }

        public QuestSystem.Quest quest;

        public QuestSystem.Quest GenerateQuest()
        {
            QuestSystem.Quest new_quest = ScriptableObject.CreateInstance<QuestSystem.Quest>();
            new_quest.name = name;
            LocalDict.AddKeyToDictionary("quest_text_" + name, text);
            quest = new_quest;
            return new_quest;
        }

        public void ConfigureQuest()
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

                if (effectslist!= null) quest.desiredEffects = effectslist.ToArray();
                foreach (PotionEffect potionEffect in quest.desiredEffects)
                {
                    Debug.Log(potionEffect);
                }

            }
        }

    }

}
