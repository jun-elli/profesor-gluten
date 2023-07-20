using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterLive2D : Character
    {
        public CharacterLive2D(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Character Live2D created: {name}");
        }
    }
}