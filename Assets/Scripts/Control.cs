using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechninierException;
using UnityEngine;

public class Control : MonoBehaviour
{

    private Rigidbody2D rb;
    public float movespeed = 3;
    public System.Threading.SynchronizationContext SynchContext;


    // Use this for initialization
    void Start()
    {
        // try
        //{

        AndroidJavaClass mainActivityClass222 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = mainActivityClass222.GetStatic<AndroidJavaObject>("currentActivity");



        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass techninierUtil = new AndroidJavaClass("com.techninier.telcomanager.TechninierUtils");
        AndroidJavaObject instance = techninierUtil.CallStatic<AndroidJavaObject>("instance");
        instance.Call("setContext", context);
        // Debug.Log("DualSimChecking=" + instance.Call<string>("DualSimChecking"));

        using (instance)
        {
            instance.Call("DoGet", "http://www.googles.com");
            try
            {
                Debug.Log("DualSimChecking=" + instance.Call<string>("DualSimChecking"));
            }
            catch (Exception e)
            {
                string[] errorMsg = e.Source.Split('|');
                Debug.Log("Source =" + e.Source);
                Debug.Log("StackTrace =" + e.StackTrace);
                Debug.Log("Message =" + e.Message);

                Debug.Log("ErrorCode =" + errorMsg[0]);
                Debug.Log("ErrorMsg =" + errorMsg[1]);
            }
        }
        SynchContext = UnitySynchronizationContext.Current;
        test().ContinueWith(t =>
        {
        });

        //}
        //catch (Exception e)
        //{

        //    Debug.Log("%%%%% Error exception called");
        //    ExceptionUtils.Instance.LogPrintErrorMessage(e, SynchContext);
        //}

    }


    public void UtilsErrorResponse(string message)
    {
        Debug.Log("message from java: " + message);
    }

    public void onResponse(string message)
    {
        Debug.Log("onResponse message from java: " + message);
    }



    async Task test()
    {
        await TaskEx.Run(() =>
        {

            try
            {
                Debug.Log("hello world");

                //   AndroidJavaClass mainActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                //   AndroidJavaObject activity = mainActivityClass.GetStatic<AndroidJavaObject>("currentActivity");

                //       AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

                //       AndroidJavaClass techninierUtil = new AndroidJavaClass("com.techninier.telcomanager.TechninierUtils");
                //    AndroidJavaObject instance = techninierUtil.Call<AndroidJavaObject>("instance");
                //    instance.Call("setContext", context);
                //    Debug.Log("Carrier Name=" + instance.Call<string>("AccessCarrierName"));

            }
            catch (Exception e)
            {

                ExceptionUtils.Instance.LogPrintErrorMessage(e, SynchContext);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            rb.velocity = new Vector2(-movespeed, rb.velocity.y);

        if (Input.GetKey(KeyCode.RightArrow))
            rb.velocity = new Vector2(movespeed, rb.velocity.y);
    }
}