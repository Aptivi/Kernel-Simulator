﻿
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.IO
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

Public Module UserManagement

    'Variables
    ''' <summary>
    ''' The users token
    ''' </summary>
    Public UsersToken As JArray

    ''' <summary>
    ''' A user property
    ''' </summary>
    Public Enum UserProperty
        ''' <summary>
        ''' Username
        ''' </summary>
        Username
        ''' <summary>
        ''' Password
        ''' </summary>
        Password
        ''' <summary>
        ''' List of permissions
        ''' </summary>
        Permissions
    End Enum

    '---------- User Management ----------
    ''' <summary>
    ''' Initializes the uninitialized user (usually a new user)
    ''' </summary>
    ''' <param name="uninitUser">A new user</param>
    ''' <param name="unpassword">A password of a user in encrypted form</param>
    ''' <param name="ComputationNeeded">Whether or not a password encryption is needed</param>
    ''' <param name="ModifyExisting">Changes the password of the existing user</param>
    ''' <returns>True if successful; False if successful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="Exceptions.UserCreationException"></exception>
    Function InitializeUser(uninitUser As String, Optional unpassword As String = "", Optional ComputationNeeded As Boolean = True, Optional ModifyExisting As Boolean = False) As Boolean
        Try
            'Compute hash of a password
            Dim Regexp As New Regex("^([a-fA-F0-9]{64})$")
            If ComputationNeeded Then
                unpassword = GetEncryptedString(unpassword, Algorithms.SHA256)
                Wdbg(DebugLevel.I, "Hash computed.")
            ElseIf Not Regexp.IsMatch(unpassword) Then
                Throw New InvalidOperationException("Trying to add unencrypted password to users list.")
            End If

            'Add user locally
            If Not Users.ContainsKey(uninitUser) Then
                Users.Add(uninitUser, unpassword)
            ElseIf Users.ContainsKey(uninitUser) And ModifyExisting Then
                Users(uninitUser) = unpassword
            End If

            'Add user globally
            If Not UsersToken.Count = 0 Then
                Dim UserExists As Boolean
                Dim ExistingIndex As Integer
                For Each UserToken As JObject In UsersToken
                    If UserToken("username").ToString = uninitUser Then
                        UserExists = True
                        Exit For
                    End If
                    ExistingIndex += 1
                Next
                If Not UserExists Then
                    Dim NewUser As New JObject(New JProperty("username", uninitUser),
                                               New JProperty("password", unpassword),
                                               New JProperty("permissions", New JArray))
                    UsersToken.Add(NewUser)
                ElseIf UserExists And ModifyExisting Then
                    UsersToken(ExistingIndex)("password") = unpassword
                End If
            Else
                Dim NewUser As New JObject(New JProperty("username", uninitUser),
                                           New JProperty("password", unpassword),
                                           New JProperty("permissions", New JArray))
                UsersToken.Add(NewUser)
            End If
            File.WriteAllText(GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))

            'Ready permissions
            Wdbg(DebugLevel.I, "Username {0} added. Readying permissions...", uninitUser)
            InitPermissionsForNewUser(uninitUser)
            Return True
        Catch ex As Exception
            Throw New Exceptions.UserCreationException(DoTranslation("Error trying to add username.") + vbNewLine +
                                                       DoTranslation("Error {0}: {1}"), ex, ex.GetType.FullName, ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Reads the user file and adds them to the list.
    ''' </summary>
    Public Sub InitializeUsers()
        'Opens file stream
        Dim UsersTokenContent As String = File.ReadAllText(GetKernelPath(KernelPathType.Users))
        Dim UninitUsersToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(UsersTokenContent), UsersTokenContent, "{}"))
        For Each UserToken As JObject In UninitUsersToken
            InitializeUser(UserToken("username"), UserToken("password"), False)
        Next
    End Sub

    ''' <summary>
    ''' Loads user token
    ''' </summary>
    Sub LoadUserToken()
        If Not File.Exists(GetKernelPath(KernelPathType.Users)) Then File.Create(GetKernelPath(KernelPathType.Users)).Close()
        Dim UsersTokenContent As String = File.ReadAllText(GetKernelPath(KernelPathType.Users))
        UsersToken = JArray.Parse(If(Not String.IsNullOrEmpty(UsersTokenContent), UsersTokenContent, "[]"))
    End Sub

    ''' <summary>
    ''' Gets user property
    ''' </summary>
    ''' <param name="User">Target user</param>
    ''' <param name="PropertyType">Property type</param>
    ''' <returns>JSON token of user property</returns>
    Public Function GetUserProperty(User As String, PropertyType As UserProperty) As JToken
        For Each UserToken As JObject In UsersToken
            If UserToken("username").ToString = User Then
                Return UserToken.SelectToken(PropertyType.ToString.ToLower)
            End If
        Next
    End Function

    ''' <summary>
    ''' Sets user property
    ''' </summary>
    ''' <param name="User">Target user</param>
    ''' <param name="PropertyType">Property type</param>
    ''' <param name="Value">Value</param>
    Public Sub SetUserProperty(User As String, PropertyType As UserProperty, Value As String)
        For Each UserToken As JObject In UsersToken
            If UserToken("username").ToString = User Then
                Select Case PropertyType
                    Case UserProperty.Username
                        UserToken("username") = Value
                    Case UserProperty.Password
                        UserToken("password") = Value
                    Case UserProperty.Permissions
                        Throw New NotSupportedException("Use AddPermission and RemovePermission for this.")
                    Case Else
                        Throw New ArgumentException("Property type is invalid")
                End Select
            End If
        Next
        File.WriteAllText(GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Adds a new user
    ''' </summary>
    ''' <param name="newUser">A new user</param>
    ''' <param name="newPassword">A password</param>
    ''' <exception cref="Exceptions.UserCreationException"></exception>
    Public Function AddUser(newUser As String, Optional newPassword As String = "") As Boolean
        'Adds user
        Wdbg(DebugLevel.I, "Creating user {0}...", newUser)
        If InStr(newUser, " ") > 0 Then
            Wdbg(DebugLevel.W, "There are spaces in username.")
            Throw New Exceptions.UserCreationException(DoTranslation("Spaces are not allowed."))
        ElseIf newUser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
            Wdbg(DebugLevel.W, "There are special characters in username.")
            Throw New Exceptions.UserCreationException(DoTranslation("Special characters are not allowed."))
        ElseIf newUser = Nothing Then
            Wdbg(DebugLevel.W, "Username is blank.")
            Throw New Exceptions.UserCreationException(DoTranslation("Blank username."))
        ElseIf Not Users.ContainsKey(newUser) Then
            Try
                If newPassword = Nothing Then
                    Wdbg(DebugLevel.W, "Initializing user with no password")
                    InitializeUser(newUser)
                Else
                    Wdbg(DebugLevel.I, "Initializing user with password")
                    InitializeUser(newUser, newPassword)
                End If
                EventManager.RaiseUserAdded(newUser)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to create user {0}: {1}", ex.Message)
                WStkTrc(ex)
                Throw New Exceptions.UserCreationException(DoTranslation("usrmgr: Failed to create username {0}: {1}"), ex, newUser, ex.Message)
            End Try
        Else
            Wdbg(DebugLevel.W, "User {0} already found.", newUser)
            Throw New Exceptions.UserCreationException(DoTranslation("usrmgr: Username {0} is already found"), newUser)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Removes a user from users database
    ''' </summary>
    ''' <param name="user">A user</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.UserManagementException"></exception>
    ''' <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
    Public Function RemoveUser(user As String) As Boolean
        If InStr(user, " ") > 0 Then
            Wdbg(DebugLevel.W, "There are spaces in username.")
            Throw New Exceptions.UserManagementException(DoTranslation("Spaces are not allowed."))
        ElseIf user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
            Wdbg(DebugLevel.W, "There are special characters in username.")
            Throw New Exceptions.UserManagementException(DoTranslation("Special characters are not allowed."))
        ElseIf user = Nothing Then
            Wdbg(DebugLevel.W, "Username is blank.")
            Throw New Exceptions.UserManagementException(DoTranslation("Blank username."))
        ElseIf Users.ContainsKey(user) = False Then
            Wdbg(DebugLevel.W, "Username {0} not found in list", user)
            Throw New Exceptions.UserManagementException(DoTranslation("User {0} not found."), user)
        Else
            'Try to remove user
            If Users.Keys.ToArray.Contains(user) And user = "root" Then
                Wdbg(DebugLevel.W, "User is root, and is a system account")
                Throw New Exceptions.UserManagementException(DoTranslation("User {0} isn't allowed to be removed."), user)
            ElseIf Users.Keys.ToArray.Contains(user) And user = CurrentUser Then
                Wdbg(DebugLevel.W, "User has logged in, so can't delete self.")
                Throw New Exceptions.UserManagementException(DoTranslation("User {0} is already logged in. Log-out and log-in as another admin."), user)
            ElseIf Users.Keys.ToArray.Contains(user) And user <> "root" Then
                Try
                    Wdbg(DebugLevel.I, "Removing permissions...")
                    UserPermissions.Remove(user)

                    'Remove user
                    Wdbg(DebugLevel.I, "Removing username {0}...", user)
                    Users.Remove(user)

                    'Remove user from Users.json
                    For Each UserToken As JObject In UsersToken
                        If UserToken("username").ToString = user Then
                            UserToken.Remove()
                            Exit For
                        End If
                    Next
                    File.WriteAllText(GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))

                    'Raise event
                    EventManager.RaiseUserRemoved(user)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.UserManagementException(DoTranslation("Error trying to remove username.") + vbNewLine +
                                                                 DoTranslation("Error {0}: {1}"), ex, ex.Message)
                End Try
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Changes the username
    ''' </summary>
    ''' <param name="OldName">Old username</param>
    ''' <param name="Username">New username</param>
    Public Function ChangeUsername(OldName As String, Username As String) As Boolean
        If Users.ContainsKey(OldName) Then
            If Not Users.ContainsKey(Username) Then
                Try
                    'Store user password
                    Dim Temporary As String = Users(OldName)

                    'Rename username in dictionary
                    Users.Remove(OldName)
                    Users.Add(Username, Temporary)
                    PermissionEditForNewUser(OldName, Username)

                    'Rename username in Users.json
                    SetUserProperty(OldName, UserProperty.Username, Username)

                    'Raise event
                    EventManager.RaiseUsernameChanged(OldName, Username)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.UserManagementException(DoTranslation("Failed to rename user. {0}"), ex, ex.Message)
                End Try
            Else
                Throw New Exceptions.UserManagementException(DoTranslation("The new name you entered is already found."))
            End If
        Else
            Throw New Exceptions.UserManagementException(DoTranslation("User {0} not found."), OldName)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Initializes root account
    ''' </summary>
    Sub InitializeSystemAccount()
        If setRootPasswd Then
            InitializeUser("root", RootPasswd, True, True)
        ElseIf File.Exists(GetKernelPath(KernelPathType.Users)) Then
            If GetUserProperty("root", UserProperty.Password) IsNot Nothing Then
                InitializeUser("root", GetUserProperty("root", UserProperty.Password), False, True)
            Else
                InitializeUser("root", "", True, True)
            End If
        Else
            InitializeUser("root", "", True, True)
        End If
        Permission(PermissionType.Administrator, "root", PermissionManagementMode.Allow)
    End Sub

    ''' <summary>
    ''' Changes user password
    ''' </summary>
    ''' <param name="Target">Target username</param>
    ''' <param name="CurrentPass">Current user password</param>
    ''' <param name="NewPass">New user password</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.UserManagementException"></exception>
    Public Function ChangePassword(Target As String, CurrentPass As String, NewPass As String)
        CurrentPass = GetEncryptedString(CurrentPass, Algorithms.SHA256)
        If CurrentPass = Users(Target) Then
            If HasPermission(CurrentUser, PermissionType.Administrator) And Users.ContainsKey(Target) Then
                'Change password locally
                NewPass = GetEncryptedString(NewPass, Algorithms.SHA256)
                Users.Item(Target) = NewPass

                'Change password globally
                SetUserProperty(Target, UserProperty.Password, NewPass)

                'Raise event
                EventManager.RaiseUserPasswordChanged(Target)
                Return True
            ElseIf HasPermission(CurrentUser, PermissionType.Administrator) And Not Users.ContainsKey(Target) Then
                Throw New Exceptions.UserManagementException(DoTranslation("User not found"))
            ElseIf HasPermission(Target, PermissionType.Administrator) And Not HasPermission(CurrentUser, PermissionType.Administrator) Then
                Throw New Exceptions.UserManagementException(DoTranslation("You are not authorized to change password of {0} because the target was an admin."), Target)
            End If
        Else
            Throw New Exceptions.UserManagementException(DoTranslation("Wrong user password."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Lists all users and includes anonymous and disabled users if enabled.
    ''' </summary>
    ''' <param name="IncludeAnonymous">Include anonymous users</param>
    ''' <param name="IncludeDisabled">Include disabled users</param>
    Public Function ListAllUsers(Optional IncludeAnonymous As Boolean = False, Optional IncludeDisabled As Boolean = False) As List(Of String)
        Dim UsersList As New List(Of String)(Users.Keys)
        If Not IncludeAnonymous Then
            UsersList.RemoveAll(New Predicate(Of String)(Function(x) HasPermission(x, PermissionType.Anonymous) = True))
        End If
        If Not IncludeDisabled Then
            UsersList.RemoveAll(New Predicate(Of String)(Function(x) HasPermission(x, PermissionType.Disabled) = True))
        End If
        Return UsersList
    End Function

    ''' <summary>
    ''' Handles the prompts for setting up a first user
    ''' </summary>
    Sub FirstUserTrigger()
        Dim [Step] As Integer = 1
        Dim AnswerUsername As String = ""
        Dim AnswerPassword As String = ""
        Dim AnswerType As Integer

        'First, select user name
        W(DoTranslation("It looks like you've got no user except root. This is bad. We'll guide you how to create one."), True, ColTypes.Neutral)
        While [Step] = 1
            W(DoTranslation("Write your username.") + vbNewLine, True, ColTypes.Neutral)
            W(">> ", False, ColTypes.Input)
            AnswerUsername = Console.ReadLine
            Wdbg(DebugLevel.I, "Answer: {0}", AnswerUsername)
            If String.IsNullOrWhiteSpace(AnswerUsername) Then
                Wdbg(DebugLevel.W, "Username is not valid. Returning...")
                W(DoTranslation("You must write your username."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            Else
                [Step] += 1
            End If
        End While

        'Second, write password
        While [Step] = 2
            W(DoTranslation("Write your password.") + vbNewLine, True, ColTypes.Neutral)
            W(">> ", False, ColTypes.Input)
            AnswerPassword = ReadLineNoInput("*")
            Console.WriteLine()
            Wdbg(DebugLevel.I, "Answer: {0}", AnswerPassword)
            If String.IsNullOrWhiteSpace(AnswerPassword) Then
                Wdbg(DebugLevel.W, "Password is not valid. Returning...")
                W(DoTranslation("You must write your password."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            Else
                [Step] += 1
            End If
        End While

        'Third, select account type
        While [Step] = 3
            W(DoTranslation("Select account type.") + vbNewLine, True, ColTypes.Neutral)
            W("1) " + DoTranslation("Administrator: This account type has the most power in the kernel, allowing you to use system management programs."), True, ColTypes.Option)
            W("2) " + DoTranslation("Normal User: This account type is slightly more restricted than administrators."), True, ColTypes.Option)
            W(vbNewLine + ">> ", False, ColTypes.Input)
            If Integer.TryParse(Console.ReadLine, AnswerType) Then
                Wdbg(DebugLevel.I, "Answer: {0}", AnswerType)
                Select Case AnswerType
                    Case 1, 2
                        [Step] += 1
                    Case Else '???
                        Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                        W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerType)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                End Select
            Else
                Wdbg(DebugLevel.W, "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While

        'Finally, create an account
        AddUser(AnswerUsername, AnswerPassword)
        If AnswerType = 1 Then Permission(PermissionType.Administrator, AnswerUsername, PermissionManagementMode.Allow)
        W(DoTranslation("Congratulations! You've made a new account! To finish this off, log in as your new account."), True, ColTypes.Neutral)
    End Sub

End Module
