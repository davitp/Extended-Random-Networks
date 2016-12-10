using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Microsoft.Office.Interop.Excel;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Result;
using Core.Utility;

namespace Storage
{
    /// <summary>
    /// Implementation of Excel result storage. Using Microsoft.Office.Interop.Excel COM.
    /// <remarks> There is a bug concerning to read/write to excel when there are > 1 ensembles.</remarks>
    /// </summary>
    class ExcelResultStorage : AbstractResultStorage
    {
        private Application excelApp = null;
        private Workbook workbook = null;
        private int currentSheetIndex = 1;
        private int globalSheetIndex = 0;
        private int globalsSheetLastRow = 0;
        // TODO
        private int centralitySheetIndex = 0;
        private int centralitySheetLastColumn = 2;

        private SortedDictionary<Guid, string> existingFileNames; 

        public ExcelResultStorage(string str)
            : base(str)
        {
            if (!storageStr.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                storageStr += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(storageStr))
            {
                Directory.CreateDirectory(storageStr);
            }
        }

        public override StorageType GetStorageType()
        {
            return StorageType.ExcelStorage;
        }

        public override void Save(ResearchResult result)
        {
            string fileName = storageStr + result.ResearchName;
            if (File.Exists(fileName + ".xls") || File.Exists(fileName + ".xlsx"))
                fileName += result.ResearchID;

            InitializeExcelApplication();
            workbook = excelApp.Workbooks.Add();

            SaveGeneralInfo(result.ResearchID, result.ResearchName,
                    result.ResearchType, result.ModelType, result.RealizationCount,
                    result.Size, result.Edges, result.ResearchParameterValues,
                    result.GenerationParameterValues);

            for (int i = 0; i < result.EnsembleResults.Count; ++i)
            {
                SaveEnsembleResult(result.ResearchName, result.EnsembleResults[i], i);
            }

            workbook.Sheets[1].Activate();
            workbook.SaveAs(fileName + ".xlsx");

            DestroyExcelApplication();
        }

        public override void Delete(Guid researchID)
        {
            if (existingFileNames != null && existingFileNames.Keys.Contains(researchID))
            {
                File.Delete(existingFileNames[researchID]);
                existingFileNames.Remove(researchID);
            }
            else
            {
                string fileNameToDelete = FileNameByGuid(researchID);
                if (fileNameToDelete != null)
                    File.Delete(fileNameToDelete);
            }
        }

        public override List<ResearchResult> LoadAllResearchInfo()
        {
            InitializeExcelApplication();

            existingFileNames = new SortedDictionary<Guid, string>();
            List<ResearchResult> researchInfos = new List<ResearchResult>();

            ResearchResult DoubleResearchInfo = null;
            Workbook book = null;
            Worksheet sheet = null;
            foreach (string fileName in Directory.GetFiles(storageStr, "*.xlsx",
                SearchOption.TopDirectoryOnly))
            {
                DoubleResearchInfo = new ResearchResult();
                try
                {
                    book = excelApp.Workbooks.Open(fileName);
                    sheet = (Worksheet)book.Worksheets[1];
                    LoadResearchInfo(sheet, DoubleResearchInfo);
                    LoadResearchAndGenerationParameters(sheet, DoubleResearchInfo);

                    researchInfos.Add(DoubleResearchInfo);
                    existingFileNames.Add(DoubleResearchInfo.ResearchID, fileName);
                }
                catch (SystemException)
                {
                    continue;
                }
            }
            book.Close();
            Marshal.ReleaseComObject(book);

            DestroyExcelApplication();

            return researchInfos;
        }

