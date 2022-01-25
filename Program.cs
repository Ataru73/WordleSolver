#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

namespace Wordle
{
    class Program
    {
        enum State
        {
            NOT_PRESENT, PRESENT, CORRECT
        }

        class Frequency : IEquatable<Frequency>, IComparable<Frequency> {
            public char charachter;
            public double frequency;
            public Frequency(char c, double v)
            {
                this.charachter = c;
                this.frequency = v;
            }

            public int CompareTo(Frequency? other)
            {
                if (other == null) 
                    return 0;
                else if (this.frequency > other.frequency)
                    return -1;
                else
                    return 1;
            }

            public bool Equals(Frequency? other)
            {
                if (other == null) return false;
                return this.frequency == other.frequency;
            }
        }

        class WordCost : IComparable<WordCost>, IEquatable<WordCost> {
            public string word;
            public double cost;

            public WordCost(string w, double v)
            {
                this.word = w;
                this.cost = v;
            }

            public int CompareTo(WordCost? other)
            {
                if (other == null) return 0;
                if (other.cost > this.cost) return 1;
                else if (other.cost < this.cost) return -1;
                else return 0;
            }

            public bool Equals(WordCost? other)
            {
                if (other == null) return false;
                if (other.cost == this.cost) return true;

                return false;
            }
        }
        class Hint
        {
            public string word;
            public  State[] states;

            public Hint()
            {
                word = "";
                states = new State[WORD_LENGTH];
            }

            public bool isEqual(State[] states)
            {
                bool retVal = true;

                for (int i = 0; i < WORD_LENGTH; i++)
                {
                    retVal &= states[i] == this.states[i];
                }

                return retVal;
            }
            
        }

        private const int WORD_LENGTH = 5;

        static List<string> wordList = new List<string>();
        static List<Hint> hints = new List<Hint>();
        static void Main(string[] args)
        {
            List<WordCost> sortedWords = new List<WordCost>();

            var listFile = args[0];
            var hintFile = args[1];

            using (var stream = File.OpenText(listFile)) {
                string? line = "";
                while(line != null)
                {
                    line = stream.ReadLine();
                    if (line != null && line != "" && line.Length == WORD_LENGTH && !line.StartsWith("#") && isValid(line))
                    {
                        wordList.Add(line.ToLower());
                    }
                }
            }

            
            // foreach (var w in wordList)
            // {
            //     sortedWords.Add(new WordCost(w, Cost(w)));
            // }

            // sortedWords.Sort();

            // wordList.Clear();

            // foreach (var sw in sortedWords)
            // {
            //     wordList.Add(sw.word);
            // }

            using (var stream = File.OpenText(hintFile)) {
                string? line = "";
                while (line != null)
                {
                    line = stream.ReadLine();
                    if (line != null && line != "")
                    {
                        Hint hint = new Hint();
                        string[] tokens = line.Split(": ");

                        hint.word = tokens[0];

                        string[] states = tokens[1].Split(" ");
                        for (var i = 0; i < WORD_LENGTH; i++)
                        {
                            switch(states[i])
                            {
                                case "0":
                                    hint.states[i] = State.NOT_PRESENT;
                                    break;
                                case "1":
                                    hint.states[i] = State.PRESENT;
                                    break;
                                case "2":
                                    hint.states[i] = State.CORRECT;
                                    break;
                            }
                        }

                        hints.Add(hint);
                    }
                }
            }

            string word = "";
            int index = 0;
            var start = DateTime.Now.Ticks;

            List<string> possibleWords = new List<string>();

            foreach (var w in wordList)
            {
                if (isPossible(w)) {
                    possibleWords.Add(w);
                    index++;
                }
            }

            var end = DateTime.Now.Ticks;

            Console.WriteLine("Most likely word: " + possibleWords[0]);
            Console.Write("Other possibilities: ");
            for (var i = 1; i < 11; i++) {
                if (possibleWords.Count >= i + 1) {
                    Console.Write(possibleWords[i] + ", ");
                }
                else 
                    break;
            }

        }

        private static bool isValid(string line)
        {
            bool valid = true;

            foreach (var c in line.ToCharArray())
            {
                valid &= c >= 'a' && c <= 'z';
            }

            return valid;
        }

        private static bool isPossible(string word)
        {
            if (word.Length != WORD_LENGTH) return false;
            var result = true;

            foreach (var hint in hints)
            {
                State[] states = Check(hint.word, word);
                result &= hint.isEqual(states);

            }

            return result;
        }

        private static State[] Check(string word1, string word2)
        {
            
            State[] retVal = new State[WORD_LENGTH];
            Dictionary<char, State> charStates = new Dictionary<char, State>();


            char[] test = word1.ToCharArray();
            char[] goal = word2.ToCharArray();

            for (int i = 0; i < WORD_LENGTH; i++)
            {
                if (test[i] == goal[i])
                {
                    retVal[i] = State.CORRECT;
                    charStates[test[i]] = State.CORRECT;
                }
                else if (Contains(goal, test[i]))
                {
                    if (!charStates.ContainsKey(test[i])) {
                        charStates[test[i]] = State.PRESENT;
                        retVal[i] = State.PRESENT;
                    }
                    else {
                        retVal[i] = State.NOT_PRESENT;
                    }
                }
                else
                {
                    retVal[i] = State.NOT_PRESENT;
                    if (!charStates.ContainsKey(test[i])) {
                        charStates[test[i]] = State.NOT_PRESENT;
                    }
                }
            }

            for (var i = 0; i < WORD_LENGTH; i++) {
                if (retVal[i] == State.PRESENT && charStates[test[i]] == State.CORRECT) {
                    retVal[i] = State.NOT_PRESENT;
                }
            }
            return retVal;
        }

        private static bool Contains(char[] goal, char v)
        {
            for (var i = 0; i < goal.Length; i++)
            {
                if (goal[i] == v) return true;
            }

            return false;
        }
    }
}
