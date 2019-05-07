using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser user;
    private string displayName;

    public GameObject email;
    public GameObject password;
    public GameObject ingresar;
    public GameObject registrarse;

    public Button btnIngresar;

    private string strEmail = "";
    private string strPassword = "";
    private string logText = "";

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    const int kMaxLogSize = 16382;

    private bool fetchingToken = false;

    public void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
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
    // Output text to the debug log text field, as well as the console.
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

    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
              task => DebugLog(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        DebugLog("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        auth.IdTokenChanged += IdTokenChanged;
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
        strEmail = email.GetComponent<InputField>().text;
        strPassword = password.GetComponent<InputField>().text;

        btnIngresar = ingresar.GetComponent<Button>();
        btnIngresar.onClick.AddListener(LoginUser);

    }

    private Boolean validarDatos()
    {
        if (strEmail != "" && strPassword != "")
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
    private void LoginUser()
    {
        Debug.Log("Ingreso!");

        string correo = strEmail;
        string contraseña = strPassword;

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
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);

                SceneManager.LoadScene("5_Home");


            });
        }     



    }
}
