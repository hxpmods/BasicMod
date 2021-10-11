using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI.WindowElements
{
    public class DividerWindowElement : AbstractWindowElement
    {
        public bool marginStart = false;
        public bool marginEnd = true;
        public override Vector2 AddToWindow(ScrollWindow instance, Vector2 localPosition, HintSection hintSection)
        {
            Vector2 value = (Vector2)SaltUI.InstantiateDivider(instance, localPosition, marginStart, marginEnd);
            return value;
        }
    }
}
