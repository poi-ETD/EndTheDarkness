using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Recover : MonoBehaviour
{
    [SerializeField] LobbyManager lobby;
    int recoverCount;
    [SerializeField] GameObject[] Recovers;
    bool recoverMode;
    bool reviveMode;
    [SerializeField] GameObject recoverView;
    [SerializeField] GameObject PopUpCanvas;
    [SerializeField] GameObject Popup;
    [SerializeField] TextMeshProUGUI popupText;
    bool ActBless10;
    [SerializeField] GameObject bless10;
    [SerializeField] GameObject bless13;
    public void RecoverD()
    {   
        if (lobby.GD.blessbool[15]) return;
        if (lobby.GD.isAct) return;
        if (lobby.canvasOn) return;
        recoverView.SetActive(true);
        lobby.canvasOn = true;
        PopUpCanvas.SetActive(true);
        if (lobby.GD.blessbool[10])
        {
            bless10.SetActive(true);
        }
        else
        {
            bless10.SetActive(false);
        }
        if (lobby.GD.blessbool[13])
        {
            bless13.SetActive(true);
        }
        else
        {
            bless13.SetActive(false);
        }
        for (int i = 0; i < lobby.ChD.size; i++)
        {
            if (lobby.ChD.characterDatas[i].curHp <= 0) continue;
            Recovers[i].SetActive(true);
            Recovers[i].GetComponent<TextMeshProUGUI>().text = lobby.ChD.characterDatas[i].name;
            Recovers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobby.ChD.characterDatas[i].curHp + "/" + lobby.ChD.characterDatas[i].maxHp;
            Recovers[i].transform.GetChild(1).gameObject.SetActive(true);
            Recovers[i].transform.GetChild(2).gameObject.SetActive(true);
            Recovers[i].transform.GetChild(3).gameObject.SetActive(false);

        }
        for (int i = lobby.ChD.size; i < 4; i++)
        {
            Recovers[i].SetActive(false);
        }

    }

    public void ReviveOn()
    {
        if (lobby.GD.blessbool[15]) return;
        lobby.RitualViewOff();
        recoverView.SetActive(true);
        lobby.canvasOn = true;
        PopUpCanvas.SetActive(true);
        if (lobby.GD.blessbool[10])
        {
            bless10.SetActive(true);
        }
        else
        {
            bless10.SetActive(false);
        }
        if (lobby.GD.blessbool[13])
        {
            bless13.SetActive(true);
        }
        else
        {
            bless13.SetActive(false);
        }
        for (int i = 0; i < lobby.ChD.size; i++)
        {
            Recovers[i].SetActive(true);
            Recovers[i].GetComponent<TextMeshProUGUI>().text = lobby.ChD.characterDatas[i].name;
            Recovers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobby.ChD.characterDatas[i].curHp + "/" + lobby.ChD.characterDatas[i].maxHp;
             if (lobby.ChD.characterDatas[i].curHp == 0)
             {
                
                     Recovers[i].transform.GetChild(1).gameObject.SetActive(false);
                     Recovers[i].transform.GetChild(2).gameObject.SetActive(false);
                     Recovers[i].transform.GetChild(3).gameObject.SetActive(true);
                 
             }
            else
            {
                Recovers[i].transform.GetChild(1).gameObject.SetActive(false);
                Recovers[i].transform.GetChild(2).gameObject.SetActive(false);
                Recovers[i].transform.GetChild(3).gameObject.SetActive(false);

            }

        }
        for (int i = lobby.ChD.size; i < 4; i++)
        {
            Recovers[i].SetActive(false);
        }
    }
    public void Revive(int i)
    {if (!lobby.GD.blessbool[13])
        {
            if (lobby.GD.tribute < 1000)
            {
                Popup.SetActive(true);
                return;
            }
            lobby.GD.tribute -= 1000;
        }
        lobby.ChD.characterDatas[i].curHp = 1;
        lobby.save();
        ReviveEnd();
    }
    public void EmergencyRecover(int i)
    {
        if (lobby.GD.Ignum < 10)
        {
            Popup.SetActive(true);
            return;
        }
        lobby.GD.Ignum -= 10;
        lobby.ChD.characterDatas[i].curHp += lobby.ChD.characterDatas[i].maxHp * Random.Range(1, 3) / 10;
        if (lobby.ChD.characterDatas[i].curHp > lobby.ChD.characterDatas[i].maxHp) lobby.ChD.characterDatas[i].curHp = lobby.ChD.characterDatas[i].maxHp;
        lobby.save();
        RecoverEnd();
    }
    public void HighRecover(int i)
    {
        if (lobby.GD.Ignum < 100)
        {
            Popup.SetActive(true);
            return;
        }
        lobby.GD.Ignum -= 100;
        lobby.ChD.characterDatas[i].curHp += lobby.ChD.characterDatas[i].maxHp * Random.Range(5, 10) / 10;
        if (lobby.ChD.characterDatas[i].curHp > lobby.ChD.characterDatas[i].maxHp) lobby.ChD.characterDatas[i].curHp = lobby.ChD.characterDatas[i].maxHp;
        lobby.save();
        RecoverEnd();
    }
    public void RecoverEnd()
    {
        if (!lobby.GD.blessbool[10])
        {
            recoverView.SetActive(false);
            PopUpCanvas.SetActive(false);
            lobby.Act();
            lobby.canvasOn = false;
        }
        else
        {
            if (ActBless10)
            {
                recoverView.SetActive(false);
                PopUpCanvas.SetActive(false);
              
                lobby.canvasOn = false;
                ActBless10 = false;
            }
            else
            {
                lobby.Act();
                ActBless10 = true;
            }
        }
    }
    public void ReviveEnd()
    {
        recoverView.SetActive(false);
        PopUpCanvas.SetActive(false);
        lobby.Act();
        lobby.canvasOn = false;
    }
}
