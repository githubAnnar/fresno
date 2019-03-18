using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Report.Helpers
{
    public static class SectionHelper
    {
        public static Unit GetRelativePageWidth(Section section)
        {
            PageSetup currentPageSetup;

            if (section != null)
            {
                currentPageSetup = section.PageSetup;
            }

            else
            {
                currentPageSetup = null;
            }

            if (currentPageSetup != null)
            {
                Unit pageWidth;
                switch (currentPageSetup.Orientation)
                {
                    case Orientation.Portrait:
                        pageWidth = currentPageSetup.PageWidth - (currentPageSetup.RightMargin + currentPageSetup.LeftMargin);
                        break;
                    case Orientation.Landscape:
                        pageWidth = currentPageSetup.PageHeight - (currentPageSetup.RightMargin + currentPageSetup.LeftMargin);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return pageWidth;
            }

            return Unit.Zero;
        }

        public static List<Unit> GetColumnWidths(int[] parts, Section section)
        {
            return GetColumnWidths(parts, GetRelativePageWidth(section));
        }

        public static List<Unit> GetColumnWidths(int[] parts, Unit width)
        {
            var total = parts.Sum();
            return GetColumnWidths(parts.Select(p => p / (double)total).ToArray(), width);
        }

        public static List<Unit> GetColumnWidths(double[] percentages, Section section)
        {
            return GetColumnWidths(percentages, GetRelativePageWidth(section));
        }

        public static List<Unit> GetColumnWidths(double[] percentages, Unit width)
        {
            return percentages.Select(p => Unit.FromMillimeter(width.Millimeter * p)).ToList();
        }
    }
}
