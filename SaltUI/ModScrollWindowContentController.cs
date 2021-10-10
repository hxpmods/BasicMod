using HarmonyLib;
using ObjectBased.UIElements.Tooltip;
using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorialSystem;
using UnityEngine;

namespace BasicMod.SaltUI
{
    public class ModScrollWindowContentController : ScrollWindowContentController
    {

        public ModScrollWindowContentController(ModHintParameters parameters)
        : base(parameters)
        {
        }

        public List<ScrollWindowButton> currentButtons = new List<ScrollWindowButton>();

        public override bool CanBeClosedAutomatically()
        {
            return false;
        }

        public override bool CanButtonBePressed(ScrollWindowButton.ButtonType currentButtonType)
        {
            //Ignored
            return true;
        }

        public override TooltipContent GetButtonTooltipContent(ScrollWindowButton.ButtonType currentButtonType)
        {
            //Ignored
            return new TooltipContent();
        }

        public override SpriteSortingLayers GetSortingLayer()
        {
            return SpriteSortingLayers.TutorialHint;
        }

        public override void OnButtonPress(ScrollWindowButton.ButtonType currentButtonType)
        { 
            //Ignored
            return;
        }

        public void OnModButtonPress(int buttonId)
        {
            Debug.Log(buttonId);
        }

        public override void OnHintPreparingEnd()
        {
            //Ignored
            return;
        }

        public override void OnNewRoomTarget()
        {
            //Ignored
            return;
        }
    }
    [HarmonyPatch(typeof(ScrollWindowButton), "Press")]
    class ScrollWindowButtonPatch
    {
        static bool Prefix(ScrollWindowButton __instance)
        {
          ModScrollWindowContentController CC = Managers.Game.scrollWindow.currentContentController as ModScrollWindowContentController;
          if(CC != null)
            {
                var list = CC.currentButtons;
                if (list.Contains(__instance)){

                    int id = list.IndexOf(__instance);
                    CC.OnModButtonPress(id);
                }

                return false;
            }
            return true;
        }
    }
   



}
