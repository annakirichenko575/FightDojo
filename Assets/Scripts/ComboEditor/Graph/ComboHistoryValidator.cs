using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace FightDojo.ComboEditor.ComboEditor.Graph
{
  public class ComboHistoryValidator
  {
    const string Header     = "ComboID;Date;Game;Character;Author;Quality%";
    const string DateFormat = "yyyy-MM-dd HH:mm:ss";

    public void Clean(string path, int cleanId = -1)
    {
      Debug.Log("Clean svx");
      string tempPath = path + ".tmp";
      try
      {
        using StreamReader reader = new StreamReader(path);
        using StreamWriter writer = new StreamWriter(tempPath);

        writer.WriteLine(Header);

        string firstLine = reader.ReadLine();
        if (firstLine != null && firstLine.Trim() != Header)
          TryWriteLine(firstLine, writer, cleanId);

        while (!reader.EndOfStream)
          TryWriteLine(reader.ReadLine(), writer, cleanId);

        reader.Close(); writer.Close();
        File.Delete(path);
        File.Move(tempPath, path);
      }
      catch { File.Delete(tempPath); throw; }
    }

    private void TryWriteLine(string line, StreamWriter writer, int cleanId)
    {
      if (string.IsNullOrWhiteSpace(line)) 
        return;

      string[] fields = line.Split(';');
      if (fields.Length != 6) 
        return;

      if (!int.TryParse(Strip(fields[0]), out int comboId) 
          || comboId <= 0 || comboId == cleanId) 
        return;
      
      if (!DateTime.TryParseExact(Strip(fields[1]), DateFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) 
        return;
      
      if (string.IsNullOrWhiteSpace(Strip(fields[2])) 
          || string.IsNullOrWhiteSpace(Strip(fields[3]))) 
        return;
      
      if (!float.TryParse(Strip(fields[5]), NumberStyles.Float,
            CultureInfo.InvariantCulture, out float quality) 
          || quality < 0 || quality > 1) 
        return;

      writer.WriteLine(line);
    }

    private string Strip(string value) => value.Trim().Trim('"');
  }
}