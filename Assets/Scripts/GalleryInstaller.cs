using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

using TMPro;

public class GalleryInstaller : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI showInfo;

    [SerializeField]
    public GameObject ThubnailPhotoPrefab;

    [SerializeField]
    public GameObject ScrollViewContent;

    public static readonly int MAX_PHOTO_NUM        = 30;
#if UNITY_ANDROID && !UNITY_EDITOR
    public static readonly string PHOTO_PATH        = "/storage/emulated/0/DCIM/";
#elif UNITY_EDITOR
    public static readonly string PHOTO_PATH        = "C:/Users/Ed-GranDen/Downloads/";
#endif
    public static readonly string FILE_EXTENSION    = "*.jpg";

    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            PermissionCallbacks callbacks = new PermissionCallbacks();
            callbacks.PermissionDenied                  += PermissionCallbacks_PermissionDenied;
            callbacks.PermissionGranted                 += PermissionCallbacks_PermissionGranted;
            callbacks.PermissionDeniedAndDontAskAgain   += PermissionCallbacks_PermissionDeniedAndDontAskAgain;

            // 請求外部存儲讀取權限
            Permission.RequestUserPermission(Permission.ExternalStorageRead, callbacks);

            showInfo.text = "Request User Permission !!";
            Debug.Log("Request User Permission");
        }
        else
        {
            showInfo.text = "Already Permission Granted !!";
            // 已經獲得權限，可以執行相應的操作
            Debug.Log("Already Permission Granted");

            PermissionCallbacks_PermissionGranted("");
        }
    }

    private void PermissionCallbacks_PermissionDenied(string permission)
    {
        showInfo.text = "Permission Denied !!";
        Debug.Log("Permission Denied");
    }

    private void PermissionCallbacks_PermissionGranted(string permission)
    {
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            LoadAndDisplayImages();

            Debug.Log("ExternalStorage Read Work");
        }
        else
        {
            showInfo.text = "No permission to access external storage. !!";
            Debug.Log("No permission to access external storage");
        }
    }

    private void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permission)
    {
        showInfo.text = "Permission Denied And Dont Ask Again !!";
        Debug.Log("Permission Denied And Dont Ask Again");
    }

    private void LoadAndDisplayImages()
    {
        if (ScrollViewContent == null)
        {
            Debug.LogError("ScrollViewContent is null");

            return;
        }

        DestroyAllChildren(ScrollViewContent);

        GridLayoutGroup gridLayoutGroup = ScrollViewContent.GetComponent<GridLayoutGroup>();
        int imgWidth                    = (int)gridLayoutGroup.cellSize.x;
        int imgHeight                   = (int)gridLayoutGroup.cellSize.y;

        // 蒐集檔案路徑
        string[] photoFiles = Directory.GetFiles(PHOTO_PATH, FILE_EXTENSION, SearchOption.AllDirectories);

        if(photoFiles.Length > MAX_PHOTO_NUM)
        {
            Array.Resize(ref photoFiles, MAX_PHOTO_NUM);
        }

        // 依照時間由近到遠排序
        Array.Sort(photoFiles, (a, b) => File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)));

        foreach (string path in photoFiles)
        {
            GameObject ThubnailPhoto    = Instantiate(ThubnailPhotoPrefab);

            if (ThubnailPhoto == null)
            {
                continue;
            }
             
            PhotoFrame photoFrame = ThubnailPhoto.GetComponent<PhotoFrame>();

            if (photoFrame == null)
            {
                continue;
            }

            photoFrame.SetImgInfo(path, imgWidth, imgHeight);
            photoFrame.transform.SetParent(ScrollViewContent.transform);
            photoFrame.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    private void OnApplicationFocus(bool bHasFocus)
    {
        if(bHasFocus)
        {
            PermissionCallbacks_PermissionGranted("");
        }
    }

    private void DestroyAllChildren(GameObject targetObj)
    {
        // 獲取父物件的 Transform
        Transform parentTransform = targetObj.transform;

        // 遍歷所有子物件
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            // 獲取子物件的 Transform
            Transform childTransform = parentTransform.GetChild(i);

            // 刪除子物件
            Destroy(childTransform.gameObject);
        }
    }
}
