using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

public class Sample : MonoBehaviour
{
    delegate void callback_delegate(int val);

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void sampleMethod4(callback_delegate callback);
#endif //UNITY_IOS && !UNITY_EDITOR

    //回调函数，必须MonoPInvokeCallback并且是static
    [MonoPInvokeCallback(typeof(callback_delegate))]
    private static void cs_callback(int val)
    {
        Debug.Log("cs_callback : " + val);
    }

    private static void sampleMethod4Invoker()
    {
#if UNITY_IOS && !UNITY_EDITOR

        //直接把函数传过去
        sampleMethod4(cs_callback);
#endif //UNITY_IOS && !UNITY_EDITOR

    }

    private void Update()
    {
        sampleMethod4Invoker();
    }
}