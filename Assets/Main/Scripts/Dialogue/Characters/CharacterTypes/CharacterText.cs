using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterText : Character
    {
        public CharacterText(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Character text created: {name}");
        }
    }
}
