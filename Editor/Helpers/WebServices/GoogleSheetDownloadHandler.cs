using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace FM.Runtime.Helpers.WebServices
{
    /// <summary>
    /// Helper class used to retrieve data from a Google Sheet document.
    /// This is not intended for runtime usage.
    /// </summary>
    public class GoogleSheetDownloadHandler
    {
        /// <summary>
        /// Base URL for a google sheet document.
        /// </summary>
        public const string GOOGLE_SHEET_URL = "https://docs.google.com/spreadsheets/d/";

        /// <summary>
        /// CSV downloader URL extension.
        /// </summary>
        public const string CSV_FORMAT_EXTENSION = "/export?format=csv";

        /// <summary>
        /// Unique ID of the target document.
        /// </summary>
        public string DocumentID { get; set; }

        /// <summary>
        /// Data format to be passed with the OnDocumentDownloaded callback.
        /// </summary>
        public E_OutputFormat OutputFormat { get; set; }

        /// <summary>
        /// Data Output Format.
        /// </summary>
        public enum E_OutputFormat
        {
            CSV,
            Json
        }

        // Private fields
        private UnityWebRequest _webRequest;
        private bool _isUpdating;

        /// <summary>
        /// Triggered when the document is downloaded.
        /// </summary>
        public event Action<string> OnDocumentDownloaded;

        /// <summary>
        /// Triggered when a Network error was raised.
        /// </summary>
        public event Action<string> OnDownloadError;

        /// <summary>
        /// Create a new GoogleSheetDownloader instance. Output format will default to CSV.
        /// </summary>
        /// <param name="documentID">Document unique ID</param>
        public GoogleSheetDownloadHandler(string documentID) : this(documentID, E_OutputFormat.CSV) { }

        /// <summary>
        /// Create a new GoogleSheetDownloader instance.
        /// </summary>
        /// <param name="documentID">Document unique ID</param>
        /// <param name="outputFormat">Data output format</param>
        public GoogleSheetDownloadHandler(string documentID, E_OutputFormat outputFormat)
        {
            DocumentID = documentID;
            OutputFormat = outputFormat;
        }

        /// <summary>
        /// Create an URL with the document ID
        /// </summary>
        /// <param name="ID">ID of the document</param>
        /// <returns>URL of the document</returns>
        public static string BuildURL(string ID, bool asCSV = false)
        {
            if (asCSV)
            {
                return GOOGLE_SHEET_URL + ID + CSV_FORMAT_EXTENSION;
            }
            else
            {
                return GOOGLE_SHEET_URL + ID;
            }
        }

        /// <summary>
        /// Download the linked Google Sheet file.
        /// </summary>
        /// <returns>Was the download started?</returns>
        public bool Download()
        {
            if (_isUpdating)
            {
                return false;
            }

            // Build the URL
            string url = BuildURL(DocumentID, asCSV: true);

            // Create a new WebRequest
            _webRequest = UnityWebRequest.Get(url);
            _webRequest.SendWebRequest();

            Debug.Log("Download started");

            // Lock the Update callback
            _isUpdating = true;

            // Since we cannot use coroutines in the Editor, use the update callback instead
            EditorApplication.update += Update;

            return true;
        }

        private void Update()
        {
            // Return while the request is not done
            if (!_webRequest.isDone)
            {
                return;
            }

            _isUpdating = false;
            EditorApplication.update -= Update;

            // Handle errors
            if (_webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                OnDownloadError?.Invoke(_webRequest.error);
                Debug.LogError($"Couldn't download the file: {_webRequest.error}");
            }
            // If there are no errors
            else
            {
                // Get the raw CSV text data
                string data = _webRequest.downloadHandler.text;
                Debug.Log("File downloaded");

                // Convert the output to target format
                if (OutputFormat == E_OutputFormat.Json)
                {
                    throw new NotImplementedException("Use CSV format for now. JSON is currently not implemented.");
                }

                // Raise callback
                OnDocumentDownloaded?.Invoke(data);
            }
        }
    }
}