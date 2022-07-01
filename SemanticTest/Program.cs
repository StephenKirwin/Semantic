using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SemanticTest
{
    public class FrequencyBin
    {
        public float Lower { get; set; }
        public float Upper { get; set; }
        bool _end;
        int Matches = 0;

        public FrequencyBin()
        {//the JSON deserialisation requres a parameterless constructor
        }

        public FrequencyBin(float l, float u, bool e, int m)
        {//For dependency injection
            Lower = l;
            Upper = u;
            _end = e;
            Matches = m;
        }

        public int GetMatches()
        {
            return Matches;
        }

        public void ResetMatches()
        {
            Matches = 0;
        }

        public void SetEnd()
        {
            _end = true;
        }

        public bool TestNumber(float f)
        {
            //check if the number falls within the range, if it does return true and increment matches
            if (f >= Lower && (_end? f <= Upper : f < Upper))
            {
                Matches += 1;
                return true;
            }
            //else return false
            return false;
        }

        public string DisplayText()
        {
            //format a string to show the bounds and matches neatly
            return string.Format("{0} - {1} : {2}", new object[] { Lower, Upper, Matches });
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {

            var frequencyBins = GatherFrequencyBins(args[1]);
            var numbers = LoadNumbersFromFile(args[0]);

            //only compute if there are numbers in the file
            if (numbers.Length > 0)
            {
                Console.WriteLine("Total Number Count  : {0}", numbers.Length);
                Console.WriteLine("We have a mean of   : {0}", Mean(numbers));
                Console.WriteLine("We have a St.Dev of : {0}  - note : I used a divisor of N as this is a population instead of a sample", StandardDeviation(numbers));
                FrequencyDistribution(numbers, frequencyBins);
                if (frequencyBins.Count > 0)
                {
                    DisplayFrequencyDistribution(frequencyBins);
                }
                else
                {
                    //we have no frequency bins, make this known
                    Console.WriteLine("The config file specified {0}, has no frequency bins", args[1]);
                }
                foreach (FrequencyBin f in frequencyBins)
                {
                    f.ResetMatches();//means the object can be used again if need be
                }
            }
            else
            {
                //we were given an empty list, make this known
                Console.WriteLine("Empty List");
            }
        }

        public static List<FrequencyBin> GatherFrequencyBins(string configFile)
        {
            //load the JSON data and deserialize into bins
            var frequencyBins = JsonSerializer.Deserialize<List<FrequencyBin>>(File.ReadAllText(configFile));
            //if theres more than one set the end bin's upper limit to be inclusive - way easier than doing (less than the smallest number above 100 probably like 100.0000000000000000125 or something silly)
            if (frequencyBins.Count > 0)
            {
                frequencyBins.Last().SetEnd();
            }
            return frequencyBins;
        }

        public static float Mean(float[] numbers)
        {
            float total = 0;
            //compute a sum of all numbers
            foreach (float num in numbers)
            {
                total += num;
            }
            //compute the sum into a mean by dividing by the number of elements
            return total / numbers.Length;
        }

        public static float StandardDeviation(float[] numbers)
        {
            //get the mean using our function
            var m = Mean(numbers);
            float SODS = 0;
            //add to the Sum of Difference Squared
            foreach (float num in numbers)
            {
                SODS += (num - m) * (num - m);
            }
            return (float)Math.Sqrt(SODS / numbers.Length);//I have used N for the Standard Deviation Denominator as we have not taken a sampple of the "population"
        }

        static void DisplayFrequencyDistribution(List<FrequencyBin> frequencyBins)
        {
            //display each of the frequency bins bounds and the number of matches it has found
            foreach (FrequencyBin f in frequencyBins)
            {
                Console.WriteLine(f.DisplayText());
            }
        }

        public static void FrequencyDistribution(float[] numbers, List<FrequencyBin> frequencyBins)
        {
            //for each number, test if it fits into the bins
            foreach (float num in numbers)
            {
                foreach(FrequencyBin f in frequencyBins)
                {
                    f.TestNumber(num);
                }
            }
            
        }

        public static float[] LoadNumbersFromFile(string fileLocation)
        {
            //load the data from the file
            var fileData = File.ReadAllText(fileLocation);
            //if this is blank return an empty set
            if (fileData == "")
            {
                return new float[0];
            }
            //if this is not blank turn the data into an array of floats (delimit by a comma)
            return fileData.Split(',').Select(a => float.Parse(a)).ToArray();
        }
    }
}
