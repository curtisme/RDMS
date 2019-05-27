using System;
using System.IO;
using System.Linq;

class TransitionFunction
{
    private int[][] FunctionTable;
    public TransitionFunction(string[] lines)
    {
        try
        {
            int[] numStatesAndAlphaSize = lines[0].Split(' ').Take(2).Select(x => Int32.Parse(x)).ToArray();
            FunctionTable = new int[numStatesAndAlphaSize[0]][];
            for (int i = 0; i < FunctionTable.Length; i++)
            {
                FunctionTable[i] = new int[numStatesAndAlphaSize[1]];
                for (int j = 0; j < FunctionTable[i].Length; j++)
                {
                    FunctionTable[i][j] = -1;
                }
            }
            for (int i = 1; i < lines.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(lines[i]))
                    continue;
                //format of line will be oldState,symbol,newState
                String[] entries = lines[i].Split('|');
                int oldState = Int32.Parse(entries[0]);
                int newState = Int32.Parse(entries[2]);
                if (entries[1].Equals("all"))
                {
                    for (int j = 0; j < FunctionTable[oldState].Length; j++)
                    {
                        FunctionTable[oldState][j] = newState;
                    }
                }
                else
                {
                    FunctionTable[oldState][GetInt(entries[1][0])] = newState;
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    private  int GetInt(char c)
    {
        int outInt;
        c = Char.ToLower(c);
        switch(c)
        {
            case ' ':
                outInt = 26;
                break;
            case '_':
                outInt = 27;
                break;
            case '(':
                outInt = 28;
                break;
            case ')':
                outInt = 29;
                break;
            case ',':
                outInt = 30;
                break;
            case ';':
                outInt = 31;
                break;
            case '=':
                outInt = 32;
                break;
            case '*':
                outInt = 33;
                break;
            case '0':
                outInt = 34;
                break;
            case '1':
                outInt = 35;
                break;
            case '2':
                outInt = 36;
                break;
            case '3':
                outInt = 37;
                break;
            case '4':
                outInt = 38;
                break;
            case '5':
                outInt = 39;
                break;
            case '6':
                outInt = 40;
                break;
            case '7':
                outInt = 41;
                break;
            case '8':
                outInt = 42;
                break;
            case '9':
                outInt = 43;
                break;
            default:
                outInt = (int)c - (int)'a';
                if (outInt < 0 || outInt > 25)
                {
                    outInt = -1;
                }
                break;
        }
        return outInt;
    }

    public int NextState(int oldState, char symbol)
    {
        int newState;
        try
        {
            newState = FunctionTable[oldState][GetInt(symbol)];
        }
        catch
        {
            newState = -1;
        }
        return newState;
    }

    public override string ToString()
    {
        string outStr = String.Format("States: {0} Alphabet Size: {1}\n", FunctionTable.Length, FunctionTable[0].Length);
        foreach (int[] row in FunctionTable)
        {
            foreach (int entry in row)
            {
                outStr += String.Format("{0, 3} ", entry);
            }
            outStr += '\n';
        }
        return outStr;
    }
}
