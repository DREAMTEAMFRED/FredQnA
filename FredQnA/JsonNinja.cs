using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class JsonNinja
    {
        List<string> names = new List<string>();
        List<string> vals = new List<string>();

        public List<string> GetVals()
        {
            return vals;
        }

        public JsonNinja(string data)
        {
            names.Clear();
            vals.Clear();
            data = data.Replace("}", "");
            data = data.Replace("{", "");
            int startPos = 0;
            int endPos = 0;
            int endPosVal = 0;
            bool startFound = false;
            bool endFound = false;
            bool collection = false;
            int collectionCounter = 0;

            int k = 0;
            while(k < data.Length)
            {
                char test = data[k];
                if (data[k] == '"' && startFound == false)
                {
                    startFound = true;
                    startPos = k;
                }
                if(data[k] == ':' && endFound == false)
                {
                    if (data[k + 1] == '[')
                    {
                        collection = true;
                    }
                    endPos = k;
                    endFound = true;

                    string nameCut = data.Substring(startPos, endPos - startPos);
                    names.Add(nameCut);
                }

                if (collection == true)
                {
                    if (data[k] == '[')
                        collectionCounter++;

                    if(data[k] == ']')
                    {
                        if(collectionCounter == 1)
                        {
                            endPosVal = k;
                            string valCut = data.Substring(endPos + 1, endPosVal - (endPos));
                            vals.Add(valCut);
                            startFound = false;
                            endFound = false;
                            collectionCounter = 0;
                            collection = false;
                        }
                        else
                        {
                            collectionCounter--;
                        }
                    }
                }
                else if ((data[k] == ',' && data[k + 1] == '"' && startFound == true) || k == data.Length - 1)
                {
                    if(k == data.Length - 1)
                    {
                        endPosVal = k;
                        string valCut = data.Substring(endPos + 1, endPosVal - (endPos));
                        vals.Add(valCut);
                        startFound = false;
                        endFound = false;
                    }
                    else
                    {
                        endPosVal = k;
                        string valCut = data.Substring(endPos + 1, endPosVal - (endPos + 1));
                        vals.Add(valCut);
                        startFound = false;
                        endFound = false;
                    }
                }
                k++;
            }
        } // end constructor

        public List<string> GetNames()
        {
            return names;
        }

        public List<string> GetPosters(string name)
        {
            List<string> value = new List<string>();
            for (int i = 0; i < names.Count; i++)
            {
                if (name == names[i])
                {
                    if(vals[i] != "null")
                    {
                        value.Add(vals[i].Substring(3, vals[i].Length - 4));
                    }
                    else
                    {
                        value.Add("");
                    }
                }
            }
            // trims ""
            return value;
        }

        public List<string> GetIds(string name)
        {
            List<string> value = new List<string>();
            for (int i = 1; i < names.Count; i++)
            {
                if (name == names[i])
                {
                    if(vals[i] != "null")
                        value.Add(vals[i]);
                }
            }
            // trims ""
            return value;
        }

        public List<string> GetDetails(string key)
        {
            List<string> value = new List<string>();
            for (int i = 0; i < names.Count; i++)
            {
                if (key == names[i])
                {
                    if (vals[i] == "nul" || vals[i] == "null" || vals[i] == "")
                    {
                        value.Add("null");
                    }
                    else
                    {
                        value.Add(vals[i].Substring(1, vals[i].Length - 2));
                    }
                }
            }
            // trims ""
            return value;
        }
    }// end class
}// end namespace