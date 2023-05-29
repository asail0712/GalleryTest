using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

using TMPro;

public class PhotoLibraryController : MonoBehaviour
{
    public TMP_InputField aa;

 #if UNITY_IOS && !UNITY_EDITOR
    // 声明插件函数
    [DllImport("__Internal")]
    private static extern void GetPhotosThumbnails();
#endif

    // 调用插件函数
    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        GetPhotosThumbnails();
#endif
    }

    // 在原生代码中调用此函数以接收缩略图
    public void ReceiveThumbnail(string thumbnailData)
    {
        // 在此处理收到的缩略图数据

        aa.text = "XDDDD  " + thumbnailData;
    }
}