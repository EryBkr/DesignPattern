using System.Drawing;
using System.IO;

namespace BaseProject.Services.Adapter
{
    //Bu arkadaş aslında adından da anlaşılacağı gibi dönüşümü arka tarafta gerçekleştiren bir class,şöyle ki; başka bir kütüphane ya da api kullanıyoruz ve daha sonra bu library ya da api de değişiklik yapmamız gerekti,bu değişikliği her yerde uygulamak yerine (ki çok fazla yerde de kullanmış olabiliriz) başka bir class ta yapıp
    public class AdvanceImageProcessAdapter : IImageProcess
    {
        private readonly IAdvanceImageProcess _advanceImageProcess;

        public AdvanceImageProcessAdapter(IAdvanceImageProcess advanceImageProcess)
        {
            _advanceImageProcess = advanceImageProcess;
        }

        public void AddWaterMark(string text, string fileName, Stream imageStream)
        {
            _advanceImageProcess.AddWatermarkImage(imageStream, text, $"wwwroot/watermarks/{fileName}", Color.FromArgb(128, 255, 255, 255), Color.FromArgb(0, 255, 255, 255));
        }
    }
}
