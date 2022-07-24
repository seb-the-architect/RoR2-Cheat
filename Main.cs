using System;
using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;

class MyMod : MonoBehaviour
{
    bool bMenu = false;
    bool bLucky = false;
    bool bRich = false;
    bool bGod = false;
    bool bSaved = false;
    bool bshowChests = false;

    public void Start()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            bMenu = !bMenu;
        }
        if (!myCharacter || !myBody) { GetCharacter(); }
        else
        {
            if (bLucky)
            {
                myCharacter.luck = 1; 
            }
            if (bRich)
            {
                myCharacter.money = 3000;
            }
            if (bSaved)
            {
                if (myBody.healthComponent.isHealthLow)
                {
                    bGod = true;
                }
            }
            myBody.healthComponent.godMode = bGod;

            var method = typeof(RoR2.CharacterBody).GetMethod("set_damage", BindingFlags.NonPublic);
            method.Invoke(myBody, new object[] { 99999f });

            /*
            MethodInfo SetDamageInfo= typeof(RoR2.CharacterBody).GetMethod("set_damage", bindFlags);
            SetDamageInfo.Invoke(new RoR2.CharacterBody(), new object[] { new List<float>() { 9999f } });
            */
        }
    }

    int frames;
    List<RoR2.PurchaseInteraction> purchaseList = new List<RoR2.PurchaseInteraction>();
    //RoR2.PurchaseInteraction.FindObjectsOfType(typeof(RoR2.PurchaseInteraction))
    public void OnGUI()
    {
        // ----- ESP -----
        if (bshowChests)
        {
            frames++;
            if(frames >= 100)
            {
                frames = 0;
                purchaseList.Clear();
                foreach (RoR2.PurchaseInteraction PurchaseInteraction in RoR2.PurchaseInteraction.FindObjectsOfType(typeof(RoR2.PurchaseInteraction)))
                {
                    purchaseList.Add(PurchaseInteraction);
                }

                Teleporter = FindObjectOfType<RoR2.TeleporterInteraction>();

            }

            RenderTeleporter();

            foreach (RoR2.PurchaseInteraction purchaseInteraction in purchaseList)
            {
                if (purchaseInteraction.available)
                {
                    Vector3 vector = Camera.main.WorldToScreenPoint(purchaseInteraction.transform.position);
                    if ((double)vector.z > 0.01)
                    {
                        GUI.color = Color.cyan;
                        string displayName = purchaseInteraction.GetDisplayName();
                        if (displayName.Contains("Chest") || displayName.Contains("Shrine"))
                        {
                            GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 100f, 50f), displayName);
                            //double distance = distanceBetween(purchaseInteraction.transform.position, myBody.footPosition);
                            //GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y + 20f, 100f, 50f), ((Math.Truncate(distance) / 10).ToString()) + "m");
                        }
                    }
                }
            }
        }

        // ----- GUI -----
        if (bMenu)
        {
            GUI.Box(new Rect(Screen.width - 250, 10, 200, 180), "RoR2 Legit Cheat");
            bLucky = GUI.Toggle(new Rect(Screen.width - 250, 40, 160, 20), bLucky, "Lucky");

            if (GUI.Button(new Rect(Screen.width - 250, 70, 100, 22), "Add 100 Coins"))
            {
                myCharacter.money += 100;
            }

            bGod = GUI.Toggle(new Rect(Screen.width - 250, 100, 160, 20), bGod, "Invincible");

            bSaved = GUI.Toggle(new Rect(Screen.width - 250, 130, 160, 20), bSaved, "Stay Safe");

            bshowChests = GUI.Toggle(new Rect(Screen.width - 250, 160, 160, 20), bshowChests, "XRay");
        }


    }

    RoR2.TeleporterInteraction Teleporter;
    void RenderTeleporter()
    {
        Vector3 vector = Camera.main.WorldToScreenPoint(Teleporter.transform.position);
        if ((double)vector.z > 0.01)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(vector.x, (float)Screen.height - vector.y, 100f, 50f), "Teleporter");
        }
    }

    double distanceBetween(Vector3 pointA, Vector3 pointB)
    {
        return Math.Sqrt(Math.Pow((pointA.x - pointB.x), 2) + Math.Pow((pointA.y - pointB.y), 2) + Math.Pow((pointA.z - pointB.z), 2));
    }

    void GetCharacter()
    {
        RoR2.CharacterMaster[] allCharacters;
        RoR2.CharacterBody[] allBodies;

        allCharacters = FindObjectsOfType<RoR2.CharacterMaster>();
        //allBodies = FindObjectsOfType<RoR2.CharacterBody>();
        myBody = FindObjectOfType<RoR2.CharacterBody>();
        for (int i = 0; i < allCharacters.Length; i++)
        {
            if (allCharacters[i].name == "PlayerMaster(Clone)")
            {
                myCharacter = allCharacters[i];
            }
        }

        /*
        for (int i = 0; i < allBodies.Length; i++)
        {
            if (allBodies[i].name == "CommandoBody(Clone)")
            {
                myBody = allBodies[i];
            }
        }
        */
    }

    RoR2.CharacterMaster myCharacter;
    RoR2.CharacterBody myBody;

}