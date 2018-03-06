using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CsvManager : MonoBehaviour {
    public static List<string[]> LoadCsv(string path)
    {
        List<string[]> r = new List<string[]>();
        Debug.Log(path);
        TextAsset csvFile = Resources.Load(path) as TextAsset;//Assets/Resources以下のぱす
        StringReader sr = new StringReader(csvFile.text);
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            string[] buf = line.Split(',');
                r.Add(buf);
        }

        return r;
    }

    public static void SaveCsv(string path,string text)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath +"/Resources/"+ path);
        sw = fi.AppendText();
        sw.WriteLine(text);
        sw.Flush();
        sw.Close();
    }
}
