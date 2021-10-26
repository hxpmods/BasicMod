using System;
using System.IO;
using UnityEngine;

//Thank you RoboPhred https://github.com/RoboPhred

namespace BasicMod
{
    static public class SpriteLoader
    {
        public static Sprite LoadSpriteFromFile(string filePath, string directory = null)
        {
            bool overrideModFilePath = filePath.StartsWith("@");

            if (directory == null || overrideModFilePath) directory = "BepinEx/Plugins/Assets/";
            if (overrideModFilePath) filePath = filePath.Remove(0, 1);


            var data = File.ReadAllBytes(directory+filePath);
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