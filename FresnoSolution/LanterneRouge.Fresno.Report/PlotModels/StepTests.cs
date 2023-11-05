using Autofac;
using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Services;
using LanterneRouge.Fresno.Services.Interfaces;
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
        private static IDataService _dataManager;

        #endregion

        private static IDataService GetDataManager()
        {
            if (_dataManager == null)
            {
                var scope = ServiceLocator.Instance.BeginLifetimeScope();
                _dataManager = scope.Resolve<IDataService>();
            }

            return _dataManager;
        }

        public static PlotModel StepTestPlotModel(List<StepTest> stepTests)
        {
            if (stepTests is null)
            {
                throw new ArgumentNullException(nameof(stepTests));
            }

            var stepTestData = stepTests.ToList();
            var testPlotType = stepTests.Count > 1 ? "Comparison" : "Plot";
            var stepTestsModel = new PlotModel { Title = $"{testPlotType} of user {string.Join(", ", stepTests.Select(s => s.User.LastName))}" };
            var minLoad = stepTests.SelectMany(st => st.Measurements).Min(s => s.Load);
            var maxLoad = stepTests.SelectMany(st => st.Measurements).Max(s => s.Load);
            var minLactate = stepTests.SelectMany(st => st.Measurements).Min(s => s.Lactate);
            var maxLactate = stepTests.SelectMany(st => st.Measurements).Max(s => s.Lactate);
            var minHeartrate = stepTests.SelectMany(st => st.Measurements).Min(s => s.HeartRate);
            var maxHearerate = stepTests.SelectMany(st => st.Measurements).Max(s => s.HeartRate);
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = minLoad - 10, Maximum = maxLoad + 10, Key = LoadAxisKey, Title = "Load [W]" });
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minLactate - 1, Maximum = maxLactate + 1, Key = LactateAxisKey, Title = "Lactate [mol/ml]" });
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Minimum = minHeartrate - 10, Maximum = maxHearerate + 10, Key = HeartrateAxisKey, Title = "HR [bpm]" });

            foreach (var stepTest in stepTestData)
            {
                var calc = new FblcCalculation(stepTest.Measurements.ToList(), 4.0);
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
