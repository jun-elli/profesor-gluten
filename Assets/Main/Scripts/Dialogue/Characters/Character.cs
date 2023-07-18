using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public abstract class Character
    {
        public string name = "";
        public RectTransform transform = null;

        public Character(string name)
        {
            this.name = name;
        }

        public enum CharacterType { Text, Sprite, SpriteSheet, Live2D, Model3D }
    }
}