using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorialSystem;

namespace BasicMod.SaltUI
{
    public class ModHintParameters : HintParameters
    {
        public List<AbstractWindowElement> scrollWindowElements = new List<AbstractWindowElement>();
        public ButtonPressedAction buttonPressedAction;

    }
}
