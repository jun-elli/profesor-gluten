using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Extensions;

namespace Dialogue
{
    /// <summary>
    /// Data container for information on a running process initiated by a command
    /// </summary>
    public class CommandProcess
    {
        public Guid id;
        public string processName;
        public Delegate command;
        public CoroutineWrapper runningProcess;
        public string[] args;
        public UnityEvent onTerminateAction;

        public CommandProcess(Guid id, string processName, Delegate command, CoroutineWrapper runningProcess, string[] args, UnityEvent onTerminateAction = null)
        {
            this.id = id;
            this.processName = processName;
            this.command = command;
            this.runningProcess = runningProcess;
            this.args = args;
            this.onTerminateAction = onTerminateAction;
        }
    }
}
