using System.Windows.Media.Imaging;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class VisionResult
    {
        public BitmapImage Image { get; set; }
        public bool Passed { get; set; }
    }
}
