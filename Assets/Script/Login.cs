using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    [SerializeField] InputField email;
    [SerializeField] InputField password;
    [SerializeField] Text exeption;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Get()
    {
        StartCoroutine(ProsesGet());
    }

    IEnumerator ProsesGet()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("password", password.text);

        UnityWebRequest www = UnityWebRequest.Post("http://pkm-tol.herokuapp.com/public/api/login", form);
        www.SendWebRequest();
        Debug.Log(www);

        //Nunggu Prosses Upload Web Request Selesai
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
            if(www.downloadHandler.text == "Email & Password Not Found")
            {
                yield return www.downloadHandler.text;
                exeption.text  = www.downloadHandler.text;
            }
            else
            {
                string json = www.downloadHandler.text;
                ID myiD = new ID();
                myiD = JsonUtility.FromJson<ID>(json);
                Debug.Log(myiD.id);

                Session.GetInstance().Id = myiD.id;
                PlayerPrefs.SetString("ID", myiD.id);

                SceneManager.LoadScene("Beacon_Detection");
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    class ID
    {
        public string id;
    }
}
