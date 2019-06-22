using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetBalance : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetBalancePrice()
    {
        StartCoroutine(BalancePrice());
    }

    IEnumerator BalancePrice()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://pkm-tol.herokuapp.com/public/api/profile/" + Session.GetInstance().Id);
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
        }

        if (www.error == null)
        {
            if (www.downloadHandler.isDone)
            {
                //Query Selesai
                Debug.Log("Selesai");
                //Debug.Log(www.downloadHandler.text);

                Balance myBalance = JsonUtility.FromJson<Balance>(www.downloadHandler.text);
                text.text = "Balance : Rp." + myBalance.saldo;
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    class Balance
    {
        public string saldo;
    }


}
