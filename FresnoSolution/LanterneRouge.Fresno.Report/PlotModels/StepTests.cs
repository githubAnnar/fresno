using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Report.PlotModels
{
    public static class StepTests
    {
        #region Fields

        private const string LoadAxisKey = "Load";
        private const string LactateAxisKey = "Lactate";
        private const string HeartrateAxisKey = "Heartrate";

        #endregion

        public static PlotModel StepTestPlotModel(IEnumerable<StepTest> stepTests)
        {
            if (stepTests is null)
            {
                throw new ArgumentNullException(nameof(stepTests));
            }

            var stepTestData = stepTests.ToList();
            var testPlotType = stepTests.Count() > 1 ? "Comparison" : "Plot";
            var stepTestsModel = new PlotModel { Title = $"{testPlotType} of user {string.Join(", ", stepTests.Select(st => ((User)st.ParentUser).LastName).Distinct())}" };
            var minLoad = stepTests.Select(st => st.Measurements.Min(m => m.Load)).Min();
            var maxLoad = stepTests.Select(st => st.Measurements.Max(m => m.Load)).Max();
            var minLactate = stepTests.Select(st => st.Measurements.Min(m => m.Lactate)).Min();
            var maxLactate = stepTests.Select(st => st.Measurements.Max(m => m.Lactate)).Max();
            var minHeartrate = stepTests.Select(st => st.Measurements.Min(m => m.HeartRate)).Min();
            var maxHearerate = stepTests.Select(st => st.Measurements.Max(m => m.HeartRate)).Max();
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = minLoad - 10, Maximum = maxLoad + 10, Key = LoadAxisKey, Title = "Load [W]" });
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minLactate - 1, Maximum = maxLactate + 1, Key = LactateAxisKey, Title = "Lactate [mol/ml]" });
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Minimum = minHeartrate - 10, Maximum = maxHearerate + 10, Key = HeartrateAxisKey, Title = "HR [bpm]" });

            foreach (var stepTest in stepTestData)
            {
                var calc = new FblcCalculation(stepTest.Measurements, 4.0);
                var minX = stepTest.Measurements.Min(m => m.Load) - 10;
                var maxX = stepTest.Measurements.Max(m => m.Load) + 10;
                stepTestsModel.Series.Add(new FunctionSeries(calc.FittedLactateCurve, minX, maxX, 20, $"LaF {stepTest.TestDate.ToShortDateString()}") { YAxisKey = LactateAxisKey, XAxisKey = LoadAxisKey });
                stepTestsModel.Series.Add(new FunctionSeries(calc.FittedHeartRateCurve, minX, maxX, 20, $"HrF {stepTest.TestDate.ToShortDateString()}") { YAxisKey = HeartrateAxisKey, XAxisKey = LoadAxisKey });

                var lactateScatterSeries = new ScatterSeries { YAxisKey = LactateAxisKey, XAxisKey = LoadAxisKey, Title = $"LaM {stepTest.TestDate.ToShortDateString()}" };
                var heartRateSeries = new ScatterSeries { YAxisKey = HeartrateAxisKey, XAxisKey = LoadAxisKey, Title = $"HRM {stepTest.TestDate.ToShortDateString()}" };

                foreach (var item in stepTest.Measurements)
                {
                    heartRateSeries.Points.Add(new ScatterPoint(item.Load, item.HeartRate));
                    lactateScatterSeries.Points.Add(new ScatterPoint(item.Load, item.Lactate));
                }

                stepTestsModel.Series.Add(heartRateSeries);
                stepTestsModel.Series.Add(lactateScatterSeries);
            }

            return stepTestsModel;
        }
    }
}
