using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class PlayerInputManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                OnUserPromptAdvance();
            }
        }

        public void OnUserPromptAdvance()
        {
            DialogueSystem.Instance.OnUserPromptAdvance();
        }
    }
}
