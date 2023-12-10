﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Lists all commits
    /// </summary>
    /// <remarks>
    /// This command lets you list all commits in your Git repository.
    /// </remarks>
    class LsCommitsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var commits = GitShellCommon.Repository.Commits;
            TextWriterColor.Write(Translate.DoTranslation("Commits for branch") + $" {GitShellCommon.BranchName}:");
            foreach (var commit in commits)
            {
                TextWriterColor.WriteKernelColor($"- {commit.Sha[..7]}, {commit.Committer.Name} <{commit.Committer.Email}>", true, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"  - {commit.MessageShort}", true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}
