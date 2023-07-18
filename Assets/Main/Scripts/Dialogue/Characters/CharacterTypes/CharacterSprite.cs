using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterSprite : Character
    {
        public CharacterSprite(string name) : base(name)
        {
            Debug.Log($"Character Sprite created: {name}");

        }
    }
}