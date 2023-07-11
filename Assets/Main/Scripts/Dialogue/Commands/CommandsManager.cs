using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// Class in charge of looking for commands in DB and executing them.
    /// Will recieve requests.
    /// </summary>
    public class CommandsManager : MonoBehaviour
    {
        public static CommandsManager Instance { get; private set; }
        private CommandsDatabase database;

        private void Awake()
        {
            if (Instance != null)
            {
                Instance = this;
                database = new CommandsDatabase();
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}