﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Threading
{
    /// <summary>
    /// The kernel thread to simplify the access to making new threads, starting them, and stopping them
    /// </summary>
    public class KernelThread
    {

        private Thread BaseThread;
        private readonly ThreadStart ThreadDelegate;
        private readonly ParameterizedThreadStart ThreadDelegateParameterized;
        private readonly bool IsParameterized;

        /// <summary>
        /// The name of the thread
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is the thread a background thread?
        /// </summary>
        public bool IsBackground { get; private set; }

        /// <summary>
        /// Is the kernel thread alive?
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return BaseThread.IsAlive;
            }
        }

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        public KernelThread(string ThreadName, bool Background, ThreadStart Executor)
        {
            BaseThread = new Thread(Executor) { Name = ThreadName, IsBackground = Background };
            IsParameterized = false;
            ThreadDelegate = Executor;
            Name = ThreadName;
            IsBackground = Background;
            DebugWriter.Wdbg(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", ThreadName, BaseThread.ManagedThreadId);
            ThreadManager.KernelThreads.Add(this);
        }

        /// <summary>
        /// Makes a new kernel thread
        /// </summary>
        /// <param name="ThreadName">The thread name</param>
        /// <param name="Background">Indicates if the kernel thread is background</param>
        /// <param name="Executor">The thread delegate</param>
        public KernelThread(string ThreadName, bool Background, ParameterizedThreadStart Executor)
        {
            BaseThread = new Thread(Executor) { Name = ThreadName, IsBackground = Background };
            IsParameterized = true;
            ThreadDelegateParameterized = Executor;
            Name = ThreadName;
            IsBackground = Background;
            DebugWriter.Wdbg(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", ThreadName, BaseThread.ManagedThreadId);
            ThreadManager.KernelThreads.Add(this);
        }

        /// <summary>
        /// Starts the kernel thread
        /// </summary>
        public void Start()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Starting kernel thread {0} with ID {1}", BaseThread.Name, BaseThread.ManagedThreadId);
            BaseThread.Start();
        }

        /// <summary>
        /// Starts the kernel thread
        /// </summary>
        /// <param name="Parameter">The parameter class instance containing multiple parameters, or a usual single parameter</param>
        public void Start(object Parameter)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Starting kernel thread {0} with ID {1} with parameters", BaseThread.Name, BaseThread.ManagedThreadId);
            BaseThread.Start(Parameter);
        }

        /// <summary>
        /// Stops the kernel thread
        /// </summary>
        public void Stop()
        {
            DebugWriter.Wdbg(DebugLevel.I, "Stopping kernel thread {0} with ID {1}", Name, BaseThread.ManagedThreadId);
            BaseThread.Interrupt();

            // Remake the thread to avoid illegal state exceptions
            if (IsParameterized)
            {
                BaseThread = new Thread(ThreadDelegateParameterized) { Name = Name, IsBackground = IsBackground };
            }
            else
            {
                BaseThread = new Thread(ThreadDelegate) { Name = Name, IsBackground = IsBackground };
            }
            DebugWriter.Wdbg(DebugLevel.I, "Made a new kernel thread {0} with ID {1}", Name, BaseThread.ManagedThreadId);
        }

        /// <summary>
        /// Waits for the kernel thread to finish
        /// </summary>
        public void Wait()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Waiting for kernel thread {0} with ID {1}", BaseThread.Name, BaseThread.ManagedThreadId);
                BaseThread.Join();
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Can't wait for kernel thread: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

    }
}