using System;
using System.Collections.Generic;

public class Database
{
    private string Name;
    private Dictionary<string, Table> Tables;
    private SQLParser Parser;

    public Database(string name)
    {
        this.Name = "Databases/" + name;
        this.Tables = new Dictionary<string, Table>();
        Parser = new SQLParser("TransitionFunction.txt");
    }

    public Table GetTable(string tableName)
    {
        string tablePath = this.Name + '#' + tableName;
        try
        {
            return Tables[tableName];
        }
        catch (KeyNotFoundException)
        {
            try
            {
                Table outTable = new Table(tablePath);
                Tables.Add(tableName, outTable);
                return outTable;
            }

            catch (Exception)
            {
                //Table doesn't exist
                throw;
            }
        }
    }

    public bool CreateTable(string tableName, List<string> colNames)
    {
        try
        {
            Tables.Add(tableName, new Table(Name + '#' + tableName, colNames));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DropTable(string tableName)
    {
        try
        {
            GetTable(tableName).Drop();
            Tables.Remove(tableName);
            return true;
        }
        catch (Exception)
        {
            throw;
        }

    }

    public bool Insert(string tableName, List<string> names, List<string> data)
    {
        if (names.Count != data.Count)
            throw new Exception();
        Dictionary<string, string> D = new Dictionary<string, string>();
        for (int i = 0; i < names.Count; i++)
        {
            D.Add(names[i], data[i]);
        }

        try
        {
            GetTable(tableName).InsertRow(D);
            return true;
        }

        catch(Exception)
        {
            throw;
        }

    }

    public Relation Select(string tableName, List<string> colNames)
    {
        try
        {
            return GetTable(tableName).SelectQuery(colNames);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Relation Select(string tableName)
    {
        try
        {
            return GetTable(tableName).SelectQuery();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool Write()
    {
        foreach(KeyValuePair<string, Table> kvp in Tables)
        {
            kvp.Value.WriteData();
        }
        return true;
    }

    public bool ProcessQuery(string Query)
    {
        DatabaseAction DA;
        Relation R;
        try
        {
            DA = Parser.Parse(Query);
            try
            {
                R = DA(this);
            }
            catch (Exception)
            {
                return false;
            }
            if (R != null) Console.WriteLine(R);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

}
