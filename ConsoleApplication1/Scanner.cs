using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace wallUpdate
{
    class Scanner
    {
        private String delimiter;
        private String text;
        private int stick = 0;
        public Scanner(String src) : this(src," ") {}
        public Scanner(String text, String delimiter)
        {
            this.text = text;
            this.delimiter = delimiter;
        }
        
        public void setDelimiter(string delimiter)
        {
            this.delimiter = delimiter;
        }
        public bool hasNext()
        {
            if (text.Substring(stick).Length > 0)
            {
                return true;
            }
            return false;
        }
        public String nextLine()
        {
            return nextGetter("\n");
        }
        public String next()
        {
            return nextGetter(delimiter);
        }
        public String[] LineArray()
        {
            ArrayList arr = new ArrayList();
            Scanner sub = new Scanner(text,delimiter);
            while (sub.hasNext())
            {
                arr.Add(sub.next());
            }
            String[] returnArr = new String[arr.Count];
            
            for (int i = 0; i < returnArr.Length; i++)
            {
                returnArr[i] = (String)arr[i];
            }

            return returnArr;
        }
        
        private String nextGetter(string d)
        {
            int carrot = (text.Substring(stick)).IndexOf(d)+stick;
            
            String returner;
            if (carrot<stick)
            {
                carrot = text.IndexOf("\n");
                if (carrot<stick)
                {
                    returner = text.Substring(stick);
                    stick = text.Length;
                }
                else
                {
                    returner = text.Substring(stick, carrot - stick);
                    stick = carrot+1;
                }
            }
            else
            {
                returner = text.Substring(stick, carrot-stick);
                stick = carrot+1;
            }
            
            return returner.Trim();
        }


    }
}
