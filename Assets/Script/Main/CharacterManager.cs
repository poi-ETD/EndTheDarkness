using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class CharacterManager : MonoBehaviour
{
    CharacterData CD = new CharacterData();
    public GameObject passiveMuch;
    public GameObject[] checkBox;
    public Sprite onCheck;
    float timer;
    public Sprite offCheck; 
    public void ToMain()
    {

        string characterData = JsonUtility.ToJson(CD, true);
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        File.WriteAllText(path, characterData);
        SceneManager.LoadScene("Main");
    }
    private void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "CharacterData.json");
        if (File.Exists(path))
        {
            string characterData = File.ReadAllText(path);
            CD = JsonUtility.FromJson<CharacterData>(characterData);
            for (int i = 0; i < 16; i++)
            {
             
            if(CD.passive[i])
            checkBox[i].GetComponent<Image>().sprite = onCheck;
               
            }
        }
     

    }
    void onMuch()
    {
        passiveMuch.SetActive(true);
        timer =1;
    }
    
    void offMuch()
    {
        passiveMuch.SetActive(false);
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                offMuch();
                timer = 0;
            }
        }
    }
    public void onPassive(int i)
    {
       
        if (CD.passive[i])
        {
           
            checkBox[i].GetComponent<Image>().sprite = offCheck;
            CD.passive[i] = false;
            CD.passiveCount[i/4]--;
        }
        else if (!CD.passive[i])
        {
           
            if (CD.passiveCount[i/4] < 2)
            {
                checkBox[i].GetComponent<Image>().sprite = onCheck;
                CD.passive[i] = true;
                CD.passiveCount[i/4]++;

            }
            else
            {
                onMuch();
            }
        }

    }
}
public class CharacterData
{
    public bool[] passive=new bool[30];
    public int[] passiveCount=new int[10];
}
