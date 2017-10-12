using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InflateIt
{

    /*
Coding Exercise:
Write a program in C# that converts a sentence from English to Inflationary English. Inflationary English takes words and word parts that sound the same as a number (e.g. “won” v. “one”) 
     * and then inflates that to the next number (e.g. “won” becomes “two”). Provide tests for your program.
Example of input and output:
Input: “Today I won an award for being awesome.”
Output: “Threeday I two an award five being awesome."     
     
     */
    class Program
    {
        private static readonly string[][] _map = new string[][] 
        { 
            new string [] { "one", "two" }, 
            new string [] { "two", "three" },
            new string [] { "three", "four" },
            new string [] { "four", "five" },
            new string [] { "five", "six" },
            new string [] { "six", "seven" },
            new string [] { "seven", "eight" },
            new string [] { "eight", "nine" },
            new string [] { "nine", "ten" }        
        };

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine("Input Key: {0}", args[0]);
                Console.WriteLine("output Key: {0}", CreateInflationaryLine(args[0]));

                string strA = "Won";
                string strB = "one";
                Console.WriteLine("{0}, {1} -> {2}, {3}", strA, strB, SoundsEx(strA) , SoundsEx(strB));
            }
            else
            {
                Console.WriteLine("Please supply input string to be inflated");
            }
            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey();
        }

        static string CreateInflationaryLine(string input)
        {
            string strCurrent = "";
            string strOutput = "";

            foreach (char charCurrent in input)
            {
                strCurrent += charCurrent;
                if (!char.IsWhiteSpace(charCurrent))
                {
                    string strCurrentSoundEx = SoundsEx(strCurrent);
                    foreach (string[] mapItem in _map)
                    {
                        if ( strCurrentSoundEx == SoundsEx(mapItem[0]))
                        {
                            strCurrent = mapItem[1];
                            strOutput += strCurrent;
                            strCurrent = "";
                        }
                    }
                }
                else
                {
                    strOutput += strCurrent;
                    strCurrent = "";
                }
            }
            strOutput += strCurrent;
            return strOutput;
        }

        static int LevenshteinDistance(string strA, string strB)
        {
            int cost;

            if (strA.Length == 0) return strB.Length;
            if (strB.Length == 0) return strA.Length;


            if (strA[strA.Length - 1] == strB[strB.Length - 1])
                cost = 0;
            else
                cost = 1;

            return Math.Min(Math.Min(LevenshteinDistance(strA.Substring(0, strA.Length - 1), strB) + 1, LevenshteinDistance(strA, strB.Substring(0, strB.Length - 1)) + 1), LevenshteinDistance(strA.Substring(0, strA.Length - 1), strB.Substring(0, strB.Length - 1)) + cost);

            
        }

        static string SoundsEx(string input)
        {
            string output = "";
            input = input.ToLower();
            
            List<char> inputCharArray = new List<char>(input.ToCharArray());

            if (inputCharArray.Count > 0)
            {
                //Save the first letter. Remove all occurrences of 'h' and 'w' except first letter.
                output = inputCharArray[0].ToString();
                char firstChar = inputCharArray[0];
                inputCharArray[0] = '#';

                inputCharArray.Remove('h');
                inputCharArray.Remove('w');
                inputCharArray[0] = firstChar;
                
                //Replace all consonants (include the first letter) with digits as in [2.] above.
                for(int iIndex = 0; iIndex < inputCharArray.Count; iIndex++)
                {
                    switch(inputCharArray[iIndex])
                    {
                        case 'b':
                        case 'f':
                        case 'p':
                        case 'v':
                                inputCharArray[iIndex] = '1';
                            break;

                        case 'c':
                        case 'g':
                        case 'j':
                        case 'k':
                        case 'q':
                        case 's':
                        case 'x':
                        case 'z':
                                inputCharArray[iIndex] = '2';
                            break;

                        case 'd':
                        case 't':
                                inputCharArray[iIndex] = '3';
                            break;

                        case 'l':
                                inputCharArray[iIndex] = '4';
                            break;

                        case 'm':
                        case 'n':
                                inputCharArray[iIndex] = '5';
                            break;

                        case 'r':
                                inputCharArray[iIndex] = '6';
                            break;
                    }
                }

                
                //Replace all adjacent same digits with one digit.
                for (int iIndex = inputCharArray.Count - 1; iIndex > 0; iIndex--)
                {
                    int currentNum = 0;

                    if (inputCharArray[iIndex] == inputCharArray[iIndex - 1] && int.TryParse(inputCharArray[iIndex].ToString(), out currentNum))
                    {
                        inputCharArray.RemoveAt(iIndex);
                    }
                }

                //Remove all occurrences of a, e, i, o, u, y except first letter.
                for (int iIndex = inputCharArray.Count - 1; iIndex > 0; iIndex--)
                {
                    switch (inputCharArray[iIndex])
                    {
                        case 'a':
                        case 'e':
                        case 'i':
                        case 'o':
                        case 'u':
                        case 'y':
                            inputCharArray.RemoveAt(iIndex);
                            break;
                    }
                }

                //If first symbol is a digit replace it with letter saved on step 1.
                int num2;
                if (int.TryParse(inputCharArray[0].ToString(), out num2))
                {
                    inputCharArray[0] = output[0];
                }

                //Append 3 zeros if result contains less than 3 digits. Remove all except first letter and 3 digits after it (This step same as [4.] in explanation above).
                output = "";

                for (int iIndex = 0; iIndex < inputCharArray.Count; iIndex++ )
                {
                    output += inputCharArray[iIndex];
                    
                }

                output = output.PadRight(4, '0').Substring(0, 4);
                
            }

            return output;
        }
    }
}
