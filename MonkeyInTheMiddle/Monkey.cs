using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MonkeyInTheMiddle
{
    public class Monkey
    {
        public int Identifier { get; set; }
        public Queue<Item> Items { get; set; }
        public double InspectionsCount { get; set; }
        public List<string> OperationTokens { get; set; }
        public int TestCondition { get; set; }
        public Dictionary<string, int> PossibleOutcomes { get; set; }

        public Monkey()
        {
            Items = new Queue<Item>();
            OperationTokens = new List<string>();
            PossibleOutcomes = new Dictionary<string, int>();
        }

        public void ParseInput(string[] input)
        {
            ParseIdentifier(input[0]);
            ParseItems(input[1]);
            ParseOperationTokens(input[2]);
            ParseTestCondition(input[3]);
            ParsePossibleOutcome(input[4]);
            ParsePossibleOutcome(input[5]);
        }

        private void ParseIdentifier(string rawIdentifierRow)
        {
            var regex = new Regex("Monkey ([0-9]+)");
            var matchGroups = regex.Match(rawIdentifierRow).Groups.Values.ToList();

            var monkeyIdentifier = int.Parse(matchGroups[1].Value);
            Identifier = monkeyIdentifier;
        }

        private void ParseItems(string rawItemsRow)
        {
            var regex = new Regex("([0-9]+)");
            var matches = regex.Matches(rawItemsRow).ToList();

            for (int i = 0; i < matches.Count; i++)
            {
                Items.Enqueue(new Item(int.Parse(matches[i].Value)));
            }
        }

        private void ParseOperationTokens(string rawOperationsRow)
        {
            var regex = new Regex("= (.*)");
            var matchGroups = regex.Match(rawOperationsRow).Groups.Values.ToList();

            OperationTokens.AddRange(matchGroups[1].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries));
        }

        private void ParseTestCondition(string rawTestConditionRow)
        {
            var regex = new Regex("by ([0-9]+)");
            var matchGroups = regex.Match(rawTestConditionRow).Groups.Values.ToList();

            TestCondition = int.Parse(matchGroups[1].Value);
        }

        private void ParsePossibleOutcome(string rawPossibleOutcomeRow)
        {
            var regex = new Regex("If (.*): throw to monkey ([0-9]+)");
            var matchGroups = regex.Match(rawPossibleOutcomeRow).Groups.Values.ToList();

            PossibleOutcomes.Add(matchGroups[1].Value, int.Parse(matchGroups[2].Value));
        }

        public Queue<(Item item, int monkeyIdentifier)> ProcessItems(bool reliefIsModulo, int reliefValue)
        {
            var processedItems = new Queue<(Item item, int monkeyIdentifier)>();

            while (Items.Any())
            {
                var item = Items.Dequeue();
                ProcessOperation(item);
                ProcessRelief(item, reliefIsModulo, reliefValue);

                var testOutcome = RetrieveTestOutcome(item);

                processedItems.Enqueue((item, testOutcome));

                InspectionsCount++;
            }

            return processedItems;
        }

        private void ProcessOperation(Item item)
        {
            var firstOperand = OperationTokens[0] == "old" ? item.WorryLevel : double.Parse(OperationTokens[0]);
            var secondOperand = OperationTokens[2] == "old" ? item.WorryLevel : double.Parse(OperationTokens[2]);
            var operation = GetOperation(OperationTokens[1]);

            item.WorryLevel = operation(firstOperand, secondOperand);
        }

        private Func<double, double, double> GetOperation(string operation)
        {
            return operation switch
            {
                "*" => (a, b) => { return a * b; },
                "+" => (a, b) => { return a + b; }
            };
        }

        private void ProcessRelief(Item item, bool reliefIsModulo, int reliefValue)
        {
            if (!reliefIsModulo)
            {
                item.WorryLevel = (int)Math.Floor((decimal)(item.WorryLevel / reliefValue));
            }
            else
            {
                item.WorryLevel = item.WorryLevel % reliefValue;
            }
        }

        private int RetrieveTestOutcome(Item item)
        {
            var isDivisible = item.WorryLevel % TestCondition == 0;
            return PossibleOutcomes[isDivisible.ToString().ToLower()];
        }
    }
}
