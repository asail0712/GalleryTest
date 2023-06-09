using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

using TMPro;

public class PhotoLibraryController : MonoBehaviour
{
    public TextMeshProUGUI picInfo;
    public TextMeshProUGUI isoInfo;

#if UNITY_IOS && !UNITY_EDITOR
    // 声明插件函数
    [DllImport("__Internal")]
    private static extern void CheckAuthorizationStatus();
#endif

    // 调用插件函数
    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        CheckAuthorizationStatus();
#endif
    }


    public void ReceiveISOInfo(string infoStr)
    {
        isoInfo.text = "ISO Info  " + infoStr;
    }

	public void ReceivePicInfo(string picInfoStr)
	{
        picInfo.text = "Pic Info  " + picInfoStr;
	}
}