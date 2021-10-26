using Npc.MonoBehaviourScripts;
using Npc.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.ModObjects
{
    public class ModNonAppearancePart : NonAppearancePart
    {
        public override void ApplyPartTo(NpcMonoBehaviour npcMonoBehaviour)
        {
            var face = npcMonoBehaviour.transform.Find("Anchor/Head").gameObject;
            var faceBack = npcMonoBehaviour.transform.Find("Anchor/HeadBack").gameObject;
            face.transform.localScale = new Vector3(0.725f, 0.725f, 1f);
            faceBack.transform.localScale = new Vector3(0.725f, 0.725f, 1f);
            //face.transform.localPosition = new Vector3(0.18f, 2.5f, 0f);

        }
    }
}