        public override ResearchResult Load(Guid researchID)
        {
            ResearchResult r = null;

            string fileNameToLoad = null;
            if (existingFileNames != null && existingFileNames.Keys.Contains(researchID))
                fileNameToLoad = existingFileNames[researchID];
            else
                fileNameToLoad = FileNameByGuid(researchID);

            InitializeExcelApplication();

            Workbook book = null;
            Worksheet sheet = null;
            if (fileNameToLoad != null)
            {
                r = new ResearchResult();
                try
                {
                    book = excelApp.Workbooks.Open(fileNameToLoad);
                    sheet = (Worksheet)book.Worksheets[1];
                    LoadResearchInfo(sheet, r);
                    LoadResearchAndGenerationParameters(sheet, r);
                    LoadEnsembleResults(book, r);
                }
                catch (SystemException ex)
                {
                    Console.WriteLine(ex.Data);
                }
            }
            book.Close();
            Marshal.ReleaseComObject(book);

            DestroyExcelApplication();
            return r;
        }

        public override ResearchResult Load(string name)
        {
            throw new NotImplementedException();
        }

        #region Utilities

        private void InitializeExcelApplication()
        {
            excelApp = new Application();
            excelApp.Visible = false;
            excelApp.SheetsInNewWorkbook = 1;
            currentSheetIndex = 1;
            globalSheetIndex = 0;
            globalsSheetLastRow = 0;
        }

        private void DestroyExcelApplication()
        {            
            if (workbook != null)
            {
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                workbook = null;
            }
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
            workbook = null;
        }

        #region Save

        private void SaveGeneralInfo(Guid researchID,
            string researchName,
            ResearchType rType,
            ModelType mType,
            int realizationCount,
            UInt32 size,
            Double edges,
            Dictionary<ResearchParameter, object> rp,
            Dictionary<GenerationParameter, object> gp)
        {
            Worksheet sheet = GetNextWorksheet();
            sheet.Name = "General";
            int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

            sheet.Columns[1].ColumnWidth = 30;
            sheet.Columns[2].ColumnWidth = 40;
            sheet.Columns[2].HorizontalAlignment = XlHAlign.xlHAlignLeft;
            sheet.Cells[lastRow, 1] = "Research Info";
            sheet.Cells[lastRow, 1].EntireRow.Font.Bold = true;
            ++lastRow;

            sheet.Cells[lastRow, 1] = "ResearchID";
            sheet.Cells[lastRow, 2] = researchID.ToString();
            ++lastRow;

            sheet.Cells[lastRow, 1] = "ResearchName";
            sheet.Cells[lastRow, 2] = researchName;
            ++lastRow;

            sheet.Cells[lastRow, 1] = "ResearchType";
            sheet.Cells[lastRow, 2] = rType.ToString();
            ++lastRow;

            sheet.Cells[lastRow, 1] = "ModelType";
            sheet.Cells[lastRow, 2] = mType.ToString();
            ++lastRow;

            sheet.Cells[lastRow, 1] = "RealizationCount";
            sheet.Cells[lastRow, 2] = realizationCount;
            ++lastRow;
            
            sheet.Cells[lastRow, 1] = "Date";
            sheet.Cells[lastRow, 2] = DateTime.Now.ToString();
            ++lastRow;

            sheet.Cells[lastRow, 1] = "Size";
            sheet.Cells[lastRow, 2] = size;
            ++lastRow;

            sheet.Cells[lastRow, 1] = "Edges";
            sheet.Cells[lastRow, 2] = edges;

            SaveResearchParameters(sheet, rp);
            SaveGenerationParameters(sheet, gp);
        }

        private void SaveResearchParameters(Worksheet sheet, Dictionary<ResearchParameter, object> p)
        {
            if (p.Count != 0)
            {
                int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
                lastRow += 2;
                sheet.Cells[lastRow, 1] = "Research Parameters";
                sheet.Cells[lastRow, 1].EntireRow.Font.Bold = true;
                ++lastRow;

                foreach (ResearchParameter rp in p.Keys)
                {
                    if (p[rp] != null)
                    {
                        sheet.Cells[lastRow, 1] = rp.ToString();
                        sheet.Cells[lastRow, 2] = p[rp].ToString();
                        ++lastRow;
                    }
                }
            }
        }

