using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Url : MonoBehaviour{

    public string Enfermeria;
    public string Medicina;
    public string Odontologia;
    public string Administracion;
    public string Gastronomia;
    public string Turismo;
    public string Derecho;
    public string Contabilidad;
    public string Software;
    public string Facebook;
    public string Instagram;
    public string Web;
    public string Twiter;
    public string Youtube;
    public string Google;
    public string Inscripciones;
    public string Matricula;
    public string Cronograma;
    public string Linkedin;
    private string numerocapitalika;
    private string numeroprops;
    private string numeroUta;
    private string numerohabiqo;
    private string numeroEcuadorTv;
    // Start is called before the first frame update

    public void EnfermeriaURL()
    {
        Application.OpenURL(Enfermeria);
    }
    public void MedicinaURL()
    {
        Application.OpenURL(Medicina);
    }
    public void OdontologiaURL()
    {
        Application.OpenURL(Odontologia);
    }
    public void AdministracionURL()
    {
        Application.OpenURL(Administracion);
    }
    public void GastronomiaURL()
    {
        Application.OpenURL(Gastronomia);
    }
    public void TurismoURL()
    {
        Application.OpenURL(Turismo);
    }
    public void DerechoURL()
    {
        Application.OpenURL(Derecho);
    }
    public void ContabilidadURL()
    {
        Application.OpenURL(Contabilidad);
    }
    public void SoftwareURL()
    {
        Application.OpenURL(Software);
    }
    public void FacebookURL()
    {
        Application.OpenURL(Facebook);
    }
    public void InstagramURL()
    {
        Application.OpenURL(Instagram);
    }
    public void WebURL()
    {
        Application.OpenURL(Web);
    }
    public void TwiterURL()
    {
        Application.OpenURL(Twiter);
    }
    public void YoutubeURL()
    {
        Application.OpenURL(Youtube);
    }
    public void GoogleURL()
    {
        Application.OpenURL(Google);
    }
    public void InscripcionesURL()
    {
        Application.OpenURL(Inscripciones);
    }
    public void MatriculaURL()
    {
        Application.OpenURL(Matricula);
    }
    public void CronogramaURL()
    {
        Application.OpenURL(Cronograma);
    }
    public void LinkedinURL()
    {
        Application.OpenURL(Linkedin);
    }
     public void numeroCapitalika()
    {
        Application.OpenURL("tel://+593962786519".Replace("#", "%23"));

    }
    public void numeroProps()
    {
        Application.OpenURL("tel://+593983359738".Replace("#", "%23"));

    }
    public void numeroUTA()
    {
        Application.OpenURL("tel://033700090".Replace("#", "%23"));

    }
    public void numeroHabiqo()
    {
        Application.OpenURL("tel://+593998724517".Replace("#", "%23"));

    }
    public void numeroECTV()
    {
        Application.OpenURL("tel://023970800".Replace("#", "%23"));

    }
}
