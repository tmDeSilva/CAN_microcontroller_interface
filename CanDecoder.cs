using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CAN_data_logger.Properties;
using System.Windows.Markup;
using NCalc;
public class CanDecoder
{
    public Dictionary<string, List<string>> channelID_Components = new Dictionary<string, List<string>>();
    public Dictionary<string, string> channelID_channelName = new Dictionary<string, string>();
    public Dictionary<string, Tuple<int, int, float, float, string>> channelID_decodeData = new Dictionary<string, Tuple<int, int, float, float, string>>();
    private HashSet<int> IDs = new HashSet<int>();

    public Dictionary<string, List<string>> parameterToComponent = new Dictionary<string, List<string>>();
    public Dictionary<string, Tuple<int,int>> componentToID = new Dictionary<string, Tuple<int, int>>();


    public CanDecoder(string dbcFileString, string sampleDataFileString, string essentialParametersFileString)
    {
        var dbcChannelData = dbcFileString.Split('*');

        List<string> essentialParametersFile = essentialParametersFileString.Split(new string[] { "}\r\n" }, StringSplitOptions.None).ToList();


        foreach (var channelData_byID in dbcChannelData)
        {
            var channelData_byID_line = channelData_byID.Split('\n');
            var specificChannelID = channelData_byID_line[0].Substring(4, 4);
            channelID_channelName[specificChannelID] = channelData_byID_line[0].Substring(9);

            var channelData_byID_line_withoutHeaderFooter = channelData_byID_line.Skip(1).Take(channelData_byID_line.Length - 2).ToArray();
            channelID_Components[specificChannelID] = channelData_byID_line_withoutHeaderFooter.Select(iComponentData => iComponentData.Split(':')[0].Substring(1)).ToList();

            foreach (var iComponentData in channelData_byID_line_withoutHeaderFooter)
            {
                var splitComponentData = iComponentData.Split(':');
                var componentName = splitComponentData[0].Substring(1);
                var rawDecodeData_fromDbcFile = splitComponentData[1].Split();

                var bit_range = rawDecodeData_fromDbcFile[1].Substring(0, rawDecodeData_fromDbcFile[1].Length - 3).Split('|').Select(int.Parse).ToList();
                var formulaTuple = rawDecodeData_fromDbcFile[2].Substring(1, rawDecodeData_fromDbcFile[2].Length - 2).Split(',').Select(float.Parse).ToList();
                var unit = rawDecodeData_fromDbcFile[4].Substring(1).Trim('Â');

                channelID_decodeData[componentName] = Tuple.Create(bit_range[0], bit_range[1], formulaTuple[0], formulaTuple[1], unit);
            }
        }

        foreach (var channelData_byID_line in sampleDataFileString.Split(new string[] { "\r\n" }, StringSplitOptions.None))
        {
            IDs.Add(Convert.ToInt32(channelData_byID_line.Substring(0, 3), 16));
        }

        foreach (var line in essentialParametersFile)
        {
            var splitLine = line.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            var batteryParameter = splitLine[0].Substring(0, splitLine[0].Length - 2);

            foreach (var element in splitLine.Skip(1))
            {
                var splitElement = element.Replace("\t-", "").Split(':');
                if (splitElement.Length > 1)
                {
                    var part = splitElement[0];


                    if (part.Contains("{n"))
                    {
                        var mode = part.Split('[')[0].Last();
                        var formula = splitElement[1].TrimStart('[').TrimEnd(']').Split(',');
                        
                        if (mode == 'r')

                        {
                            formula[0] = formula[0].Substring(1);
                            formula[1] = formula[1].Substring(0, formula[1].Length - 2);
                            var range = part.Split('[')[1].Split(']')[0].Split(',').Select(int.Parse).ToArray();
                            for (var n = range[0]; n <= range[1]; n++)
                            {
                                Expression iXFormula = new Expression(formula[0]);
                                Expression iYFormula = new Expression(formula[1]);
                                iXFormula.Parameters["n"] = n;
                                iYFormula.Parameters["n"] = n;
                                var x = Convert.ToInt32(iXFormula.Evaluate());
                                var y = Convert.ToInt32(iYFormula.Evaluate());
                                var iPart = part.Split('[')[0].Replace("{n}", " " + Convert.ToString(n));
                                iPart = iPart.Substring(0, iPart.Length - 1);
                                
                                parameterToComponent = AddToParameterToComponent(parameterToComponent, iPart, batteryParameter);
                                componentToID = AddToComponentToID(componentToID, Tuple.Create(x, y), ComponentTitle(batteryParameter, iPart));
                            }
                        }
                        else if (mode == 'l')
                        {
                            formula[0] = formula[0].Substring(1);
                            formula[1] = formula[1].Substring(0, formula[1].Length - 2);
                            var list = part.Split('[')[1].Split(']')[0].Split(',').Select(int.Parse).ToArray();
                            for (var i = 0; i < list.Length; i++)
                            {
                                Expression iXFormula = new Expression(formula[0]);
                                Expression iYFormula = new Expression(formula[1]);
                                iXFormula.Parameters["i"] = i;
                                iYFormula.Parameters["i"] = i;
                                var x = (int)(iXFormula.Evaluate());
                                var y = (int)(iYFormula.Evaluate());
                                var iPart = part.Split('[')[0].Replace("{n}", " " + Convert.ToString(list[i]));
                                iPart = iPart.Substring(0, iPart.Length - 1);

                                parameterToComponent = AddToParameterToComponent(parameterToComponent, iPart, batteryParameter);
                                componentToID = AddToComponentToID(componentToID, Tuple.Create(x, y), ComponentTitle(batteryParameter, iPart));
                            }
                        }
                    }
                    else
                    {
                        var point = splitElement[1].Substring(1, splitElement[1].Length - 3).Split(',').Select(int.Parse).ToArray();

                        var x = point[0];
                        var y = point[1];
                        parameterToComponent = AddToParameterToComponent(parameterToComponent, part, batteryParameter);
                        componentToID = AddToComponentToID(componentToID, Tuple.Create(x, y), ComponentTitle(batteryParameter, part));
                    }
                }
            }
        }
    }

