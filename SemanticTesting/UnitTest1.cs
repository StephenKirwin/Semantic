using Microsoft.VisualStudio.TestTools.UnitTesting;
using SemanticTest;
using System.Collections.Generic;

namespace SemanticTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FrequencyBinMatchNonEnd()
        {
            FrequencyBin f = new FrequencyBin(10, 20, false, 0);

            var match = f.TestNumber(15);
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void FrequencyBinMatchOnStartNonEnd()
        {
            FrequencyBin f = new FrequencyBin(10, 20, false, 0);

            var match = f.TestNumber(10);
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void FrequencyBinMatchEnd()
        {
            FrequencyBin f = new FrequencyBin(10, 20, true, 0);

            var match = f.TestNumber(20);
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void FrequencyBinNonMatchNonEnd()
        {
            FrequencyBin f = new FrequencyBin(10, 20, false, 0);

            var match = f.TestNumber(9);
            Assert.IsFalse(match);
        }

        [TestMethod]
        public void FrequencyBinNonMatchOnEdgeNonEnd()
        {
            FrequencyBin f = new FrequencyBin(10, 20, false, 0);

            var match = f.TestNumber(20);
            Assert.IsFalse(match);
        }

        [TestMethod]
        public void FrequencyBinCorrectDisplayNonZero()
        {
            FrequencyBin f = new FrequencyBin(10, 20, false, 9);

            var display = f.DisplayText();
            Assert.AreEqual(display, "10 - 20 : 9");
        }

        [TestMethod]
        public void FrequencyBinCorrectDisplayZero()
        {
            FrequencyBin f = new FrequencyBin(0, 15, false, 0);

            var display = f.DisplayText();
            Assert.AreEqual(display, "0 - 15 : 0");
        }

        [TestMethod]
        public void FrequencyBinSetEnd()
        {
            FrequencyBin f = new FrequencyBin(10, 20, false, 0);

            f.SetEnd();
            var match = f.TestNumber(20);
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void TestMean1()
        {
            float[] numbers = new float[] { 3, 5, 7, 9, 11 };
            var result = SemanticTest.Program.Mean(numbers);
            Assert.AreEqual(result, 7);
        }

        [TestMethod]
        public void TestMean2()
        {
            float[] numbers = new float[] { 0, 2, 4, 6, 8 };
            var result = SemanticTest.Program.Mean(numbers);
            Assert.AreEqual(result, 4);
        }

        [TestMethod]
        public void TestStandardDeviation1()
        {
            float[] numbers = new float[] { 10, 12, 23, 23, 16, 23, 21, 16 };
            var result = SemanticTest.Program.StandardDeviation(numbers);
            Assert.AreEqual(result, 4.8989794855664f);
        }

        [TestMethod]
        public void TestStandardDeviation2()
        {
            float[] numbers = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 100 };
            var result = SemanticTest.Program.StandardDeviation(numbers);
            Assert.AreEqual(result, 28.605069480776f);
        }

        [TestMethod]
        public void TestFileLoad1()
        {
            var numbers = SemanticTest.Program.LoadNumbersFromFile("TestSet1.csv");
            Assert.AreEqual(numbers[0], 0);
            Assert.AreEqual(numbers[1], 1);
            Assert.AreEqual(numbers[2], 2);
            Assert.AreEqual(numbers[3], 3);
            Assert.AreEqual(numbers[4], 4);
        }

        [TestMethod]
        public void TestFileLoad2()
        {
            var numbers = SemanticTest.Program.LoadNumbersFromFile("TestSet2.csv");
            Assert.AreEqual(numbers[0], 5);
            Assert.AreEqual(numbers[1], 6);
            Assert.AreEqual(numbers[2], 7);
            Assert.AreEqual(numbers[3], 8);
            Assert.AreEqual(numbers[4], 100);
        }

        [TestMethod]
        public void TestFileLoadEmpty()
        {
            var numbers = SemanticTest.Program.LoadNumbersFromFile("TestSetEmpty.csv");

            Assert.AreEqual(numbers.Length, 0);
        }

        [TestMethod]
        public void TestConfigLoad1()
        {
            var bins = SemanticTest.Program.GatherFrequencyBins("ConfigTest1.json");
            Assert.AreEqual(bins[0].Lower, 0);
            Assert.AreEqual(bins[0].Upper, 10);
            Assert.AreEqual(bins[1].Lower, 10);
            Assert.AreEqual(bins[1].Upper, 20);
            Assert.AreEqual(bins[2].Lower, 20);
            Assert.AreEqual(bins[2].Upper, 30);
        }

        [TestMethod]
        public void TestConfigLoadEmpty()
        {
            var bins = SemanticTest.Program.GatherFrequencyBins("ConfigEmpty.json");
            Assert.AreEqual(bins.Count, 0);
        }

        [TestMethod]
        public void TestFrequencyDistribution1()
        {
            float[] numbers = new float[] { 5, 11, 14, 21 };

            List<FrequencyBin> frequencyBins = new List<FrequencyBin>() { new FrequencyBin(0, 10, false, 0), new FrequencyBin(10, 20, false, 0), new FrequencyBin(20, 30, false, 0) };

            SemanticTest.Program.FrequencyDistribution(numbers, frequencyBins);

            Assert.AreEqual(frequencyBins[0].GetMatches(), 1);
            Assert.AreEqual(frequencyBins[1].GetMatches(), 2);
            Assert.AreEqual(frequencyBins[2].GetMatches(), 1);
        }

        [TestMethod]
        public void TestFrequencyDistribution2()
        {
            float[] numbers = new float[] { 5, 11, 14 };
            List<FrequencyBin> frequencyBins = new List<FrequencyBin>() { new FrequencyBin(0, 10, false, 0), new FrequencyBin(10, 20, false, 0), new FrequencyBin(20, 30, false, 0) };

            SemanticTest.Program.FrequencyDistribution(numbers, frequencyBins);

            Assert.AreEqual(frequencyBins[0].GetMatches(), 1);
            Assert.AreEqual(frequencyBins[1].GetMatches(), 2);
            Assert.AreEqual(frequencyBins[2].GetMatches(), 0);
        }

        [TestMethod]
        public void TestResetMatches()
        {
            FrequencyBin f = new FrequencyBin(0, 10 ,false, 100);
            f.ResetMatches();

            Assert.AreEqual(f.GetMatches(), 0);
        }
    }
}
