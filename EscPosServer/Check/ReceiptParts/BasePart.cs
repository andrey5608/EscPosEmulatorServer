using System.Collections.Generic;
using ESCPOS_NET.Emitters;

namespace EscPosServer.Check.ReceiptParts
{
    public delegate byte[] GetBytesFromEmitter(ICommandEmitter emitter);

    public abstract class BasePart
    {
        protected static readonly Dictionary<Align, GetBytesFromEmitter> AlignMethodsMap = new()
        {
            {Align.Center, e => e.CenterAlign()},
            {Align.Left, e => e.LeftAlign()},
            {Align.Right, e => e.RightAlign()},
        };

        protected readonly ICommandEmitter Emitter;
        protected readonly List<byte> PartBytes = new();

        private readonly char _spaceCharacter;
        private readonly int _width;

        protected BasePart(ICommandEmitter emitter, char spaceCharacter = '-', int width = 40)
        {
            Emitter = emitter;
            _spaceCharacter = spaceCharacter;
            _width = width;
        }

        public byte[] GetBytes()
        {
            if (PartBytes.Count == 0)
            {
                GetBytesInternal();
                var separator = string.Empty.PadLeft(_width, _spaceCharacter);
                PartBytes.AddRange(Emitter.PrintLine(separator));
            }

            return PartBytes.ToArray();
        }

        protected abstract void GetBytesInternal();
    }
}