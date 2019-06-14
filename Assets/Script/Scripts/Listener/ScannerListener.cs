using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScannerListener : MonoBehaviour
{
    //Button Scanner
    public Button ScanBtn;

    public Text[] text;

    //Region
    private string s_Region;

    // Dummy Respon
    public GameObject PopUpLoading;
    public GameObject PopUpDetected;

    
    void Start()
    {
        // Enable Bluetooth
        BluetoothState.Init();
        BluetoothState.EnableBluetooth();

        //Init Pop Up
        PopUpDetected.active = false;
        PopUpLoading.active = false;

        // Auto Region
        s_Region = "Indonesia";

        // Button Listener
        ScanBtn.onClick.AddListener(startScan);
    }

    private void startScan()
    {
        // active popup loading
        PopUpLoading.active = true;
        // Add Beacon Listener
        iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
        // Fill Mandatories
        iBeaconReceiver.regions = new iBeaconRegion[] { new iBeaconRegion(s_Region, new Beacon()) };
        // Start Scan
        iBeaconReceiver.Scan();
        Debug.Log("Listening for beacons");
    }

    // Listener jika masuk jangkauan beacons
    private void OnBeaconRangeChanged(Beacon[] beacons)
    {
        foreach (Beacon b in beacons)
        {
            // Dummy Checking
            if (b != null)
            {
                // change pop up
                PopUpLoading.active = false;
                PopUpDetected.active = true;

                //Place API Transaction Here
                //text.text = b.UUID;

                string uuid = b.UUID;
                StartCoroutine(PostTransaction(uuid));
                //-----------
                iBeaconReceiver.Stop();
            }

            // Featured Checking
            //if (b.UUID == "PintulTol UUID")
            //{
            //  PopUp.active = true;
            //  Place API Transaction Here
            //  iBeaconReceiver.Stop();
            //}
        }
    }

    IEnumerator PostTransaction(string uuid)
    {
        //Form
        WWWForm form = new WWWForm();

        form.AddField("uuid", uuid);

        UnityWebRequest www = UnityWebRequest.Post("http://pkm-tol.herokuapp.com/public/api/transaction/12", form);
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

                string json = www.downloadHandler.text;
                Transaction myTrans = new Transaction();
                myTrans = JsonUtility.FromJson<Transaction>(json);

                Session.GetInstance().Price = myTrans.price;
                Session.GetInstance().Masuk = myTrans.masuk;
                Session.GetInstance().Keluar = myTrans.keluar;
                PlayerPrefs.SetString("Price", myTrans.price);
                PlayerPrefs.SetString("Masuk", myTrans.masuk);
                PlayerPrefs.SetString("Keluar", myTrans.keluar);

                string a = "Masuk : " + Session.GetInstance().Masuk;
                string b = "Keluar : " + Session.GetInstance().Keluar;
                string c = "Harga : " + Session.GetInstance().Price;

                text[0].text = a;
                text[1].text = b;
                text[2].text = c;
            }

        }
        else
        {
            Debug.Log(www.error);
        }
    }

    class Transaction
    {
        public string masuk;
        public string keluar;
        public string price;
    }
}
