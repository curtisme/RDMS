using System;
using System.IO;
using System.Collections.Generic;

public class Relation
{
    protected Dictionary<string, int> ColumnNames;
    protected List<List<string>> TableData;

    public Relation()
    {
        ColumnNames = new Dictionary<string, int>();
        TableData = new List<List<string>>();
    }
    public Relation(Dictionary<string, int> D, List<List<string>> L)
    {
        ColumnNames = D;
        TableData = L;
    }

    public Relation SelectQuery(List<string> columns)
    {
        if (columns.Count  < 1 || columns[0].Length < 1)
            return new Relation(ColumnNames, TableData);
        Dictionary<string, int> D = new Dictionary<string, int>();
        List<List<string>> L = new List<List<string>>();
        int count = 0;
        try
        {
            foreach (string name in columns)
            {
                D.Add(name, count++);
            }
            foreach (List<string> row in TableData)
            {
                L.Add(new List<string>());
                foreach(string name in columns)
                {                
                    L[L.Count - 1].Add(row[ColumnNames[name]]);
                }
            }
            return new Relation(D, L);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Relation SelectQuery()
    {
        return new Relation(ColumnNames, TableData);
    }

    public override string ToString()
    {
        string outStr = "";
        foreach (KeyValuePair<string, int> kvp in ColumnNames)
        {
            outStr += kvp.Key + ',';
        }
        outStr += '\n';
        foreach (List<string> row in TableData)
        {
            foreach (KeyValuePair<string, int> kvp in ColumnNames)
            {
                outStr += row[kvp.Value] + ',';
            }
            outStr += '\n';
        }
        return outStr.Replace(",\n", "\n");
    }
}
