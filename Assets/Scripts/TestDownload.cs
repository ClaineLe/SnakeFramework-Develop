using System;
using System.Collections;
//using Unity.Networking;
using UnityEngine;

public class TestDownload : MonoBehaviour
{
    public int WIDGET_WIDTH = 480;
    public int WIDGET_HEIGHT = 80;
    private string[] urls = new[]
    {
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_1.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_2.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_3.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_4.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_5.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_6.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_7.wnf",
        @"https://halowfantasy.oss-cn-shenzhen.aliyuncs.com/ext_res/ext_res_part_8.wnf",
    };





    private string error;
    private string content;

    private DownloadOperation[] downloads = new DownloadOperation[8];
    //private BackgroundDownload[] downloads = new BackgroundDownload[8];

    public static string path = string.Empty;

    private void Awake()
    {
        path = Application.persistentDataPath + "/BGDL/";
    }


    private void Start()
    {
    }


    private void OnGUI()
    {
        GUILayout.Space(WIDGET_HEIGHT);
        if (GUILayout.Button("StartDownload", GUILayout.Width(WIDGET_WIDTH), GUILayout.Height(WIDGET_HEIGHT)))
        {
            error = string.Empty;
            for (int i = 0; i < urls.Length; i++)
            {
                BackgroundDownloadOptions option = new BackgroundDownloadOptions(urls[i]);
                option.SetDestinationPath(path);
                downloads[i] = BackgroundDownloads.GetDownloadOperation(option.URL);
                if (downloads[i] != null)
                    downloads[i] = BackgroundDownloads.StartOrContinueDownload(option);
                else
                {

                    downloads[i] = BackgroundDownloads.StartDownload(option);
                }
            }
        }

        if (GUILayout.Button("ClearFold", GUILayout.Width(WIDGET_WIDTH), GUILayout.Height(WIDGET_HEIGHT)))
        {
            for (int i = 0; i < urls.Length; i++)
            {
                DownloadOperation operation = BackgroundDownloads.GetDownloadOperation(urls[i]);
                BackgroundDownloads.CancelDownload(operation);
            }
        }

        if (GUILayout.Button("ClearFold", GUILayout.Width(WIDGET_WIDTH), GUILayout.Height(WIDGET_HEIGHT))) 
            {
            string foldPath = path;
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(foldPath);
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
            }
        }

        using (new GUILayout.VerticalScope("Box"))
        {
            GUILayout.Label("flag5");

            if (string.IsNullOrEmpty(content) == false)
                GUILayout.Label("Content:\n" + content);

            if (string.IsNullOrEmpty(error) == false)
                GUILayout.Label("Error:\n" + error);

            using (new GUILayout.VerticalScope("Box"))
            {
                for (int i = 0; i < urls.Length; i++)
                {
                    if (downloads[i] != null)
                        GUILayout.Label("[" + (downloads[i].Progress * 100).ToString("0.0000") + "%]" + System.IO.Path.GetFileName(urls[i]) + ":" + downloads[i].Status);
                }
            }
        }
    }

}
