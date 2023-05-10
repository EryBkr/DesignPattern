using System.Drawing;
using System.IO;

namespace BaseProject.Services.Adapter
{

    public interface IAdvanceImageProcess
    {
        //void AddWaterMark(string text, string fileName, Stream imageStream);

        void AddWatermarkImage(Stream stream, string text, string filePath, Color color, Color outlineColor);
    }
}
