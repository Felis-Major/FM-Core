using Daniell.Runtime.Helpers.DataStructures;
using Daniell.Runtime.Helpers.Logging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Daniell.Runtime.Helpers.Files
{
    /// <summary>
    /// Base class for handling files
    /// </summary>
    public class FileHandler
    {
        /* ==========================
         * > Data Structures
         * -------------------------- */

        /// <summary>
        /// Type of the file
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// Plain text
            /// </summary>
            Text,

            /// <summary>
            /// JSON file
            /// </summary>
            Json,

            /// <summary>
            /// Binary formatted file
            /// </summary>
            Binary
        }


        /* ==========================
         * > Private Fields
         * -------------------------- */

        protected readonly FileType _fileType;
        protected readonly string _filePath;


        /* ==========================
         * > Constructors
         * -------------------------- */

        public FileHandler(string filePath, FileType fileType)
        {
            _filePath = filePath;
            _fileType = fileType;
        }


        /* ==========================
         * > Methods
         * -------------------------- */

        /// <summary>
        /// Write data to a file
        /// </summary>
        /// <typeparam name="T">Data to be written</typeparam>
        /// <param name="data">Data Type</param>
        public void Write<T>(T data)
        {
            // Create directory if it doesn't exist
            var dir = Path.GetDirectoryName(_filePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Write data to file
            switch (_fileType)
            {
                // Write file as text
                case FileType.Text:
                    File.WriteAllText(_filePath, data.ToString());
                    break;

                // Write file as Json
                case FileType.Json:
                    var jsonData = JsonUtility.ToJson(new ValueWrapper<T>(data), true);
                    File.WriteAllText(_filePath, jsonData);
                    break;

                // Write file as Binary
                case FileType.Binary:
                    // Create the file
                    FileStream stream = new FileStream(_filePath, FileMode.Create);

                    // Serialize Data and save to file
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, new ValueWrapper<T>(data));

                    // Close the file stream
                    stream.Close();
                    break;
            }
        }

        /// <summary>
        /// Read data from a file
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <returns>Data as T</returns>
        public T Read<T>()
        {
            // Throw an exception if the file doesn't exist
            if (!File.Exists(_filePath))
            {
                throw new System.Exception($"File {_filePath} doesn't exist");
            }

            // Read data from file
            switch (_fileType)
            {
                case FileType.Text:
                    return (T)(object)File.ReadAllText(_filePath);

                case FileType.Json:
                    string json = File.ReadAllText(_filePath);
                    return JsonUtility.FromJson<ValueWrapper<T>>(json).value;

                case FileType.Binary:
                    // Open the file
                    FileStream stream = new FileStream(_filePath, FileMode.Open);

                    // Deserialize data
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    var serializedData = (ValueWrapper<T>)binaryFormatter.Deserialize(stream);
                    stream.Close();

                    return serializedData.value;
            }

            // Return default if a case wasn't handled
            return default;
        }
    }
}