using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session
{
    private string id;
    private string masuk;
    private string keluar;
    private string price;

    private static Session instance = new Session();

    public string Id { get => id; set => id = value; }
    public string Masuk { get => masuk; set => masuk = value; }
    public string Keluar { get => keluar; set => keluar = value; }
    public string Price { get => price; set => price = value; }

    private Session()
    {
        
    }
    
    public static Session GetInstance()
    {
        return instance;
    }
}
