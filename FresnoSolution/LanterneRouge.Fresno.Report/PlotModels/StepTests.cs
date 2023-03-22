using Autofac;
using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Core.Entities;
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

        public static PlotModel StepTestPlotModel(IEnumerable<StepTest> stepTests)
        {
            if (stepTests is null)
            {
                throw new ArgumentNullException(nameof(stepTests));
            }

            var stepTestData = stepTests.ToList();
            var testPlotType = stepTests.Count() > 1 ? "Comparison" : "Plot";
            var stepTestsModel = new PlotModel { Title = $"{testPlotType} of user {string.Join(", ", stepTests.Select(st => (GetDataManager().GetUserByStepTest(st)).LastName).Distinct())}" };
            var minLoad = stepTests.Select(st => GetDataManager().GetAllMeasurementsByStepTest(st).Min(m => m.Load)).Min();
            var maxLoad = stepTests.Select(st => GetDataManager().GetAllMeasurementsByStepTest(st).Max(m => m.Load)).Max();
            var minLactate = stepTests.Select(st => GetDataManager().GetAllMeasurementsByStepTest(st).Min(m => m.Lactate)).Min();
            var maxLactate = stepTests.Select(st => GetDataManager().GetAllMeasurementsByStepTest(st).Max(m => m.Lactate)).Max();
            var minHeartrate = stepTests.Select(st => GetDataManager().GetAllMeasurementsByStepTest(st).Min(m => m.HeartRate)).Min();
            var maxHearerate = stepTests.Select(st => GetDataManager().GetAllMeasurementsByStepTest(st).Max(m => m.HeartRate)).Max();
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = minLoad - 10, Maximum = maxLoad + 10, Key = LoadAxisKey, Title = "Load [W]" });
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = minLactate - 1, Maximum = maxLactate + 1, Key = LactateAxisKey, Title = "Lactate [mol/ml]" });
            stepTestsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Minimum = minHeartrate - 10, Maximum = maxHearerate + 10, Key = HeartrateAxisKey, Title = "HR [bpm]" });

            foreach (var stepTest in stepTestData)
            {
                var calc = new FblcCalculation(GetDataManager().GetAllMeasurementsByStepTest(stepTest), 4.0);
                var minX = GetDataManager().GetAllMeasurementsByStepTest(stepTest).Min(m => m.Load) - 10;
                var maxX = GetDataManager().GetAllMeasurementsByStepTest(stepTest).Max(m => m.Load) + 10;
                stepTestsModel.Series.Add(new FunctionSeries(calc.FittedLactateCurve, minX, maxX, 20, $"LaF {stepTest.TestDate.ToShortDateString()}") { YAxisKey = LactateAxisKey, XAxisKey = LoadAxisKey });
                stepTestsModel.Series.Add(new FunctionSeries(calc.FittedHeartRateCurve, minX, maxX, 20, $"HrF {stepTest.TestDate.ToShortDateString()}") { YAxisKey = HeartrateAxisKey, XAxisKey = LoadAxisKey });

                var lactateScatterSeries = new ScatterSeries { YAxisKey = LactateAxisKey, XAxisKey = LoadAxisKey, Title = $"LaM {stepTest.TestDate.ToShortDateString()}" };
                var heartRateSeries = new ScatterSeries { YAxisKey = HeartrateAxisKey, XAxisKey = LoadAxisKey, Title = $"HRM {stepTest.TestDate.ToShortDateString()}" };

                foreach (var item in GetDataManager().GetAllMeasurementsByStepTest(stepTest))
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
