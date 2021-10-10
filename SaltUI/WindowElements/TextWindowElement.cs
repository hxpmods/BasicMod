using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI.WindowElements
{
    public class TextWindowElement : AbstractWindowElement
    {
        public string text;
        public override Vector2 AddToWindow(ScrollWindow instance, Vector2 localPosition, HintSection hintSection)
        {
            Vector2 value = (Vector2)SaltUI.InstantiateText(instance, localPosition, text, hintSection != null);
            return value;
        }
    }
}
