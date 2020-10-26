using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Globalization;
using FANNCSharp.Double;
using UnityEngine.UI;
using System.IO;

namespace AI_FANN
{
    public class Fann : MonoBehaviour
    {
        public Text text1;
        public Text text2;
        public Text text3;

        public Text answer1;
        public Text answer2;
        public Text answer3;

        public NeuralNet ann;

        private void Start()
        {
            TrainFann();
            RunTests();
        }

        private void TrainFann()
        {
            string french_text = System.IO.File.ReadAllText(GetFullTextPathTraining("french.txt"));
            string english_text = System.IO.File.ReadAllText(GetFullTextPathTraining("english.txt"));
            string polish_text = System.IO.File.ReadAllText(GetFullTextPathTraining("polish.txt"));
            string espagnol_text = System.IO.File.ReadAllText(GetFullTextPathTraining("espagnol.txt"));
            string portuguese_text = System.IO.File.ReadAllText(GetFullTextPathTraining("portuguese.txt"));

            double[] french_frequencies = Frequencies(french_text);
            double[] english_frequencies = Frequencies(english_text);
            double[] polish_frequencies = Frequencies(polish_text);
            double[] espagnol_frequencies = Frequencies(espagnol_text);
            double[] portuguese_frequencies = Frequencies(portuguese_text);

            double[][] inputs = { french_frequencies, english_frequencies, polish_frequencies, espagnol_frequencies, portuguese_frequencies };
            double[][] outputs = { new double[] { 1, 0, 0, 0, 0 }, new double[] { 0, 1, 0, 0, 0 }, new double[] { 0, 0, 1, 0, 0 }, new double[] { 0, 0, 0, 1, 0 }, new double[] { 0, 0, 0, 0, 1 } };


            List<uint> layers = new List<uint>();
            layers.Add(26);
            layers.Add(10);
            layers.Add(5);

            NeuralNet network = new NeuralNet(FANNCSharp.NetworkType.LAYER, layers);

            TrainingData data = new TrainingData();
            data.SetTrainData(inputs, outputs);

            network.TrainOnData(data, 10000, 10, 0.0001f);

            ann = network;
        }

        private void RunTests()
        {
            List<string> list_tests = CreateTestSets();

            string test_text_1 = System.IO.File.ReadAllText(GetFullTextPathTest(list_tests[0]));
            string test_text_2 = System.IO.File.ReadAllText(GetFullTextPathTest(list_tests[1]));
            string test_text_3 = System.IO.File.ReadAllText(GetFullTextPathTest(list_tests[2]));

            SetStringToText(text1, test_text_1);
            SetStringToText(text2, test_text_2);
            SetStringToText(text3, test_text_3);

            double[] test_frequencies = Frequencies(test_text_1);
            double[] result = ann.Run(test_frequencies);

            answer1.text = ChooseLanguage(result) + "\n\n" + ResultToString(result);

            test_frequencies = Frequencies(test_text_2);
            result = ann.Run(test_frequencies);

            answer2.text = ChooseLanguage(result) + "\n\n" + ResultToString(result); ;

            test_frequencies = Frequencies(test_text_3);
            result = ann.Run(test_frequencies);

            answer3.text = ChooseLanguage(result) + "\n\n" + ResultToString(result); ;
        }

        static double[] Frequencies(string _text)
        {
            _text = new String(_text.ToLowerInvariant().Where(Char.IsLetter).ToArray());
            _text = _text.Normalize(NormalizationForm.FormD);
            _text = new string(_text.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());

            double[] frequencies = new double[26];

            foreach (char c in _text)
            {
                if (c - 'a' >= 0 && c - 'a' < frequencies.Length)
                    frequencies[c - 'a']++;
            }

            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] /= _text.Length;
            }
            return frequencies;
        }

        private string GetFullTextPathTraining(string _filename)
        {
            return Application.dataPath + "/AI/TP3-Fann/Materials/Sources-For-Training/" + _filename;
        }

        private string GetFullTextPathTest(string _filename)
        {
            return Application.dataPath + "/AI/TP3-Fann/Materials//Sources-For-Tests/" + _filename;
        }

        private void SetStringToText(Text _textui, string _text)
        {
            _textui.text = _text;
        }

        private string ChooseLanguage(double[] frequencies)
        {
            string res = "None";

            if(frequencies[0] > frequencies[1] && frequencies[0] > frequencies[2] && frequencies[0] > frequencies[3] && frequencies[0] > frequencies[4])
            {
                res = "French";
            }
            else if (frequencies[1] > frequencies[0] && frequencies[1] > frequencies[2] && frequencies[1] > frequencies[3] && frequencies[1] > frequencies[4])
            {
                res = "English";
            }
            else if (frequencies[2] > frequencies[0] && frequencies[2] > frequencies[1] && frequencies[2] > frequencies[3] && frequencies[2] > frequencies[4])
            {
                res = "Polish";
            }
            else if (frequencies[3] > frequencies[0] && frequencies[3] > frequencies[1] && frequencies[3] > frequencies[2] && frequencies[3] > frequencies[4])
            {
                res = "Espagnol";
            }
            else if (frequencies[4] > frequencies[0] && frequencies[4] > frequencies[1] && frequencies[4] > frequencies[2] && frequencies[4] > frequencies[3])
            {
                res = "Portuguese";
            }

            return res;
        }

        private string ResultToString(double[] _res)
        {
            return "French : " + Percentage(_res[0], 1) + "%" + " English : " + Percentage(_res[1], 1) + "%" + " Polish : " + Percentage(_res[2], 1) + "%";
        }

        private double Percentage(double _number, int _number_decimal)
        {
            return Math.Round(_number * 100, _number_decimal);
        }

        private List<string> CreateTestSets()
        {
            List<string> list_texts = new List<string>();
            foreach (FileInfo file in ((new DirectoryInfo(GetFullTextPathTest("")).GetFiles("*.txt"))))
            {
                list_texts.Add(file.Name);
            }

            Shuffle(list_texts);

            return list_texts;
        }

        private void Shuffle<T>(IList<T> _list)
        {
            int n = _list.Count;
            System.Random rng = new System.Random();

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = _list[k];
                _list[k] = _list[n];
                _list[n] = value;
            }
        }

        public void ResetFann()
        {
            RunTests();
        }
    }
}