    #region Functions for Debugging
    public void DisplayIDs()
    {
        
        foreach (var iID in IDs)
        {
            if (channelID_channelName.TryGetValue(iID.ToString(), out var channelName))
            {
                Console.WriteLine($"{iID} {channelName}");
            }
        }
    }
    public void DisplayIDComponents(string pSpecificChannelID)
    {
        if (channelID_Components.TryGetValue(pSpecificChannelID, out var components))
        {
            for (var i = 0; i < components.Count; i++)
            {
                Console.WriteLine($"{i} {components[i]}");
            }
        }
    }
    #endregion
    public List<(string, double, string)> ProcessData(string pData)
    {
    
        string pID = Convert.ToInt32(pData.Substring(0, 3), 16).ToString();
      

        string pBinary = "";
        List<string> list = new List<string>(pData.Substring(6).Split(' '));
        foreach (var hexValue in list.Take(list.Count - 1).ToList())
        {

            string iBinaryValue = Convert.ToString(Convert.ToInt32(hexValue, 16), 2);
            if (iBinaryValue.Length < 8)
            {
                iBinaryValue = iBinaryValue.PadLeft(8, '0');
            }
            char[] iBinaryArray = iBinaryValue.ToCharArray();
            Array.Reverse(iBinaryArray);
            string iBinary = new string(iBinaryArray);
                
            pBinary = pBinary + iBinary;

        }

        
        var res = new List<(string, double, string)>();
 

        foreach (var iComponent in channelID_Components[pID])
        {
            
            var (iBrS, iBrL, iSf, iOf, iUnit) = channelID_decodeData[iComponent];
            if (iBrL < 33) 
            { 
                double iRes = Convert.ToInt32(new string(pBinary.Substring(iBrS, iBrL).Reverse().ToArray()), 2) * iSf + iOf;
                res.Add((iComponent, iRes, iUnit));
            }
            else
            {
                double iRes = 1.0;
                res.Add((iComponent, iRes, iUnit));
            }
            
            
        }
        return res;
    }
    private Dictionary<string, List<string>> AddToParameterToComponent(Dictionary<string, List<string>> dictionary, string value, string key)
    {

        if (!dictionary.ContainsKey(key))
        {
            dictionary[key] = new List<string>();
        }
        dictionary[key].Add(value);
        return dictionary;

    }

    private Dictionary<string, Tuple<int, int>> AddToComponentToID(Dictionary<string, Tuple<int, int>> dictionary, Tuple<int, int> value, string key)
    {

        dictionary[key] = value;
        return dictionary;
    }

    static string ComponentTitle(string parameter, string part)
    {
        return $"{parameter}-{part}".ToLower();
    }
}