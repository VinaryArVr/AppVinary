using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class url : MonoBehaviour{

    public string web1;
    public string web2;
    public string web3;
    public string web4;
    private string numero_capitalika;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Facebook()
    {
        Application.OpenURL(web1);
    }
    public void SitioWeb()
    {
        Application.OpenURL(web2);
    }
    public void Linkedin()
    {
        Application.OpenURL(web3);
    }
    public void Instagram()
    {
        Application.OpenURL(web4);
    }
    public void numerocapitalika()
    {
        Application.OpenURL("tel://+593962786519".Replace("#", "%23"));

    }
}
