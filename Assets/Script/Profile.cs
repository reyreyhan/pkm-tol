using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Profile : MonoBehaviour
{

    public RawImage image;
    [SerializeField] InputField email;
    [SerializeField] InputField name;
    [SerializeField] InputField address;
    [SerializeField] InputField phone;
    [SerializeField] Text balance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetProf()
    {
        StartCoroutine(GetProfile());
    }

    public void PostProf()
    {
        StartCoroutine(GetProfile());
    }

    IEnumerator GetProfile()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://pkm-tol.herokuapp.com/public/api/profile/" + Session.GetInstance().Id);
        www.SendWebRequest();
        //Debug.Log(www.SendWebRequest());

        while (www.isDone == false)
        {
            yield return null;
        }
        //Nunggu Download Selesai
        while (!www.downloadHandler.isDone)
        {
            yield return null;
        }

        //Cek Error Status Web Server
        if (www.error == null)
        {
            SQL sql = JsonUtility.FromJson<SQL>(www.downloadHandler.text);
            email.text = sql.email;
            name.text = sql.name;
            address.text = sql.address;
            phone.text = sql.phone;
            balance.text = sql.balance;
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator PostProfile()
    {
        yield return null;
    }

    [System.Serializable]
    public class SQL
    {
        public string email;
        public string name;
        public string address;
        public string phone;
        public string balance;
        public string create_at;
        public string update_at;
    }
}
