using System;
using System.IO;
using System.Collections.Generic;

public class Table : Relation
{
    private string Name;
    public Table(string tableName)
    {
        string path = tableName + ".csv";
        string tmp;
        this.Name = String.Copy(tableName);
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                int count = 0;
                foreach(string name in reader.ReadLine().Split(','))
                {
                    ColumnNames.Add(name, count++);
                }

                while(!String.IsNullOrWhiteSpace(tmp = reader.ReadLine()))
                {
                    TableData.Add(new List<string>());
                    foreach(string entry in tmp.Split(','))
                    {
                        TableData[TableData.Count - 1].Add(entry);
                    }
                    if (TableData[TableData.Count - 1].Count != count)
                    {
                        throw new Exception(String.Format("Column mismatch in table '{0}' at row {1}", tableName, TableData.Count));
                    }
                }

            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public Table(string tableName, List<string> colNames)
    {
        this.Name = tableName;
        int count = 0;
        foreach(string name in colNames)
        {
            ColumnNames.Add(name, count++);
        }
    }

    public void WriteData()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(this.Name + ".csv"))
            {
                writer.WriteLine(this.ToString());
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void InsertRow(Dictionary<string, string> D)
    {
        TableData.Add(new List<string>(ColumnNames.Count));
        for (int i = 0; i < ColumnNames.Count; i++)
        {
            TableData[TableData.Count - 1].Add("");
        }
        foreach (KeyValuePair<string, string> kvp in D)
        {
            TableData[TableData.Count - 1][ColumnNames[kvp.Key]] = kvp.Value;
        }

    }

    private void DeleteRowAt(int rowIndex)
    {
        try
        {
            TableData.RemoveAt(rowIndex);
        }
        catch(Exception e)
        {
            throw e;
        }
    }

    public void DeleteRows()
    {
        TableData = new List<List<string>>();
    }

    public bool Drop()
    {
        try
        {
            File.Delete(this.Name + ".csv");
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }

}
