using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Plat : MonoBehaviour
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
        form.AddField("plat", inputField[0].text);
        form.AddField("category", inputField[1].text);

        UnityWebRequest www = UnityWebRequest.Post("http://pkm-tol.herokuapp.com/public/api/vahicle/12", form);
        www.SendWebRequest();
        Debug.Log(Session.GetInstance().Id);

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
            //exeption.text = www.downloadHandler.text;
        }

        if (www.error == null)
        {
            //Query Selesai
            Debug.Log("Selesai");
            Debug.Log(www.downloadHandler.text);

            string json = www.downloadHandler.text;
            PlatID myPlat = new PlatID();
            myPlat = JsonUtility.FromJson<PlatID>(json);
            Debug.Log(myPlat.id);

            Session.GetInstance().Id = myPlat.id;
            PlayerPrefs.SetString("PlatID", myPlat.id);
            Debug.Log(Session.GetInstance().Id);
        }
        else
        {   
            Debug.Log(www.error);
        }
    }

    class PlatID
    {
        public string id;
    }
}
