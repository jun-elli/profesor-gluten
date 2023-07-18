using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterText : Character
    {
        public CharacterText(string name) : base(name)
        {
            Debug.Log($"Character Text created: {name}");
        }
    }
}
