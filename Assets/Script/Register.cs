using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    [SerializeField] InputField[] inputField;
    [SerializeField] Text exeption;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SubmitData()
    {
        StartCoroutine(Submit());
    }

    IEnumerator Submit()
    {
        //Form
        WWWForm form = new WWWForm();

        form.AddField("name", inputField[0].text);
        form.AddField("email", inputField[1].text);
        form.AddField("password", inputField[2].text);
        form.AddField("password_confirmation", inputField[3].text);
        form.AddField("phone", inputField[4].text);

        UnityWebRequest www = UnityWebRequest.Post("http://pkm-tol.herokuapp.com/public/api/register", form);
        www.SendWebRequest();

        //Waiting Upload
        while (www.isDone == false)
        {
            Debug.Log("Menuggu Upload");
            yield return www;
            Debug.Log(www.ToString());
        }

        //Belum Selesai Download
        if (www.downloadHandler.isDone == false)
        {
            Debug.Log("Menunggu Download");
            yield return www;
            Debug.Log(www.downloadHandler.text);
            exeption.text = www.downloadHandler.text;
        }

        if (www.error == null)
        {
            if (www.downloadHandler.isDone)
            {
                //Query Selesai
                Debug.Log("Selesai");
                Debug.Log(www.downloadHandler.text);
            }

            //if (www.downloadHandler.text == "name")
            //{
            //    //Query Gagal
            //    Debug.Log(www.downloadHandler.text + "ISIEN CUK NAME NYA");
            //}


            //if (www.downloadHandler.text == "Not Connected")
            //{
            //    //Tidak Connect Data Base
            //}
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
