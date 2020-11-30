using System;
using QRCoder;

namespace Whyvra.Tunnel.Presentation.Services
{
    public class QrCodeService
    {
        private readonly QRCodeGenerator _generator;

        public QrCodeService(QRCodeGenerator generator)
        {
            _generator = generator;
        }

        public string RenderQrCode(string text)
        {
            var data = _generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q, forceUtf8: true);
            var code =  new PngByteQRCode(data);
            var encoded = code.GetGraphic(20);

            return $"data:image/png;base64, {Convert.ToBase64String(encoded)}";
        }
    }
}