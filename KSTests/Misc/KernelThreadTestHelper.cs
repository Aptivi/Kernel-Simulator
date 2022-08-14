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

using System.Threading;
using System;

namespace KSTests
{

    public static class KernelThreadTestHelper
    {

        /// <summary>
        /// [Kernel thread test] Write hello to console
        /// </summary>
        public static void WriteHello()
        {
            try
            {
                Console.WriteLine("Hello world!");
                Console.WriteLine("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
                while (true)
                    Thread.Sleep(1);
            }
            catch
            {
                Console.WriteLine("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
            }
        }

        /// <summary>
        /// [Kernel thread test] Write hello to console with argument
        /// </summary>
        public static void WriteHelloWithArgument(string Name)
        {
            try
            {
                Console.WriteLine("Hello, {0}!", Name);
                Console.WriteLine("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
                while (true)
                    Thread.Sleep(1);
            }
            catch
            {
                Console.WriteLine("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId);
            }
        }

    }
}
