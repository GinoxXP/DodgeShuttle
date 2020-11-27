using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class ScoreLoger : MonoBehaviour
{
    public UnityEngine.UI.Text scoreBar;

    void Start()
    {
        string path = "./recordes";
        if(!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sw.WriteLine("<recordes>");
                sw.WriteLine("</recordes>");
            }
        }

        XmlDocument xDoc = new XmlDocument();
        xDoc.Load("./recordes");
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

        xDoc.Save("./recordes");
    }
}
