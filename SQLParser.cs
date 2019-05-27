using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public class SQLParser
{
    private int startState;
    private int[] acceptStates;
    private TransitionFunction tFunction;

    public SQLParser(string path)
    {
        startState = 0;
        String[] lines = System.IO.File.ReadAllText(path).Split('\n');
        acceptStates = lines[0].Split(' ').Skip(2).Select(x => Int32.Parse(x)).ToArray();
        tFunction = new TransitionFunction(lines);
    }

    public DatabaseAction Parse(string input)
    {
        DatabaseAction outValue = (x) => { return null; };
        int currentState = startState;
        bool isAcceptState = containedIn(currentState, acceptStates);
        StringBuilder tableName = new StringBuilder();
        StringBuilder colName = new StringBuilder();
        List<string> createColArray = new List<string>();
        List<string> insertInArray = new List<string>();
        List<string> insertValArray = new List<string>();
        List<string> selectValArray = new List<string>();
        foreach (char c in input)
        {
            currentState = tFunction.NextState(currentState, c);
            if (currentState == -1)
            {
                Console.WriteLine("Failed on {0}", c);
                isAcceptState = false;
                break;
            }
            if (currentState == 35 || currentState == 14 || currentState == 68 || currentState == 53)
            {
                tableName.Append(c);
            }
            else if (currentState == 36)
            {
                string TName = tableName.ToString();
                outValue = (D) =>
                {
                    try
                    {
                        D.DropTable(TName);
                        return null;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                tableName = new StringBuilder();
            }
            else if (currentState == 17 || currentState == 71 || currentState == 83 || currentState == 44)
            {
                colName.Append(c);
            }
            else if (currentState == 18 || currentState == 19)
            {
                createColArray.Add(colName.ToString());
                colName = new StringBuilder();
            }
            else if (currentState == 20)
            {
                List<string> colNames = createColArray;
                string TName = tableName.ToString();
                outValue = (D) =>
                {
                    try
                    {
                        D.CreateTable(TName, colNames);
                        return null;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                tableName = new StringBuilder();
                colName = new StringBuilder();
                createColArray = new List<string>();
            }
            else if (currentState == 45 || currentState == 46)
            {
                selectValArray.Add(colName.ToString());
                colName = new StringBuilder();
            }
            else if (currentState == 54)
            {
                List<string> selectValNames = selectValArray;
                string TName = tableName.ToString();
                outValue = (D) =>
                {
                    try
                    {
                        return D.Select(TName, selectValNames);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                selectValArray = new List<string>();
                tableName = new StringBuilder();
            }
            else if (currentState == 72 || currentState == 73)
            {
                insertInArray.Add(colName.ToString());
                colName = new StringBuilder();
            }
            else if (currentState == 84 || currentState == 85)
            {
                insertValArray.Add(colName.ToString());
                colName = new StringBuilder();
            }
            else if (currentState == 86)
            {
                List<string> insertInNames = insertInArray;
                List<string> insertValNames = insertValArray;
                string TName = tableName.ToString();
                outValue = (D) =>
                {
                    try
                    {
                        D.Insert(TName, insertInNames, insertValNames);
                        return null;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                tableName = new StringBuilder();
                insertInArray = new List<string>();
                insertValArray = new List<string>();
            }
            isAcceptState = containedIn(currentState, acceptStates);
        }
        if (!isAcceptState)
            throw new SyntaxException();
        return outValue;
    }

    private bool containedIn(int n, int[] A)
    {
        foreach (int i in A)
        {
            if (i == n)
                return true;
        }
        return false;
    }
}
