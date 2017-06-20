using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kinteraction.Frames
{
    /// <summary>
    ///     Interaction logic for KinectViewer.xaml
    /// </summary>
    public partial class KinectViewer : UserControl
    {
        public readonly IDictionary<BrushType, Brush> BrushTypes = new Dictionary<BrushType, Brush>
        {
            {BrushType.LightCyan, new SolidColorBrush(Colors.LightCyan)},
            {BrushType.LightCoral, new SolidColorBrush(Colors.LightCoral)},
            {BrushType.LightGreen, new SolidColorBrush(Colors.LightGreen)},
            {BrushType.LightSkyBlue, new SolidColorBrush(Colors.LightSkyBlue)},
            {BrushType.LightPink, new SolidColorBrush(Colors.LightPink)},
            {BrushType.LightSeaGreen, new SolidColorBrush(Colors.LightSeaGreen)}
        };

        public KinectViewer()
        {
            InitializeComponent();
        }
    }

    public enum BrushType
    {
        LightCyan = 0,
        LightCoral,
        LightGreen,
        LightSkyBlue,
        LightPink,
        LightSeaGreen
    }
}