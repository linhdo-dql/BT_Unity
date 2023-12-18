using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class GoogleDriveSample : EditorWindow
{
    /// < summary >
    /// Key JSON file
    /// </ summary >
    private const string JSON_FILE = @"Assets/Resources/Json/srndrive-42f20a65f891.json";
    /// < summary >
    /// Google Drive folder ID
    /// </ summary >
    private string GOOGLE_DRIVE_FOLDER_ID = "1JmjbEwNKXFI7nNN6EVEVgd1dM7C7YdZj";
    /// < summary >
    /// File path to upload
    /// </ summary >
    private const string FILE_PATH = @"C:\sample\LocalFile.txt";
    /// < summary >
    /// File ID to download
    /// </ summary >
    private const string DOWNLOAD_FILE_ID = "1wUeXUAAowK30dY6SZvmxhe7BpsThLmKh";
    /// < summary >
    /// Download file save path
    /// </ summary >
    private const string SAVE_PATH = @"Assets/Resources";
    /// < summary >
    /// canvas text
    /// </ summary >
    [SerializeField] private TextMeshProUGUI _canvasText;
    /// < summary >
    /// Google Drive services
    /// </ summary >
    private DriveService _driveService;
    private string savePath = "Assets/Resources/Animations/Main/";
    private string statusText = "Status: ";
    private bool isDownloading;
    private int downloadProgress = 0;
    private int totalFile = 0;

    // Thiết lập một cửa sổ mới để hiển thị kết quả và thông báo lỗi
    [MenuItem("Window/GoogleDriveSample")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GoogleDriveSample));
    }

    // Khởi tạo đối tượng Drive API Service khi cửa sổ được hiển thị
    void OnEnable()
    {
        InitializeAPIService();
    }
    private void InitializeAPIService()
    {
        // Get authentication information
        GoogleCredential credential;
        using (var stream = new FileStream(JSON_FILE, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
        }
        //Create Drive API service
        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google Drive Sample",
        });
    }
    private void OnGUI()
    {
        GUILayout.Label("Google Drive Editor", EditorStyles.boldLabel);

        // Hiển thị ô input để nhập folder ID
        GOOGLE_DRIVE_FOLDER_ID = EditorGUILayout.TextField("Folder ID:", GOOGLE_DRIVE_FOLDER_ID);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save Path:", savePath);
        if (GUILayout.Button("Browse"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Save Path", "", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                savePath = selectedPath;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(isDownloading);
        if (GUILayout.Button("Download Folder"))
        {
            isDownloading = true;
            Thread downloadThread = new(DownFolder);
            downloadThread.Start();
            EditorApplication.update += UpdateDownload;
        }
        EditorGUI.EndDisabledGroup();
        // Hiển thị thanh tiến trình download
        // if (isDownloading)
        // {
        EditorGUILayout.LabelField("Download Progress: " + downloadProgress+"/"+totalFile);
        //}
        EditorGUILayout.LabelField("Status:", statusText);
    }
    /// < summary >
    /// List files
    /// </ summary >
    ///
    private void UpdateDownload()
    {
        Repaint(); // Cập nhật lại giao diện người dùng
    }
    public void List()
    {
        try
        {
            List(_driveService);
        }
        catch (Exception e)
        {
            OutputCanvasText(e.Message);
        }
    }
    /// < summary >
    /// List files
    /// </ summary >
    /// < param name = "service" > Google Drive service </ param > 
    private void List(DriveService service)
    {
        OutputCanvasText("List start");
        //Search inside the folder
        var request = service.Files.List();
        request.Q = "'" + GOOGLE_DRIVE_FOLDER_ID + "' in parents";
        // You can get all information with files(*)
        request.Fields = "nextPageToken, files(id, name, size, createdTime)";
        var files = new List<Google.Apis.Drive.v3.Data.File>();
        do
        {
            var result = request.Execute();
            files.AddRange(result.Files);
            request.PageToken = result.NextPageToken;
        } while (!string.IsNullOrEmpty(request.PageToken));
        // print the result
        foreach (var file in files)
        {
            OutputCanvasText("Name: " + file.Name + " ID: " + file.Id +
                             " Size: " + file.Size + "byte CreatedTime: " + file.CreatedTime);
        }
        OutputCanvasText("List end");
    }
    /// < summary >
    /// Download file
    /// </ summary >
    public void Download()
    {
        try
        {
            Download(_driveService);
        }
        catch (Exception e)
        {
            OutputCanvasText(e.Message);
        }
    }
    /// < summary >
    /// Download file
    /// </ summary >
    /// < param name = "service" > Google Drive service </ param > 
    private void Download(DriveService service)
    {
        OutputCanvasText("Download start");
        // get metadata
        var file = service.Files.Get(DOWNLOAD_FILE_ID).Execute();
        if (file == null)
        {
            OutputCanvasText("file is null");
            return;
        }
        OutputCanvasText("Name: " + file.Name + " ID: " + file.Id);
        // download
        var request = service.Files.Get(DOWNLOAD_FILE_ID);
        var fileStream = new FileStream(Path.Combine(SAVE_PATH, file.Name), FileMode.Create, FileAccess.Write);
        request.Download(fileStream);
        fileStream.Close();
        OutputCanvasText("Download end");
    }
    /// < summary >
    /// File upload
    /// </ summary >
    public void Upload()
    {
        try
        {
            // Create metadata for the file to be uploaded
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(FILE_PATH),
                Parents = new[] { GOOGLE_DRIVE_FOLDER_ID },
            };
            Upload(_driveService, fileMetadata);
        }
        catch (Exception e)
        {
            OutputCanvasText(e.Message);
        }
    }
    /// < summary >
    /// File upload
    /// </ summary >
    /// < param name = "service" > Google Drive service </ param > 
    /// < param name = "fileMetadata" > Metadata </ param > 
    /// < exception cref = "Exception" ></ exception > 
    private void Upload(DriveService service, Google.Apis.Drive.v3.Data.File fileMetadata)
    {
        OutputCanvasText("Upload start");
        var request = service.Files.Create(fileMetadata, new FileStream(FILE_PATH, FileMode.Open), "text/plain");
        request.Fields = "name, id";
        var uploadProgress = request.Upload();
        if (uploadProgress.Status != UploadStatus.Completed)
        {
            OutputCanvasText("Upload failed:" + uploadProgress.Status);
            return;
        }
        // Get the name and ID of the uploaded file
        var file = request.ResponseBody;
        OutputCanvasText("Name:" + file.Name + " ID:" + file.Id);
        OutputCanvasText("Upload end");
    }
    /// < summary >
    /// Append to the text on the canvas
    /// </ summary >
    /// < param name = "text" > Text </ param > 
    private void OutputCanvasText(string text)
    {
        statusText = text + " \n ";
    }

    public void DownFolder()
    {
        GetFileAmount(GOOGLE_DRIVE_FOLDER_ID, savePath);
        DownloadAllFolder(GOOGLE_DRIVE_FOLDER_ID, savePath);
    }

    public void DownloadAllFolder(string folderId, string currentPath)
    {
        var files = GetFiles(folderId);

        if (files == null)
        {
            isDownloading = false;
            return;
        }
        foreach (var fileOrFolder in files)
        {
            if (!fileOrFolder.Name.Contains("SEI"))
            {
                var subFolderPath = Path.Combine(currentPath, fileOrFolder.Name);
                Directory.CreateDirectory(subFolderPath);
                DownloadAllFolder(fileOrFolder.Id, subFolderPath);
                Debug.Log("[Folder] " + fileOrFolder.Name);
            }
            else
            {
                //var subFolderPath = Path.Combine(savePath, fileOrFolder.Name);
                DownloadFile(fileOrFolder.Id, currentPath);
            }
        }
    }
    public List<Google.Apis.Drive.v3.Data.File> GetFiles(string folderId)
    {
        var request = _driveService.Files.List();
        request.Q = $"'{folderId}' in parents";
        request.Fields = "nextPageToken, files(id, name, size, createdTime)";
        var files = new List<Google.Apis.Drive.v3.Data.File>();
        do
        {
            var result = request.Execute();
            files.AddRange(result.Files);
            request.PageToken = result.NextPageToken;
        } while (!string.IsNullOrEmpty(request.PageToken));
        return files;
    }
    private void DownloadFile(string fileId, string savePath1)
    {
        OutputCanvasText("Download start");
        // get metadata
        var file = _driveService.Files.Get(fileId).Execute();
        if (file == null)
        {
            OutputCanvasText("file is null");
            return;
        }
        OutputCanvasText("Name: " + file.Name + " ID: " + file.Id);
        // download
        var request = _driveService.Files.Get(fileId);
        var fileStream = new FileStream(Path.Combine(savePath1, file.Name), FileMode.Create, FileAccess.Write);
        request.Download(fileStream);
        fileStream.Close();
        downloadProgress += 1;
        OutputCanvasText("Download end");
    }

    private void GetFileAmount(string folderId, string currentPath)
    {
        var files = GetFiles(folderId);

        if (files == null)
        {
            isDownloading = false;
            return;
        }
        foreach (var fileOrFolder in files)
        {
            if (!fileOrFolder.Name.Contains("SEI"))
            {
                GetFileAmount(fileOrFolder.Id, currentPath);   
            }
            else
            {
                totalFile += 1;
            }
        }
    }
}