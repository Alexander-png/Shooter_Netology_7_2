using System.Text.RegularExpressions;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Base
{
    public static class PluralHelper
    {
        private static readonly Regex PluralsExpr = new Regex("(\\[(\\d+-?\\d*:\\w*,?)+\\])");

        public static string GetPlural(string key, int quantity = 1)
        {
            string term = LocalizationCore.GetTerm(key);
            int qty = CalculatePluralQuantity(quantity);
            return PluralsExpr.Replace(term, x => PluaralReplacer(x, qty));
        }

        private static int CalculatePluralQuantity(int quantity)
        {
            int qty = Mathf.Abs(quantity) % 100;
            if (qty == 0 && quantity > 0)
            {
                qty = 100;
            }
            else
            {
                qty = qty < 20 ? qty : qty % 10;
                if (qty == 0 && quantity > 0)
                {
                    qty = 20;
                }
            }
            return qty;
        }

        private static string PluaralReplacer(Capture x, int qty)
        {
            string pluralGroup = x.Value.Trim('[', ']');
            string[] variants = pluralGroup.Split(',');

            foreach (var variant in variants)
            {
                string[] data = variant.Split(':');
                PluralRange range = new PluralRange(data[0]);
                if (range.InRange(qty))
                {
                    return data[1];
                }
            }
            return null;
        }

        private class PluralRange
        {
            private readonly int _max;
            private readonly int _min;

            public PluralRange(string range)
            {
                string[] values = range.Split('-');
                _min = int.Parse(values[0]);
                if (values.Length == 2)
                {
                    var secondValue = values[1];
                    _max = string.IsNullOrEmpty(secondValue) ? int.MaxValue : int.Parse(secondValue);
                }
            }

            public bool InRange(int value) 
                => value >= _min && value <= _max;
        }
    }
}
