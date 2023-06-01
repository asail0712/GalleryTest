using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// 可以參考的做法
// https://www.cnblogs.com/Jason-c/p/14303306.html

public class PhotoFrame : MonoBehaviour
{
    [SerializeField]
    private RawImage photoImg;

    private Texture2D thumbnailImgTex;

    public void SetImgInfo(string imagePath, int imgWidth, int imgHeight)
    {
        /************************************************
         * 讀取圖片到Texture裡面
         ************************************************/
        Texture2D sourceImgTex  = new Texture2D(2, 2);
        byte[] imageData        = File.ReadAllBytes(imagePath);
        sourceImgTex.LoadImage(imageData);

        //thumbnailImgTex = new Texture2D(imgWidth, imgHeight, sourceImgTex.format, false);
        //Graphics.ConvertTexture(sourceImgTex, thumbnailImgTex);

        photoImg.texture        = sourceImgTex;

        /************************************************
        * Image Scale處理
        ************************************************/

        int sourceImgWidth  = sourceImgTex.width;
        int sourceImgHeight = sourceImgTex.height;
        Vector3 scaleRatio  = Vector3.one;

        if(sourceImgWidth > sourceImgHeight)
        {
            scaleRatio.y = (float)sourceImgHeight / (float)sourceImgWidth;
        }
        else 
        {
            scaleRatio.x = (float)sourceImgWidth / (float)sourceImgHeight;
        }

        photoImg.GetComponent<RectTransform>().localScale = scaleRatio;
    }

    //Texture2D CutOrCopyTexture(Texture2D source, RectInt cutScope, RectInt targetScope)
    //{
    //    Texture2D target = new Texture2D(targetScope.width, targetScope.height, source.format, false);
    //    Graphics.CopyTexture(source, 0, 0, cutScope.x, cutScope.y, cutScope.width, cutScope.height, target, 0, 0, 0, 0);
    //    return target;
    //}

    private void OnDestroy()
    {
        if (thumbnailImgTex)
        {
            Destroy(thumbnailImgTex);
        }
    }
}

