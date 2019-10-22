using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.Report.Helpers;
using LanterneRouge.Fresno.Report.PlotModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using OxyPlot.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LanterneRouge.Fresno.Report
{
    public class StepTestReport
    {
        #region Fields

        private readonly int[] _metaDataTableColumnRelativeWidths = new[] { 1, 3, 1, 1 };
        private readonly int[] _measurementsTableColumnRelativeWidths = new[] { 1, 1, 1, 2 };
        private readonly int[] _resultTableColumnRelativeWidths = new[] { 1, 1, 1, 1, 1 };
        private FblcCalculation _lactateCalculation;
        private List<Zone> _lactateZones;
        private const double MARKER = 4d;
        private readonly double[] ZONES = new[] { 0.8, 1.5, 2.5, 4.0, 6.0, 10.0 };

        #endregion

        #region Constructor

        public StepTestReport(StepTest stepTest)
        {
            ReportStepTest = stepTest ?? throw new ArgumentNullException(nameof(stepTest));
        }

        #endregion

        #region Properties

        public StepTest ReportStepTest { get; }

        private FblcCalculation LactateCalculation => _lactateCalculation ?? (_lactateCalculation = new FblcCalculation(ReportStepTest.Measurements, MARKER));

        private List<Zone> LactateZones => _lactateZones ?? (_lactateZones = new LactateBasedZones(LactateCalculation, ZONES).Zones.ToList());

        #endregion

        #region Methods

        public Document CreateReport()
        {
            var document = new Document();

            var dataSection = document.AddSection();
            dataSection.PageSetup = document.DefaultPageSetup.Clone();

            // Table for formatting Test metadata
            var metaDataTable = dataSection.AddTable();
            foreach (var columnWidth in SectionHelper.GetColumnWidths(_metaDataTableColumnRelativeWidths, dataSection))
            {
                var column = metaDataTable.AddColumn(columnWidth);
            }

            var firstMetaDataRow = metaDataTable.AddRow();
            firstMetaDataRow.Format.Font.Size = Unit.FromPoint(12d);
            firstMetaDataRow[0].AddParagraph("Navn:").Format.Font.Bold = true;
            firstMetaDataRow[1].AddParagraph($"{ReportStepTest.ParentUser.FirstName} {ReportStepTest.ParentUser.LastName}");
            firstMetaDataRow[1].MergeRight = 2;
            var secondMetatDataRow = metaDataTable.AddRow();
            secondMetatDataRow.Format.Font.Size = Unit.FromPoint(12d);
            secondMetatDataRow[0].AddParagraph("Dato:").Format.Font.Bold = true;
            secondMetatDataRow[1].AddParagraph(ReportStepTest.TestDate.Date.ToLongDateString());
            secondMetatDataRow[2].AddParagraph("Kl:").Format.Font.Bold = true;
            secondMetatDataRow[3].AddParagraph(ReportStepTest.TestDate.TimeOfDay.ToString(@"hh\:mm"));

            var p = dataSection.AddParagraph(string.Empty);
            p.Format.SpaceAfter = Unit.FromMillimeter(10d);
            p.Format.SpaceBefore = Unit.FromMillimeter(10d);

            // Table for Measurements
            var measurementsTable = dataSection.AddTable();
            foreach (var columnWidth in SectionHelper.GetColumnWidths(_measurementsTableColumnRelativeWidths, dataSection))
            {
                var column = measurementsTable.AddColumn(columnWidth);
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            // First row is header
            var measurementHeaderRow = measurementsTable.AddRow();
            measurementHeaderRow.HeadingFormat = true;
            measurementHeaderRow.Format.Font.Color = Colors.White;
            measurementHeaderRow.Height = Unit.FromMillimeter(10d);
            measurementHeaderRow.VerticalAlignment = VerticalAlignment.Center;
            measurementHeaderRow.Shading = new Shading { Color = Colors.Black };
            measurementHeaderRow[0].AddParagraph("Prøve nr.");
            measurementHeaderRow[1].AddParagraph($"Belastning [{ReportStepTest.EffortUnit}]");
            measurementHeaderRow[2].AddParagraph($"HR [BPM]");
            measurementHeaderRow[3].AddParagraph($"Laktat [mmol/L]");

            // Add measurements
            foreach (var measurement in ReportStepTest.Measurements)
            {
                var dataRow = measurementsTable.AddRow();
                dataRow.Borders = new Borders { Width = 0.1, Color = Colors.Black };
                dataRow.Height = Unit.FromMillimeter(10d);
                dataRow.VerticalAlignment = VerticalAlignment.Center;
                dataRow[0].AddParagraph(measurement.Sequence.ToString());
                dataRow[1].AddParagraph(measurement.Load.ToString("0"));
                dataRow[2].AddParagraph(measurement.HeartRate.ToString());
                dataRow[3].AddParagraph(measurement.Lactate.ToString("0.0"));
            }

            // Add Result Header
            p = dataSection.AddParagraph("Resultat:");
            p.Format.SpaceAfter = Unit.FromMillimeter(10d);
            p.Format.SpaceBefore = Unit.FromMillimeter(10d);

            // Add Result Table
            var resultsTable = dataSection.AddTable();
            foreach (var columnWidth in SectionHelper.GetColumnWidths(_resultTableColumnRelativeWidths, dataSection))
            {
                var column = resultsTable.AddColumn(columnWidth);
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            // First and secondt Row is Header
            var resultHeaderRow1 = resultsTable.AddRow();
            resultHeaderRow1.HeadingFormat = true;
            resultHeaderRow1.Format.Font.Color = Colors.White;
            resultHeaderRow1.Shading = new Shading { Color = Colors.Black };
            resultHeaderRow1.Height = Unit.FromMillimeter(10d);
            resultHeaderRow1.VerticalAlignment = VerticalAlignment.Center;
            resultHeaderRow1[0].AddParagraph("Sone");
            resultHeaderRow1[1].AddParagraph("HR [BPM]");
            resultHeaderRow1[1].MergeRight = 1;
            resultHeaderRow1[3].AddParagraph($"Belastning [{ReportStepTest.EffortUnit}]");
            resultHeaderRow1[3].MergeRight = 1;

            var resultHeaderRow2 = resultsTable.AddRow();
            resultHeaderRow2.HeadingFormat = true;
            resultHeaderRow2.Format.Font.Color = Colors.White;
            resultHeaderRow2.Shading = new Shading { Color = Colors.Black };
            resultHeaderRow2[1].AddParagraph("Fra");
            resultHeaderRow2[2].AddParagraph("Til");
            resultHeaderRow2[3].AddParagraph("Fra");
            resultHeaderRow2[4].AddParagraph("Til");
            resultHeaderRow1.KeepWith = 1;

            // Add zones
            foreach (var zone in LactateZones)
            {
                var dataRow = resultsTable.AddRow();
                dataRow.Height = Unit.FromMillimeter(10d);
                dataRow.Borders = new Borders { Width = 0.1, Color = Colors.Black };
                dataRow.VerticalAlignment = VerticalAlignment.Center;
                dataRow[0].AddParagraph(zone.Name);
                dataRow[1].AddParagraph(zone.LowerHeartRate.ToString("0"));
                dataRow[2].AddParagraph(zone.UpperHeartRate.ToString("0"));
                dataRow[3].AddParagraph(zone.LowerLoad.ToString("0.0"));
                dataRow[4].AddParagraph(zone.UpperLoad.ToString("0.0"));
            }

            // Add Threshold string
            p = dataSection.AddParagraph($"Belastning Terskel: {LactateCalculation.LoadThreshold.ToString("0.0")} {ReportStepTest.EffortUnit}\tHjertefrekvens Terskel: {LactateCalculation.HeartRateThreshold.ToString("0")} BPM");
            p.Format.SpaceAfter = Unit.FromMillimeter(10d);
            p.Format.SpaceBefore = Unit.FromMillimeter(10d);

            return document;
        }

        public byte[] GetStepTestPlotXImage(IEnumerable<StepTest> additionalStepTests)
        {
            var list = new List<StepTest> { ReportStepTest };
            list.AddRange(additionalStepTests);
            var plotModel = StepTests.StepTestPlotModel(list);
            using (var mstream = new MemoryStream())
            {
                PdfExporter.Export(plotModel, mstream, 841.98, 595.11);
                return mstream.ToArray();
            }
        }

        public void PdfRender(string filename, Document document, bool addPlot, IEnumerable<StepTest> additionalStepTests)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("message", nameof(filename));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var pdfRenderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            pdfRenderer.RenderDocument();

            pdfRenderer.Save(filename);

            if (addPlot)
            {
                var image = GetStepTestPlotXImage(additionalStepTests);
                using (var pdfDocument = PdfReader.Open(filename))

                {
                    using (var plotPdf = PdfReader.Open(new MemoryStream(image), PdfDocumentOpenMode.Import))
                    {
                        foreach (PdfPage page in plotPdf.Pages)
                        {
                            pdfDocument.AddPage(page);
                        }
                    }

                    pdfDocument.Save(filename);
                }
            }
        }

        #endregion
    }
}
