using System;
using System.IO;
using UnityEngine;

//Thank you RoboPhred https://github.com/RoboPhred

namespace BasicMod
{
    static public class SpriteLoader
    {
        public static Sprite LoadSpriteFromFile(string filePath)
        {
            var data = File.ReadAllBytes("BepinEx/Plugins/Assets/"+filePath);
            var tex = new Texture2D(2, 2);
            tex.filterMode = FilterMode.Bilinear;
            if (!tex.LoadImage(data))
            {
                throw new Exception("Failed to load image from file: " + filePath);
            }
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width+0, tex.height+0), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}