using ESCPOS_NET.Emitters;

namespace EscPosServer.Check.ReceiptParts
{
    public class WelcomePart : TextPart
    {
        public WelcomePart(ICommandEmitter emitter, string company, string address, string phone) :
            this(emitter, string.Empty, company, address, phone)
        {
        }

        public WelcomePart(ICommandEmitter emitter, string welcomeWords, string company, string address, string phone)
            : base(emitter, Align.Center, new[] {welcomeWords, company, address, phone})
        { 
        }
    }
}