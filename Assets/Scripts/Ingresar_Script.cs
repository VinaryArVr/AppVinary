using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ingresar_Script : MonoBehaviour
{
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser user;
    private string displayName;

    [SerializeField] private InputField InputField_Correo = null;
    [SerializeField] private InputField inputField_Contraseña = null;



    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
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
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                displayName = user.DisplayName ?? "";
                Debug.Log("Signed in " + user.UserId);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     private Boolean validarDatos()
    {
        string correo = InputField_Correo.text;
        string contraseña = inputField_Contraseña.text;

        if (correo != "" && contraseña != "")
        {
            Debug.Log("Campos llenos");
            return true;
        }
        else
        {
            Debug.Log("Llene los campos");
            return false;
        }
    }
    public void LoginUser() {

        Debug.Log("Ingreso!");

          string correo = InputField_Correo.text;
            string contraseña = inputField_Contraseña.text;

        Boolean datosValidos = validarDatos();
        if (datosValidos)
        {
            auth.SignInWithEmailAndPasswordAsync(correo, contraseña).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("Acceso con correo y contraseña no es valido!");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Error: " + task.Exception);
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                SceneManager.LoadSceneAsync("5_Home");
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);


            });
        }
           



    }

}
