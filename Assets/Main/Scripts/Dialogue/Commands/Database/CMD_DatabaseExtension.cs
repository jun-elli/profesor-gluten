using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Commands
{
    public abstract class CMD_DatabaseExtension
    {
        public static void Extend(CommandsDatabase database) { }
        public static CommandParameters ConvertToCommandParameters(string[] data) => new CommandParameters(data);

    }
}
