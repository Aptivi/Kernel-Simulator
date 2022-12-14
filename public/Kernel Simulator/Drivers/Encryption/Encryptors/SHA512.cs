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

using KS.Kernel.Debugging;
using System.IO;
using System.Text;
using Encryptor = System.Security.Cryptography.SHA512;
using FS = KS.Files.Filesystem;

namespace KS.Drivers.Encryption.Encryptors
{
    /// <summary>
    /// SHA512 encryptor
    /// </summary>
    public class SHA512 : BaseEncryptionDriver, IEncryptionDriver
    {
        /// <inheritdoc/>
        public override string DriverName => "SHA512";

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.Encryption;

        /// <inheritdoc/>
        public override string EmptyHash => GetEncryptedString("");

        /// <inheritdoc/>
        public override int HashLength => 128;

        /// <inheritdoc/>
        public override string GetEncryptedFile(Stream stream)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", stream.Length);
            var hashbyte = Encryptor.Create().ComputeHash(stream);
            return Encryption.GetArrayEnc(hashbyte);
        }

        /// <inheritdoc/>
        public override string GetEncryptedFile(string Path)
        {
            Path = FS.NeutralizePath(Path);
            var Str = new FileStream(Path, FileMode.Open);
            string Encrypted = GetEncryptedFile(Str);
            Str.Close();
            return Encrypted;
        }

        /// <inheritdoc/>
        public override string GetEncryptedString(string str)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "String length: {0}", str.Length);
            var hashbyte = Encryptor.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            return Encryption.GetArrayEnc(hashbyte);
        }

        /// <inheritdoc/>
        public override bool VerifyHashFromHash(string FileName, string ExpectedHash, string ActualHash) => 
            HashVerifier.VerifyHashFromHash(FileName, DriverName, ExpectedHash, ActualHash);

        /// <inheritdoc/>
        public override bool VerifyHashFromHashesFile(string FileName, string HashesFile, string ActualHash) => 
            HashVerifier.VerifyHashFromHashesFile(FileName, DriverName, HashesFile, ActualHash);

        /// <inheritdoc/>
        public override bool VerifyUncalculatedHashFromHash(string FileName, string ExpectedHash) => 
            HashVerifier.VerifyUncalculatedHashFromHash(FileName, DriverName, ExpectedHash);

        /// <inheritdoc/>
        public override bool VerifyUncalculatedHashFromHashesFile(string FileName, string HashesFile) => 
            HashVerifier.VerifyUncalculatedHashFromHashesFile(FileName, DriverName, HashesFile);
    }
}
