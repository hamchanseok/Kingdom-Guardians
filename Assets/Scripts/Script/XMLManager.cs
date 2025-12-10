using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class XMLManager : MonoBehaviour
{
    public static XMLManager s;

    //xml ÆÄÀÏ
    public TextAsset enemyFileXml;

    struct MonParams
    {
        public string name;
        public int maxHp;
        public int damage;
    }

    Dictionary<string, MonParams> dicMonsters = new Dictionary<string, MonParams>();

    void Awake()
    {
        s = this;
    }

    private void Start()
    {
        MakeMonsterXML();
    }

    void MakeMonsterXML()
    {
        XmlDocument monsterXMLDoc = new XmlDocument();
        monsterXMLDoc.LoadXml(enemyFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonParams monParams = new MonParams();
            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "name")
                {
                    monParams.name = childNode.InnerText;
                }

                if (childNode.Name == "maxHp")
                {
                    monParams.maxHp = Int32.Parse(childNode.InnerText);
                }

                if (childNode.Name == "damage")
                {
                    monParams.damage = Int32.Parse(childNode.InnerText);
                }
                print(childNode.Name + ": " + childNode.InnerText);
            }
            dicMonsters.Add(monParams.name, monParams);
        }
    }

    public void LoadMonsterParamsFromXML(string monName, EnemyStats stats)
    {
        if (dicMonsters.ContainsKey(monName))
        {
            stats.statCurHP = stats.statMaxHP = dicMonsters[monName].maxHp;
            stats.enemyDamage = dicMonsters[monName].damage;
        }
        else
        {
            Debug.LogError("Monster name '" + monName + "' not found in XML file.");
        }
    }
}