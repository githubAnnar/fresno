using LanterneRouge.Fresno.Calculations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class StepTestsPlotViewModel : WorkspaceViewModel
    {
        #region Fields

        private const string LoadAxisKey = "Load";
        private const string LactateAxisKey = "Lactate";
        private const string HeartrateAxisKey = "Heartrate";

        #endregion

        #region Constructors

        public StepTestsPlotViewModel(IEnumerable<StepTestViewModel> stepTests)
        {
            StepTestData = stepTests.ToList();
            var testPlotType = stepTests.Count() > 1 ? "Comparison" : "Plot";
            StepTestsModel = new PlotModel { Title = $"{testPlotType} of user {string.Join(", ", stepTests.Select(st => st.Source.ParentUser.LastName).Distinct())}", LegendPosition= LegendPosition.LeftTop };
            var minLoad = stepTests.Select(st => st.Source.Measurements.Min(m => m.Load)).Min();
            var maxLoad = stepTests.Select(st => st.Source.Measurements.Max(m => m.Load)).Max();
            var minLactate = stepTests.Select(st => st.Source.Measurements.Min(m => m.Lactate)).Min();
            var maxLactate = stepTests.Select(st => st.Source.Measurements.Max(m => m.Lactate)).Max();
            var minHeartrate = stepTests.Select(st => st.Source.Measurements.Min(m => m.HeartRate)).Min();
            var maxHearerate = stepTests.Select(st => st.Source.Measurements.Max(m => m.HeartRate)).Max();
            StepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = minLoad - 10, Maximum = maxLoad + 10, Key = LoadAxisKey, Title = "Load [W]" });

            StepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minLactate - 1, Maximum = maxLactate + 1, Key = LactateAxisKey, Title = "Lactate [mol/ml]" });
            StepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Minimum = minHeartrate - 10, Maximum = maxHearerate + 10, Key = HeartrateAxisKey, Title = "HR [bpm]" });

            foreach (var stepTest in StepTestData)
            {
                var calc = new FblcCalculation(stepTest.Source.Measurements, 4.0);
                var minX = stepTest.Source.Measurements.Min(m => m.Load) - 10;
                var maxX = stepTest.Source.Measurements.Max(m => m.Load) + 10;
                StepTestsModel.Series.Add(new FunctionSeries(calc.FittedLactateCurve, minX, maxX, 20, $"LaF {stepTest.TestDate.ToShortDateString()}") { YAxisKey = LactateAxisKey, XAxisKey = LoadAxisKey });

                var lactateScatterSeries = new ScatterSeries { YAxisKey = LactateAxisKey, XAxisKey = LoadAxisKey, Title = $"LaM {stepTest.TestDate.ToShortDateString()}" };
                var heartRateSeries = new LineSeries { YAxisKey = HeartrateAxisKey, XAxisKey = LoadAxisKey, Title = $"HR {stepTest.TestDate.ToShortDateString()}" };

                foreach (var item in stepTest.Source.Measurements)
                {
                    heartRateSeries.Points.Add(new DataPoint(item.Load, item.HeartRate));
                    lactateScatterSeries.Points.Add(new ScatterPoint(item.Load, item.Lactate));
                }

                StepTestsModel.Series.Add(heartRateSeries);
                StepTestsModel.Series.Add(lactateScatterSeries);
            }
        }

        #endregion

        #region Properties

        private List<StepTestViewModel> StepTestData { get; }

        public PlotModel StepTestsModel { get; private set; }

        #endregion

        #region WorkspaceViewModel implementation

        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Display Properties

        public override string DisplayName => $"Step Test plot";

        #endregion
    }
}
