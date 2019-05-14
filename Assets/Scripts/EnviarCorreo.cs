using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnviarCorreo : MonoBehaviour
{
    
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser user;
    private bool signedIn;
    private string displayName;



    public InputField inputFieldEmail;

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

    public void RestaurarEmail()
    {
        string email = inputFieldEmail.text;

        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {

            Debug.LogFormat("Correo Enviado");

            SceneManager.LoadScene(1);


        });
    }
}
