using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

using Core.Enumerations;
using Session.StatEngine;

namespace RandNetStat
{
    public partial class GraphicsTab : UserControl
    {
        private AnalyzeOption option;
        private SortedDictionary<Double, Double> values;
        private DrawingOption drawingOptions;
        private StatisticsOption statisticsOptions;

        public GraphicsTab(AnalyzeOption o, SortedDictionary<Double, Double> v)
        {
            option = o;
            values = v;
            drawingOptions = new DrawingOption(Color.Black, false);
            statisticsOptions = new StatisticsOption(ApproximationType.None, ThickeningType.None, 0);

            InitializeComponent();
        }

        public void SaveChartToPng(string location)
        {
            Guid i = Guid.NewGuid();
            string fileName = location + "\\" + i.ToString() + ".png";
            graphic.SaveImage(fileName, ChartImageFormat.Png);
        }

        private void GraphicsTab_Load(object sender, EventArgs e)
        {
            /*ChartArea chArea = new ChartArea("my chart area");
            chArea.AxisX.Title = "X axis";
            chArea.AxisY.Title = "Y axis";
            graphic.ChartAreas.Add(chArea);*/

            Series s = new Series(option.ToString());
            InitializeDrawingOptions(s);
            InitializeValues(s);
            graphic.Series.Add(s);
        }

        private void drawingOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(graphic.Series.Count == 1);
            DrawingOptionsWindow w = new DrawingOptionsWindow();
            w.LineColor = drawingOptions.LineColor;
            w.Points = drawingOptions.IsPoints;
            if (w.ShowDialog() == DialogResult.OK)
            {
                drawingOptions.LineColor = w.LineColor;
                drawingOptions.IsPoints = w.Points;
                InitializeDrawingOptions(graphic.Series[0]);
            }
        }

        private void statisticsOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(graphic.Series.Count == 1);
            StatisticsOptionsWindow w = new StatisticsOptionsWindow();
            w.ApproximationType = statisticsOptions.ApproximationType;
            w.ThickeningType = statisticsOptions.ThickeningType;
            w.ThickeningValue = statisticsOptions.ThickeningValue;
            if (w.ShowDialog() == DialogResult.OK)
            {
                statisticsOptions.ApproximationType = w.ApproximationType;
                statisticsOptions.ThickeningType = w.ThickeningType;
                statisticsOptions.ThickeningValue = w.ThickeningValue;
                InitializeValues(graphic.Series[0]);
            }
        }

        private void combineToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region Utilities

        private void InitializeDrawingOptions(Series s)
        {
            s.ChartType = drawingOptions.IsPoints ? SeriesChartType.Point : SeriesChartType.Line;
            s.Color = drawingOptions.LineColor;
        }

        private void InitializeValues(Series s)
        {
            // TODO calculate values using statistics options
            // if options are not changed maybe no set values?
            foreach (Double d in values.Keys)
                s.Points.Add(new DataPoint(d, values[d]));
        }

        #endregion
    }
}
