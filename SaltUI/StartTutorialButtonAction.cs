using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI
{
    class StartTutorialButtonAction : ButtonPressedAction
    {
        public StartTutorialButtonAction(ScrollWindow window) : base(window)
        {
        }

        public override void Press(int buttonId)
        {
            base.Press(buttonId);
            if (buttonId == 0)
            {
                Managers.Tutorial.StartTutorialSet("Tutorial Set Main");
            }
        }
    }
}
