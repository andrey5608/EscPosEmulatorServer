using System;
using System.Collections.Generic;
using ESCPOS_NET.Emitters;
using EscPosServer.Check.ReceiptParts;

namespace EscPosServer.Check.ExampleChecks
{
    public class BarCheck : EscPosCheck
    {
        private static readonly WelcomePart Welcome = new(Program.CommandEmitter,
            "Gotfrid's",
            "Sovietskaya street, 84",
            "+7 (342) 123-45-67");

        private static TextPart CheckInfo => new(Program.CommandEmitter, Align.Left,
            new[]
            {
                $"Pre-check №    045      {DateTime.Now}",
                $"Check №        022      ",
                $"Table k        2",
                $"Waiter         John Johnson",
                $"Check open     {DateTime.Now - TimeSpan.FromHours(1)}",
                $"Check closed   {DateTime.Now}",
            });

        private static TextPart Receipt => new(Program.CommandEmitter, Align.Center,
            new[]
            {
                "Item              Price  Quant.  T.Price",
                "----------------------------------------",
                "French 75           380       1      380",
                "Whiskey Sour        420     3x1     1260",
                "Snow Hill           420     3x1     1260",
                "Almond Rooibos      200       1      200",
                "Hookah             1000     3x1     3000",
                "John Collins        350       1      350",
                "Margarita           420     5x1     2100",
                "Knickerbocker       420       1      420",
                "Mai Tai             550       1      550",
                "Fire Cherry         200       1      200",
                "Salad with chicken  380       1      380",
                "and mushrooms",
                "Special order         1   900x1      900",
                "Quesadilla          490       1      490",
                "Shrimp salad        640       1      640",
                "Cowberry cc.        350       1      350",
                "Last Word           520       1      520",
                "Bruschetta with     590       1      590",
                "salmon",
                "Gimlet              350       1      350",
                "Blood and Sand      450       1      450",
                "Pasta with pesto    480       1      480",
                "and chicken",
                "Anchel              420       1      420",
                "Pasta with veal     580       1      580",
                "Spinach soup        320       1      320",
                "Voyage              420       1      420",
                "----------------------------------------",
                "Subtotal                           17820",
                "Discount                           -1482",
                "----------------------------------------",
                "Total                              16338",
            });

        public BarCheck() : base(new List<BasePart> { Welcome, CheckInfo, Receipt })
        {
        }
    }
}