        private void SaveGenerationParameters(Worksheet sheet, Dictionary<GenerationParameter, object> p)
        {
            int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row + 2;
            sheet.Cells[lastRow, 1] = "Generation Parameters";
            sheet.Cells[lastRow, 1].EntireRow.Font.Bold = true;
            ++lastRow;

            foreach (GenerationParameter gp in p.Keys)
            {
                if (p[gp] == null)
                    continue;
                if (gp == GenerationParameter.AdjacencyMatrix)
                {
                    sheet.Cells[lastRow, 1] = "FileName";
                    sheet.Cells[lastRow, 2] = ((MatrixPath)p[gp]).Path;
                }
                else
                {
                    sheet.Cells[lastRow, 1] = gp.ToString();
                    sheet.Cells[lastRow, 2] = p[gp].ToString();
                }       
                ++lastRow;
            }
        }

        private void SaveEnsembleResult(string researchName, EnsembleResult e, int id)
        {
            foreach (AnalyzeOption opt in e.Result.Keys)
            {
                AnalyzeOptionInfo info = ((AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false))[0];
                OptionType optionType = info.OptionType;

                switch (optionType)
                {
                    case OptionType.Global:
                        SaveToGlobalSheet(id, info, e.Result[opt]);
                        break;
                    case OptionType.ValueList:
                        SaveValueListSheet(id, info, e.Result[opt]);
                        break;
                    case OptionType.Centrality:
                        SaveCentralitySheet(id, info, e.Result[opt]);
                        break;
                    case OptionType.Distribution:
                        SaveDistributionSheet(id, info, e.Result[opt]);
                        break;
                    case OptionType.Trajectory:
                        SaveTrajectorySheetAndFile(researchName, id, info, e.Result[opt]);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveToGlobalSheet(int id, AnalyzeOptionInfo info, Object value)
        {
            Worksheet sheet = null;
            if (globalSheetIndex == 0)
            {
                sheet = GetNextWorksheet();
                globalSheetIndex = sheet.Index;
            }
            else
            {
                sheet = (Worksheet)workbook.Sheets[globalSheetIndex];
            }
            sheet.Name = "Globals";
            sheet.Columns[1].ColumnWidth = 30;
            sheet.Columns[2].ColumnWidth = 20;
            sheet.Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            if (globalsSheetLastRow == 0)
                globalsSheetLastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

            sheet.Cells[globalsSheetLastRow, 1] = info.FullName;
            sheet.Cells[globalsSheetLastRow, 2] = value.ToString();
            ++globalsSheetLastRow;
        }

        private void SaveValueListSheet(int id, AnalyzeOptionInfo info, Object value)
        {
            Worksheet sheet = GetNextWorksheet();
            sheet.Name = info.FullName;
            sheet.Columns[1].ColumnWidth = 20;
            sheet.Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

            Debug.Assert(value is List<Double>);
            List<Double> l = value as List<Double>;
            foreach (Double d in l)
            {
                sheet.Cells[lastRow, 1] = d;
                ++lastRow;
            }
        }

        private void SaveCentralitySheet(int id, AnalyzeOptionInfo info, Object value)
        {
            Debug.Assert(value is List<Double>);
            List<Double> l = value as List<Double>;
            int s = l.Count();
            Worksheet sheet = null;
            if (centralitySheetIndex == 0)
            {
                sheet = GetNextWorksheet();
                centralitySheetIndex = sheet.Index;
                sheet.Name = "Centrality";
                sheet.Columns[1].ColumnWidth = 10;
                sheet.Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                sheet.Cells[1, 1] = "Vertex";
                sheet.Cells[1, 1].EntireRow.Font.Bold = true;
                for (int i = 2; i <= s + 1; ++i)
                    sheet.Cells[i, 1] = i - 2;
            }
            else
            {
                sheet = (Worksheet)workbook.Sheets[centralitySheetIndex];
            }
            sheet.Columns[centralitySheetLastColumn].ColumnWidth = 20;
            sheet.Cells[1, centralitySheetLastColumn] = info.FullName;
            for (int i = 2; i <= s + 1; ++i)
                sheet.Cells[i, centralitySheetLastColumn] = l[i - 2].ToString();
            ++centralitySheetLastColumn;
        }

        private void SaveDistributionSheet(int id, AnalyzeOptionInfo info, Object value)
        {
            int length = (info.FullName.Length > 31) ? 30 : info.FullName.Length;
            Worksheet sheet = GetNextWorksheet();
            sheet.Name = info.FullName.Substring(0, length);
            sheet.Columns[1].ColumnWidth = 20;
            sheet.Columns[2].ColumnWidth = 20;
            sheet.Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

            sheet.Cells[lastRow, 1] = info.XAxisName;
            sheet.Cells[lastRow, 1].EntireRow.Font.Bold = true;
            sheet.Cells[lastRow, 2] = info.YAxisName;
            sheet.Cells[lastRow, 2].EntireRow.Font.Bold = true;
            ++lastRow;

            Debug.Assert(value is SortedDictionary<Double, Double>);
            SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
            foreach (Double d in l.Keys)
            {
                sheet.Cells[lastRow, 1] = d;
                sheet.Cells[lastRow, 2] = l[d];
                ++lastRow;
            }
        }

        private void SaveTrajectorySheetAndFile(string researchName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = storageStr + researchName + "_ " + info.FullName + ".txt";

            int length = (info.FullName.Length > 31) ? 30 : info.FullName.Length;
            Worksheet sheet = GetNextWorksheet();
            sheet.Name = info.FullName.Substring(0, length);
            sheet.Columns[1].ColumnWidth = 100;
            sheet.Columns.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            int lastRow = sheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

            sheet.Cells[lastRow, 1] = fileName;
            sheet.Cells[lastRow, 1].EntireRow.Font.Bold = true;

            Debug.Assert(value is SortedDictionary<Double, Double>);
            SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (Double d in l.Keys)
                    file.WriteLine(d.ToString() + " " + l[d].ToString());
            }
        }

        #endregion

        #region Load

        private void LoadResearchInfo(Worksheet sheet, ResearchResult r)
        {
            r.ResearchID = new Guid((string)sheet.get_Range("B2").Cells.Value);
            r.ResearchName = sheet.get_Range("B3").Cells.Value;
            r.ResearchType = (ResearchType)Enum.Parse(typeof(ResearchType), (string)sheet.get_Range("B4").Cells.Value);
            r.ModelType = (ModelType)Enum.Parse(typeof(ModelType), (string)sheet.get_Range("B5").Cells.Value);
            r.RealizationCount = (Int32)sheet.get_Range("B6").Cells.Value;
            r.Date = sheet.get_Range("B7").Cells.Value;
            r.Size = (UInt32)sheet.get_Range("B8").Cells.Value;
            r.Edges = sheet.get_Range("B9").Cells.Value;
        }

        private void LoadResearchAndGenerationParameters(Worksheet sheet, ResearchResult r)
        {
            int rowIndex = 12;
            string currentParameter = sheet.get_Range("A" + rowIndex).Cells.Value;
            if ("Research Parameters" == sheet.get_Range("A11").Cells.Value)
            {                
                while (currentParameter != null)
                {
                    ResearchParameter rp = (ResearchParameter)Enum.Parse(typeof(ResearchParameter), currentParameter);
                    r.ResearchParameterValues.Add(rp, sheet.get_Range("B" + rowIndex).Cells.Value);
                    ++rowIndex;
                    currentParameter = sheet.get_Range("A" + rowIndex).Cells.Value;
                }
                rowIndex += 2;
                currentParameter = sheet.get_Range("A" + rowIndex).Cells.Value;
            }
            while (currentParameter != null)
            {
                GenerationParameter gp = (GenerationParameter)Enum.Parse(typeof(GenerationParameter), currentParameter);
                r.GenerationParameterValues.Add(gp, sheet.get_Range("B" + rowIndex).Cells.Value);
                ++rowIndex;
                currentParameter = sheet.get_Range("A" + rowIndex).Cells.Value;
            }
        }
        
        private void LoadEnsembleResults(Workbook book, ResearchResult r)
        {
            EnsembleResult e = new EnsembleResult(r.Size);
            //e.NetworkSize = r.Size;
            e.EdgesCount = r.Edges;
            e.Result = new Dictionary<AnalyzeOption, Object>();

            Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
            foreach (AnalyzeOption opt in existingOptions)
            {
                AnalyzeOptionInfo optInfo = (AnalyzeOptionInfo)(opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false)[0]);
                switch (optInfo.OptionType)
                {
                    case OptionType.Global:
                        Double v = FindValueInGlobals(book, optInfo.FullName);
                        if ( v != -1)
                            e.Result.Add(opt, v);
                        break;
                    case OptionType.ValueList:
                        Object vl = LoadValueList(book, optInfo);
                        if (vl != null)
                            e.Result.Add(opt, vl);
                        break;
                    case OptionType.Distribution:
                    case OptionType.Trajectory:
                        Object vd = LoadDistribution(book, optInfo);
                        if (vd != null)
                            e.Result.Add(opt, vd);
                        break;
                    default:
                        break;
                }
            }
            
            r.EnsembleResults.Add(e);
        }

        private Object LoadValueList(Workbook book, AnalyzeOptionInfo info)
        {
            int length = (info.FullName.Length > 31) ? 30 : info.FullName.Length;
            Worksheet sheet = FindSheetInBook(book, info.FullName.Substring(0, length));
            if (sheet == null)
                return null;

            List<Double> valueList = new List<Double>();
            Range r = sheet.UsedRange;
            foreach (Range c in r)
            {
                valueList.Add(c.Value);
            }
            return valueList;
        }

        private Object LoadDistribution(Workbook book, AnalyzeOptionInfo info)
        {
            int length = (info.FullName.Length > 31) ? 30 : info.FullName.Length;
            Worksheet sheet = FindSheetInBook(book, info.FullName.Substring(0, length));
            if (sheet == null)
                return null;

            Range range = sheet.UsedRange;
            SortedDictionary<Double, Double> d = new SortedDictionary<Double, Double>();
            for (int i = 2; i <= range.Rows.Count; ++i)
            {
                Range r = range.Rows[i];
                d.Add(r.Cells[1].Value, r.Cells[2].Value);
            }
            return d;
        }

        #endregion

        private Worksheet GetNextWorksheet()
        {
            Worksheet r = null;
            if (currentSheetIndex == 1)
                r = (Worksheet)workbook.Sheets[currentSheetIndex];
            else
                r = (Worksheet)workbook.Sheets.Add(After: workbook.Sheets[workbook.Sheets.Count]);
            ++currentSheetIndex;
            return r;
        }

        private Worksheet FindSheetInBook(Workbook book, string name)
        {
            foreach (Worksheet sheet in book.Sheets)
            {
                if (name == sheet.Name)
                    return sheet;
            }
            return null;
        }

        private Double FindValueInGlobals(Workbook book, string name)
        {
            Worksheet sheet = FindSheetInBook(book, "Globals");
            if (sheet == null)
                return -1;

            Range range = sheet.UsedRange;
            foreach (Range row in range.Rows)
            {
                if (row.Cells[1].value == name)
                    return row.Cells[2].value;
            }

            return -1;
        }

        private string FileNameByGuid(Guid id)
        {
            InitializeExcelApplication();

            Workbook book = null;
            Worksheet sheet = null;
            string rId = null;
            foreach (string fileName in Directory.GetFiles(storageStr, "*.xlsx",
                SearchOption.TopDirectoryOnly))
            {
                try
                {
                    book = excelApp.Workbooks.Open(fileName);
                    sheet = (Worksheet)book.Worksheets[1];
                    rId = sheet.get_Range("B2").Cells.Value;
                    if (id == Guid.Parse(rId))
                    {
                        return fileName;
                    }
                }
                catch (SystemException)
                {
                    continue;
                }
            }
            book.Close();
            Marshal.ReleaseComObject(book);

            DestroyExcelApplication();
            return null;
        }

        #endregion
    }
}