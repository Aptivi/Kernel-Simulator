﻿using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Calendar;

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

using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Commands
{
	class CalendarCommand : CommandExecutor, ICommand
	{

		public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
		{
			string Action = ListArgsOnly[0];

			// Enumerate based on action
			int ActionMinimumArguments = 1;
			string[] ActionArguments = ListArgsOnly.Skip(1).ToArray();
			switch (Action ?? "")
			{
				case "show":
					{
						// User chose to show the calendar
						if (ActionArguments.Length != 0)
						{
							try
							{
								string StringYear = ActionArguments[0];
								string StringMonth = DateTime.Today.Month.ToString();
								if (ActionArguments.Length >= 2)
									StringMonth = ActionArguments[1];

								// Show the calendar using the provided year and month
								CalendarPrint.PrintCalendar(Conversions.ToInteger(StringYear), Conversions.ToInteger(StringMonth));
							}
							catch (Exception ex)
							{
								DebugWriter.WStkTrc(ex);
								TextWriterColor.Write(Translate.DoTranslation("Failed to add or remove an event.") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
							}
						}
						else
						{
							CalendarPrint.PrintCalendar();
						}

						break;
					}
				case "event":
					{
						// User chose to manipulate with the day events
						if (ActionArguments.Length >= ActionMinimumArguments)
						{
							// User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
							string ActionType = ActionArguments[0];
							switch (ActionType ?? "")
							{
								case "add":
									{
										// Parse the arguments to check to see if enough arguments are passed to those parameters
										ActionMinimumArguments = 3;
										if (ActionArguments.Length >= ActionMinimumArguments)
										{
											// Enough arguments provided.
											try
											{
												string StringDate = ActionArguments[1];
												string EventTitle = ActionArguments[2];
												var ParsedDate = DateTime.Parse(StringDate);
												EventManager.AddEvent(ParsedDate, EventTitle);
											}
											catch (Exception ex)
											{
												DebugWriter.WStkTrc(ex);
												TextWriterColor.Write(Translate.DoTranslation("Failed to add an event.") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
											}
										}
										else
										{
											TextWriterColor.Write(Translate.DoTranslation("Not enough arguments provided to add an event."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
										}

										break;
									}
								case "remove":
									{
										// Parse the arguments to check to see if enough arguments are passed to those parameters
										ActionMinimumArguments = 2;
										if (ActionArguments.Length >= ActionMinimumArguments)
										{
											// Enough arguments provided.
											try
											{
												int EventId = Conversions.ToInteger(ActionArguments[1]);
												var EventInstance = EventManager.CalendarEvents[EventId - 1];
												EventManager.RemoveEvent(EventInstance.EventDate, EventId);
											}
											catch (Exception ex)
											{
												DebugWriter.WStkTrc(ex);
												TextWriterColor.Write(Translate.DoTranslation("Failed to remove an event.") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
											}
										}
										else
										{
											TextWriterColor.Write(Translate.DoTranslation("Not enough arguments provided to remove an event."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
										}

										break;
									}
								case "list":
									{
										// User chose to list. No parse needed as we're only listing.
										EventManager.ListEvents();
										break;
									}
								case "saveall":
									{
										// User chose to save all.
										EventManager.SaveEvents();
										break;
									}

								default:
									{
										// Invalid action.
										TextWriterColor.Write(Translate.DoTranslation("Invalid action."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
										break;
									}
							}
						}
						else
						{
							TextWriterColor.Write(Translate.DoTranslation("Not enough arguments provided for event manipulation."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						}

						break;
					}
				case "reminder":
					{
						// User chose to manipulate with the day reminders
						if (ActionArguments.Length >= ActionMinimumArguments)
						{
							// User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
							string ActionType = ActionArguments[0];
							switch (ActionType ?? "")
							{
								case "add":
									{
										// Parse the arguments to check to see if enough arguments are passed to those parameters
										ActionMinimumArguments = 3;
										if (ActionArguments.Length >= ActionMinimumArguments)
										{
											// Enough arguments provided.
											try
											{
												string StringDate = ActionArguments[1];
												string ReminderTitle = ActionArguments[2];
												var ParsedDate = DateTime.Parse(StringDate);
												ReminderManager.AddReminder(ParsedDate, ReminderTitle);
											}
											catch (Exception ex)
											{
												DebugWriter.WStkTrc(ex);
												TextWriterColor.Write(Translate.DoTranslation("Failed to add a reminder.") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
											}
										}
										else
										{
											TextWriterColor.Write(Translate.DoTranslation("Not enough arguments provided to add a reminder."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
										}

										break;
									}
								case "remove":
									{
										// Parse the arguments to check to see if enough arguments are passed to those parameters
										ActionMinimumArguments = 2;
										if (ActionArguments.Length >= ActionMinimumArguments)
										{
											// Enough arguments provided.
											try
											{
												int ReminderId = Conversions.ToInteger(ActionArguments[1]);
												var ReminderInstance = ReminderManager.Reminders[ReminderId - 1];
												ReminderManager.RemoveReminder(ReminderInstance.ReminderDate, ReminderId);
											}
											catch (Exception ex)
											{
												DebugWriter.WStkTrc(ex);
												TextWriterColor.Write(Translate.DoTranslation("Failed to remove a reminder.") + " {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), ex.Message);
											}
										}
										else
										{
											TextWriterColor.Write(Translate.DoTranslation("Not enough arguments provided to remove a reminder."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
										}

										break;
									}
								case "list":
									{
										// User chose to list. No parse needed as we're only listing.
										ReminderManager.ListReminders();
										break;
									}
								case "saveall":
									{
										// User chose to save all.
										ReminderManager.SaveReminders();
										break;
									}

								default:
									{
										// Invalid action.
										TextWriterColor.Write(Translate.DoTranslation("Invalid action."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
										break;
									}
							}
						}
						else
						{
							TextWriterColor.Write(Translate.DoTranslation("Not enough arguments provided for reminder manipulation."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						}

						break;
					}

				default:
					{
						// Invalid action.
						TextWriterColor.Write(Translate.DoTranslation("Invalid action."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
						break;
					}
			}
		}

	}
}