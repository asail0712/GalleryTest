using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.InteropServices;

//#if UNITY_IOS
using UnityEngine.iOS;
//#endif

public class GalleryInstaller : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _RequestPhotoLibraryAuthorization();

    [DllImport("__Internal")]
    private static extern void _SaveImageToPhotoLibrary(string imagePath);

    public void RequestPhotoLibraryAuthorization()
    {
        _RequestPhotoLibraryAuthorization();
    }

    public void SaveImageToPhotoLibrary(string imagePath)
    {
        _SaveImageToPhotoLibrary(imagePath);
    }

    // Start is called before the first frame update
    void Start()
    {
        List<string> allImgsPath = CollectAllImgsPath();

        //PHPhotoLibrary.RequestAuthorization(OnAuthorizationCallback);
    }


    private List<string> CollectAllImgsPath()
    {
#if UNITY_IOS

#elif UNITY_ANDROID

#else

#endif
        return new List<string>();
    }
}
