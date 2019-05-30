using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;

public class CargaDatos : MonoBehaviour
{
    DatabaseReference reference;
    public RawImage image;

    const int kMaxLogSize = 16382;
    private string logText = "";
    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vinaryapp-7c2e3.firebaseio.com/");

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

      
       

        

    }
    void InitializeFirebase()
    {
        DebugLog("Setting up Firebase Auth");
    }
    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mostrar() {

       reference.Child("Archivos").OrderByChild("link").LimitToLast(10).GetValueAsync().ContinueWith(x => {
       if (x.Result == null)
       {
           Debug.Log("null!");
       }
       else if (!x.Result.HasChildren)
       {
           Debug.Log("no children!");
       }
       else
       {
           foreach (var child in x.Result.Children)
           {
                    Debug.Log(child.Value.ToString());
                   StartCoroutine(LoadImage(child.Value.ToString()));
                   
           }
       }
   });



    }

    public IEnumerator LoadImage(string dir)
    {
        WWW www = new WWW(dir);
        yield return www;
        Texture2D te = www.texture;
        image.texture = te;


    }

}
