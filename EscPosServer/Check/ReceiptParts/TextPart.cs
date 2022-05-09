using System.Collections.Generic;
using ESCPOS_NET.Emitters;

namespace EscPosServer.Check.ReceiptParts
{
    public class TextPart : BasePart
    {
        private readonly string[] _lines;
        private readonly Align _alignment;

        public TextPart(ICommandEmitter emitter, Align alignment, string[] lines) : base(emitter)
        {
            _alignment = alignment;
            _lines = lines;
        }

        protected override void GetBytesInternal()
        {

            var alignMethod = AlignMethodsMap[_alignment];
            PartBytes.AddRange(alignMethod(Emitter));

            foreach (var line in _lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    PartBytes.AddRange(Emitter.PrintLine(line));
            }
        }
    }
}