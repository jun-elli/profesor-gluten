using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterModel3D : Character
    {
        public CharacterModel3D(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Character Model3D created: {name}");
        }
    }
}