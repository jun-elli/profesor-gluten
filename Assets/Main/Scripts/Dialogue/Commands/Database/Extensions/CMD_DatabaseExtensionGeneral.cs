using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Commands
{
    public class CMD_DatabaseExtensionGeneral : CMD_DatabaseExtension
    {
        new public static void Extend(CommandsDatabase database)
        {
            database.AddCommand("Wait", new Func<string, IEnumerator>(Wait));
        }

        private static IEnumerator Wait(string data)
        {
            if (float.TryParse(data, out float floatSeconds))
            {
                yield return new WaitForSeconds(floatSeconds);

            }
            else if (int.TryParse(data, out int intSeconds))
            {
                yield return new WaitForSeconds(intSeconds);
            }
        }
    }
}
