using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class ScoreLoger : MonoBehaviour
{
    public UnityEngine.UI.Text scoreBar;
    public UnityEngine.UI.Text bestScore;

    void Start()
    {
        string path = "./records";
        if(!File.Exists(path))
            CreateFile(path);

        WritreXML(path);

        GetBestScore(path);
    }

    void CreateFile(string path)
    {
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sw.WriteLine("<records>");
            sw.WriteLine("</records>");
        }
    }

    void WritreXML(string path)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);
        XmlElement xRoot = xDoc.DocumentElement;
            XmlElement scoreElem = xDoc.CreateElement("record");
                XmlElement scoreAttr = xDoc.CreateElement("score");
                    XmlText scoreText = xDoc.CreateTextNode(scoreBar.text);
                    scoreAttr.AppendChild(scoreText);
                scoreElem.AppendChild(scoreAttr);
                XmlElement dateAttr = xDoc.CreateElement("date");
                    XmlText dateText = xDoc.CreateTextNode(System.DateTime.UtcNow.ToString());
                    dateAttr.AppendChild(dateText);
                scoreElem.AppendChild(dateAttr);
                XmlElement usernameAttr = xDoc.CreateElement("username");
                    XmlText usernameText = xDoc.CreateTextNode(System.Environment.UserName);
                    usernameAttr.AppendChild(usernameText);
                scoreElem.AppendChild(usernameAttr);
            xRoot.AppendChild(scoreElem);

        xDoc.Save(path);
    }

    void GetBestScore(string path)
    {
        float bestScore = 0;

        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);
        XmlElement xRoot = xDoc.DocumentElement;
        foreach(XmlNode xnode in xRoot)
        {
            foreach(XmlNode childnode in xnode.ChildNodes)
            {
                if(childnode.Name=="score")
                {
                    float score = float.Parse(childnode.InnerText);

                    if(score > bestScore)
                    {
                        bestScore = score;
                    }
                }
            }
        }

        this.bestScore.text = bestScore.ToString();
    }
}
