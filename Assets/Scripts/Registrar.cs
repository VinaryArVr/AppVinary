using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registrar : MonoBehaviour
{


    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser user;
    private string displayName;

    public InputField Email1;
    public InputField Password;

    private bool signedIn;


    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
                //  emailAddress = user.Email ?? "";
                //  photoUrl = user.PhotoUrl ?? "";
            }
        }
    }

    public void CreateUserByEmail()
    {

        string email = Email1.text;
        string password = Password.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {

            Debug.LogFormat("Usuario Creado");

            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }




   

  

 

   

}
