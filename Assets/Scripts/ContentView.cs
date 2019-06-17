using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ContentView : MonoBehaviour
{
    public RawImage Image;
    DatabaseReference reference;
    
    const int kMaxLogSize = 16382;
    private string logText = "";

    long cantidad;



    List<Texture> equipos = new List<Texture>();
    List<string> enlaces = new List<string>();

    public Transform ContentPanel;
    public SimpleObjectPool buttonObjectPool;


    private void Awake()
    {
        Image = this.gameObject.GetComponent<RawImage>();
      
    }

    // Start is called before the first frame update
   IEnumerator Start()
    {
       
        
        reference = FirebaseDatabase.DefaultInstance.GetReference("Equipos");


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vinaryapp-7c2e3.firebaseio.com/");
       
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
                equipos.Clear();
                enlaces.Clear();

                reference.GetValueAsync().ContinueWith(x => {
                    if (x.Result == null)
                    {
                        Debug.Log("null!");
                    }
                    else if (!x.Result.HasChildren)
                    {
                        Debug.Log("no children!");
                    }
                    else if (x.IsCompleted)
                    {
                        DataSnapshot snapshot = x.Result;
                        cantidad = snapshot.ChildrenCount;
                        DebugLog("Numero de objetos en la base: "+ cantidad.ToString());
                        for (int j = 1; j <= cantidad; j++)
                        {                         
                            string valor = j.ToString();
                           
                                reference.Child(valor).Child("Enlace").GetValueAsync().ContinueWith(y => {
                                if (y.Result == null)
                                {
                                    Debug.Log("null!");
                                }
                                else if (!y.Result.HasChildren)
                                {
                                    Debug.Log("no children!");
                                }
                                else if (y.IsCompleted)
                                {
                                    foreach (DataSnapshot child in y.Result.Children)
                                    {

                                        Debug.Log(child.Value.ToString());

                                        enlaces.Add(child.Value.ToString());

                                        Debug.Log(enlaces.Count.ToString());

                                    }


                                       
                                }

                                    
                            });
                           
                        }
                    }

                });
               
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        
        yield return new WaitForSeconds(1f);

        foreach (var item in enlaces) {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(item);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                

                // Show results as text
                Debug.Log("dato" + www.downloadHandler.text);
                Texture2D myTexture = DownloadHandlerTexture.GetContent(www);
                Image.texture = myTexture;
                equipos.Add(Image.texture);
                Debug.Log("Numero " + equipos.Count.ToString());
            }


        }

        Refresh();
    }

    private void Refresh()
    {

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

}
