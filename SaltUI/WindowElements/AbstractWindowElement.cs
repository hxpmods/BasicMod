using ScrollWindowHint;
using UnityEngine;

namespace BasicMod.SaltUI
{
    public abstract class AbstractWindowElement
    {
        public abstract Vector2 AddToWindow(ScrollWindow instance, Vector2 localPosition, HintSection hintSection);
    }
}
