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
    public Text inputText;
    public RawImage image;

    private string strEmail = "";
    private string strPassword = "";
    private string logText = "";

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    const int kMaxLogSize = 16382;

    private bool fetchingToken = false;


    public void Start()
    {   inputText.gameObject.SetActive(false);
        image.gameObject.SetActive(false);

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
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();


        strEmail = email.GetComponent<InputField>().text;
        strPassword = password.GetComponent<InputField>().text;


    }

    
    public void SigninAsync()
    {
        inputText.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        DebugLog(String.Format("Intento de inicio de sesión con la cuenta {0}...", strEmail));
                  auth.SignInWithEmailAndPasswordAsync(strEmail, strPassword)
      .ContinueWith(HandleSigninResult);


    }

    private void HandleSigninResult(Task<Firebase.Auth.FirebaseUser> authTask)
    {
        Boolean complete = LogTaskCompletion(authTask, "Inicio de sesión");
        if (complete)
        {
            SceneManager.LoadSceneAsync("5_Home");
        }
    }

    private bool LogTaskCompletion(Task task, string operation)
    {
        
        bool complete = false;
        if (task.IsCanceled)
        {
            DebugLog(operation + " cancelado.");
            inputText.text = " cancelado.";
        }
        else if (task.IsFaulted)
        {
            
            DebugLog(operation + " erróneo.");
            inputText.text = " erróneo.";
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                int codigoErrorAutenticacion;
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;

                if (firebaseEx != null)
                {
                    
                    codigoErrorAutenticacion = firebaseEx.ErrorCode;
                   
                    DesplegarErrorAutenticacion(codigoErrorAutenticacion);
                    showToast(inputText.text,image, 2);
                    authErrorCode = String.Format("Error de autenticación.{0}: ",
                    codigoErrorAutenticacion.ToString());
                    
                }
                DebugLog(authErrorCode + exception.ToString());
                //inputText.text = authErrorCode + exception.ToString();
            }
           
        }
        else if (task.IsCompleted)
        {
            DebugLog(operation + " completado");
            inputText.text = " completado";
            complete = true;
        }
        return complete;
    }

    private void DesplegarErrorAutenticacion(int error)
    {
        switch (error)
        {
            case -1:
                inputText.text = "No implementado";
                break;
            case 0:
                inputText.text = "Ninguno";
                break;
            case 1:
                inputText.text = "Fallo";
                break;
            case 2:
                inputText.text = "Token personalizado no válido";
                break;
            case 3:
                inputText.text = "Conflicto de token personalizado";
                break;
            case 4:
                inputText.text = "Credencial inválida";
                break;
            case 5:
                inputText.text = "Usuario deshabilitado";
                break;
            case 6:
                inputText.text = "Cuenta existe con diferentes credenciales";
                break;
            case 7:
                inputText.text = "Operación no permitida";
                break;
            case 8:
                inputText.text = "Correo electrónico ya en uso";
                break;
            case 9:
                inputText.text = "Requiere inicio de sesión reciente";
                break;
            case 10:
                inputText.text = "Credencial ya en uso";
                break;
            case 11:
                inputText.text = "Correo electrónico inválido";
                break;
            case 12:
                inputText.text = "Contraseña incorrecta";
                break;
            case 13:
                inputText.text = "Demasiadas solicitudes";
                break;
            case 14:
                inputText.text = "Usuario no encontrado";
                break;
            case 15:
                inputText.text = "Proveedor ya vinculado";
                break;
            case 16:
                inputText.text = "No hay tal proveedor";
                break;
            case 17:
                inputText.text = "Token de usuario no válido";
                break;
            case 18:
                inputText.text = "Token de usuario caducado";
                break;
            case 19:
                inputText.text = "Error en la solicitud de red";
                break;
            case 20:
                inputText.text = "Clave de API no válida";
                break;
            case 21:
                inputText.text = "Aplicación no autorizada";
                break;
            case 22:
                inputText.text = "Falta de coincidencia del usuario";
                break;
            case 23:
                inputText.text = "Contraseña débil";
                break;
            case 24:
                inputText.text = "Usuario no registrado";
                break;
            case 25:
                inputText.text = "Api no disponible";
                break;
            case 26:
                inputText.text = "Código de Acción Vencido";
                break;
            case 27:
                inputText.text = "Código de acción no válido";
                break;
            case 28:
                inputText.text = "Carga de mensaje no válida";
                break;
            case 29:
                inputText.text = "Número de teléfono inválido";
                break;
            case 30:
                inputText.text = "Número de teléfono faltante";
                break;
            case 31:
                inputText.text = "Correo electrónico del destinatario no válido";
                break;
            case 32:
                inputText.text = "Remitente inválido";
                break;
            case 33:
                inputText.text = "Código de verificación no válido";
                break;
            case 34:
                inputText.text = "ID de verificación no válido";
                break;
            case 35:
                inputText.text = "Falta el código de verificación";
                break;
            case 36:
                inputText.text = "Identificación de verificación faltante";
                break;
            case 37:
                inputText.text = "Correo electrónico faltante";
                break;
            case 38:
                inputText.text = "Falta la contraseña";
                break;
            case 39:
                inputText.text = "Cuota excedida";
                break;
            case 40:
                inputText.text = "Reintentar Teléfono Aut.";
                break;
            case 41:
                inputText.text = "Sesión expirada";
                break;
            case 42:
                inputText.text = "Aplicación no verificada";
                break;
            case 43:
                inputText.text = "Error en la verificación de la aplicación";
                break;
            case 44:
                inputText.text = "Error en el chequeo de Captcha";
                break;
            case 45:
                inputText.text = "Credencial de aplicación no válida";
                break;
            case 46:
                inputText.text = "Falta la credencial de la aplicación";
                break;
            case 47:
                inputText.text = "ID de cliente inválido";
                break;
            case 48:
                inputText.text = "URL de continuación inválido";
                break;
            case 49:
                inputText.text = "Falta URL de continuación";
                break;
            case 50:
                inputText.text = "Error de llavero";
                break;
            case 51:
                inputText.text = "Falta el token de la aplicación";
                break;
            case 52:
                inputText.text = "Falta el ID del paquete de Ios";
                break;
            case 53:
                inputText.text = "Notificación no reenviada";
                break;
            case 54:
                inputText.text = "Dominio no autorizado";
                break;
            case 55:
                inputText.text = "Contexto web ya presentado";
                break;
            case 56:
                inputText.text = "Contexto web cancelado";
                break;
            case 57:
                inputText.text = "Enlace Dinámico No Activado";
                break;
            case 58:
                inputText.text = "Cancelado";
                break;
            case 59:
                inputText.text = "ID de proveedor no válido";
                break;
            case 60:
                inputText.text = "Error interno de la web";
                break;
            case 61:
                inputText.text = "Sitio web no admitido";
                break;
            case 62:
                inputText.text = "Discrepancia de identificación del inquilino";
                break;
            case 63:
                inputText.text = "Operación del inquilino no admitida";
                break;
            case 64:
                inputText.text = "Dominio de enlace no válido";
                break;
            case 65:
                inputText.text = "Credencial rechazada";
                break;
            case 66:
                inputText.text = "Número de teléfono no encontrado";
                break;
            case 67:
                inputText.text = "Identificación de inquilino inválida";
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }
    }

    public void SignOut()
    {
        DebugLog("Signing out.");
        auth.SignOut();
    }

    void showToast(string text, RawImage image,
    int duration)
    {
        StartCoroutine(showToastCOR(text,image, duration));
    }

    private IEnumerator showToastCOR(string text,RawImage image,
        int duration)
    {
        Color orginalColor = inputText.color;
        Color originalColor = image.color;

        inputText.text = text;
        inputText.enabled = true;
        

        image.enabled = true;


        //Fade in
        yield return fadeInAndOutImage(image, true, 0.5f);
        yield return fadeInAndOut(inputText, true, 0.5f);
       

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOutImage(image, false, 0.4f);
        yield return fadeInAndOut(inputText, false, 0.5f);

        inputText.enabled = false;
        inputText.color = orginalColor;

        image.enabled = false;
    }

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
    IEnumerator fadeInAndOutImage(RawImage image, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = new Color(1f,1f,1f,0f);
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }


}
