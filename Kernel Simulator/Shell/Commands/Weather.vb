﻿
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Misc.Forecast

Namespace Shell.Commands
    Class WeatherCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ListMode As Boolean
            If ListSwitchesOnly.Contains("-list") Then ListMode = True
            If ListMode Then
                Dim Cities As Dictionary(Of Long, String) = Core.Forecast.ListAllCities()
                WriteList(Cities)
            Else
                Dim APIKey As String = Forecast.ApiKey
                If ListArgsOnly.Length > 1 Then
                    APIKey = ListArgsOnly(1)
                ElseIf String.IsNullOrEmpty(APIKey) Then
                    Write(DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys."), True, ColTypes.Neutral)
                    Write(DoTranslation("Enter your API key:") + " ", False, ColTypes.Input)
                    APIKey = ReadLineNoInput()
                    Forecast.ApiKey = APIKey
                    Console.WriteLine()
                End If
                PrintWeatherInfo(ListArgsOnly(0), APIKey)
            End If
        End Sub

        Public Sub HelpHelper()
            Write(DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + DoTranslation("Or, pass ""listcities"" to this command."), True, ColTypes.Neutral)
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -list: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows all the available cities"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace