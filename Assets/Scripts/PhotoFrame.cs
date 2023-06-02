using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

// Copy Scope Texture�i�H�ѦҪ����k
// https://www.cnblogs.com/Jason-c/p/14303306.html

// �ֳt���J�Ϥ����ѦҰ��k
// https://forum.unity.com/threads/texture2d-loadimage-too-slow-anyway-to-use-threading-to-speed-it-up.442622/

// Url�̭����Ů檺���k
// https://answers.unity.com/questions/225469/unity-not-passing-space-character-to-www-url.html
public class PhotoFrame : MonoBehaviour
{
    [SerializeField]
    private RawImage photoImg;

    private Texture2D thumbnailImgTex;

    static readonly int thumbnailSize = 100;


    public void SetImgInfoWithWWWAsync(string imagePath, int imgWidth, int imgHeight)
    {
        StartCoroutine(SetImgInfoWithWWWAsync_Internal(imagePath, imgWidth, imgHeight));
    }

    public IEnumerator SetImgInfoWithWWWAsync_Internal(string imagePath, int imgWidth, int imgHeight)
    {
        // �|�����|�W�٪����D
        imagePath = imagePath.Replace(" ", "%20");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + imagePath, false);

        yield return www.SendWebRequest();

        yield return new WaitUntil(() => www.isDone);

        Texture2D wwwTex = null;

        if (www.result != UnityWebRequest.Result.ProtocolError)
        {
            wwwTex    = ((DownloadHandlerTexture)www.downloadHandler).texture;
            
            //photoImg.texture = wwwTex;
            StartCoroutine(CreateThumbnailTexture(wwwTex, thumbnailSize, thumbnailSize, (value) => photoImg.texture = value));
        }
        else
        {
            Debug.LogError("Image Fail");
            yield return null;
        }

        /************************************************
        * Image Scale�B�z
        ************************************************/

        int sourceImgWidth  = wwwTex.width;
        int sourceImgHeight = wwwTex.height;
        Vector3 scaleRatio  = Vector3.one;

        if (sourceImgWidth > sourceImgHeight)
        {
            scaleRatio.y = (float)sourceImgHeight / (float)sourceImgWidth;
        }
        else
        {
            scaleRatio.x = (float)sourceImgWidth / (float)sourceImgHeight;
        }

        photoImg.GetComponent<RectTransform>().localScale = scaleRatio;
    }

    public void SetImgInfo(string imagePath, int imgWidth, int imgHeight)
    {
        /************************************************
         * Ū���Ϥ���Texture�̭�
         ************************************************/
       
        thumbnailImgTex     = new Texture2D(2, 2);
        byte[] imageData    = File.ReadAllBytes(imagePath);
        
        thumbnailImgTex.LoadImage(imageData);
        StartCoroutine(CreateThumbnailTexture(thumbnailImgTex, thumbnailSize, thumbnailSize, (value) => photoImg.texture = value));

        /************************************************
        * Image Scale�B�z
        ************************************************/
        int sourceImgWidth  = thumbnailImgTex.width;
        int sourceImgHeight = thumbnailImgTex.height;

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

    private void OnDestroy()
    {
        if (thumbnailImgTex)
        {
            Destroy(thumbnailImgTex);
        }
    }

    //private void CreateThumbnailTexture(Texture2D originalTexture, int thumbnailWidth, int thumbnailHeight, Action<Texture2D> taskCompletedCallBack)
    private IEnumerator CreateThumbnailTexture(Texture2D originalTexture, int thumbnailWidth, int thumbnailHeight, Action<Texture2D> taskCompletedCallBack)
    {
        // �s�@�Y���Ϫ� Texture2D
        Texture2D thumbnailTexture  = new Texture2D(thumbnailWidth, thumbnailHeight);
        Color[] thumbnailPixels     = new Color[thumbnailWidth * thumbnailHeight];

        float xRatio = (float)originalTexture.width / thumbnailWidth;
        float yRatio = (float)originalTexture.height / thumbnailHeight;

        for (int y = 0; y < thumbnailHeight; y++)
        {
            for (int x = 0; x < thumbnailWidth; x++)
            {
                int originalX = Mathf.FloorToInt(x * xRatio);
                int originalY = Mathf.FloorToInt(y * yRatio);

                Color originalPixel = originalTexture.GetPixel(originalX, originalY);                
                int thumbnailIndex  = y * thumbnailWidth + x;

                thumbnailPixels[thumbnailIndex] = originalPixel;
            }

            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(1.0f);
        }

        // �]�w�Y���Ϫ�������ƨ����Χ��
        thumbnailTexture.SetPixels(thumbnailPixels);
        thumbnailTexture.Apply();

        taskCompletedCallBack?.Invoke(thumbnailTexture);

        if (originalTexture)
        {
            Destroy(originalTexture);
        }

        yield return null;
    }
}

