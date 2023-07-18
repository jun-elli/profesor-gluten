using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterModel3D : Character
    {
        public CharacterModel3D(string name) : base(name)
        {
            Debug.Log($"Character Model3D created: {name}");
        }
    }
}