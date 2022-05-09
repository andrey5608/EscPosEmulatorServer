using System.Collections.Generic;
using System.IO;
using System.Linq;
using EscPosServer.Check.ReceiptParts;

namespace EscPosServer.Check
{
    public class  EscPosCheck
    {
        private readonly List<BasePart> _checkParts;
        private byte[] _checkBytes;
        
        public EscPosCheck(List<BasePart> checkParts)
        {
            _checkParts = checkParts;
        }

        public byte[] GetBytes()
        {
            return _checkBytes ??= _checkParts.SelectMany(part => part.GetBytes()).ToArray();
        }
    }
}