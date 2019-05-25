namespace Shop.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IMvxSimpleFileStoreService
    {
        bool TryReadTextFile(string path, out string contents);

        bool TryReadBinaryFile(string path, out byte[] contents);

        bool TryReadBinaryFile(string path, Func<Stream, bool> readMethod);

        void WriteFile(string path, string contents);

        void WriteFile(string path, IEnumerable<byte> contents);

        void WriteFile(string path, Action<Stream> writeMethod);

        bool TryMove(string from, string to, bool deleteExistingTo);

        bool Exists(string path);

        void EnsureFolderExists(string folderPath);

        IEnumerable<string> GetFilesIn(string folderPath);

        void DeleteFile(string path);
    }
}