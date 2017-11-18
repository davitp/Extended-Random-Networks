using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using Session;
using Core.Enumerations;
using Core.Attributes;
using Core.Settings;
using Core.Utility;
using Core.Exceptions;

namespace RandNet
{
    public enum DialogType
    {
        Create,
        Edit,
        Clone
    };

    public partial class CreateEditResearchWindow : Form
    {
        private DialogType dType;
        private ResearchType rType;
        private Guid researchId;

        public Guid ResultResearchId { private set; get; }

        public CreateEditResearchWindow(DialogType dt, ResearchType rt)
        {
            dType = dt;
            rType = rt;

            InitializeComponent();
            InitializeTexts();
        }

        public CreateEditResearchWindow(DialogType dt, ResearchType rt, Guid id)
        {
            Debug.Assert(SessionManager.ResearchExists(id));
            Debug.Assert(SessionManager.GetResearchType(id) == rt);
            dType = dt;
            rType = rt;
            researchId = id;

            InitializeComponent();
            InitializeTexts();
        }

        #region Event Hanldlers

        private void CreateEditResearchWindow_Load(object sender, EventArgs e)
        {
            InitializeGeneralGroup();
            InitializeParametersGroup();
            InitializeAnalyzeOptionsGroup();
        }

        private void modelTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeParametersGroup();
            InitializeAnalyzeOptionsGroup();
        }

        private void generationTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetCurrentGenerationType() == GenerationType.Random)
                realizationCountTxt.Enabled = true;
            else
            {
                realizationCountTxt.Enabled = false;
                realizationCountTxt.Value = 1;
            }
            InitializeParametersGroup();
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            ChangeCheckedStatesForAnalyzeOptions(true);
        }

        private void deselectAll_Click(object sender, EventArgs e)
        {
            ChangeCheckedStatesForAnalyzeOptions(false);
        }        

        private void create_Click(object sender, EventArgs e)
        {
            if (researchNameTxt.Text == "")
            {
                MessageBox.Show("Research Name should be specified.", "Error");
                researchNameTxt.Focus();
                return;
            }

            if (realizationCountTxt.Value <= 0)
            {
                MessageBox.Show("Realizations count should be positive number.", "Error");
                realizationCountTxt.Focus();
                return;
            }

            switch (dType)
            {
                case DialogType.Create:
                case DialogType.Clone:
                    ResultResearchId = SessionManager.CreateResearch(rType);
                    break;
                case DialogType.Edit:
                    ResultResearchId = researchId;
                    break;
            }
            SetGeneralValues();
            SetParameterValues();
            SetAnalyzeOptionsValues();
            DialogResult = DialogResult.OK;
        }

        #endregion

        #region Utilities

        private void InitializeTexts()
        {
            researchTypeTxt.Text = rType.ToString();
            switch (dType)
            {
                case DialogType.Create:
                    Text = "Create Research";
                    create.Text = "Create";
                    break;
                case DialogType.Clone:
                    Text = "Clone Research";
                    create.Text = "Create";
                    break;
                case DialogType.Edit:
                    Text = "Edit Research";
                    create.Text = "Update";
                    break;
            }
        }

        private void InitializeGeneralGroup()
        {  
            if (dType != DialogType.Create)
            {
                researchNameTxt.Text = SessionManager.GetResearchName(researchId);
                tracingCheck.Checked = (SessionManager.GetResearchTracingPath(researchId) != "");
                checkConnected.Checked = SessionManager.GetResearchCheckConnected(researchId);
                realizationCountTxt.Value = SessionManager.GetResearchRealizationCount(researchId);
            }
            
            InitializeModelTypeCmb();
            InitializeStorageTypeCmb();
            InitializeGenerationTypeCmb();
        }

        private void InitializeParametersGroup()
        {
            parametersPanel.Controls.Clear();
            
            List<ResearchParameter> rp = SessionManager.GetRequiredResearchParameters(rType);
            List<GenerationParameter> gp = SessionManager.GetRequiredGenerationParameters(
                rType, GetCurrentModelType(), GetCurrentGenerationType());
            rp.Sort(); gp.Sort();

            int location = 0, f = 10, s = 200;
            foreach (ResearchParameter r in rp)
            {
                Type rt = GetResearchParameterType(r);
                if (rt != typeof(Boolean))
                    AddLabelToParametersPanel(r.ToString(), f, location);
                if (rt == typeof(UInt32) || rt == typeof(Double))
                    AddTextBoxToParametersPanel(r.ToString(), GetResearchParameterValue(r), rt, s, location);
                else if (rt == typeof(Boolean))
                    AddCheckBoxToParametersPanel(r.ToString(), GetResearchParameterValue(r), s, location);
                else if (rt == typeof(ResearchType))
                    AddTextBoxToParametersPanel(r.ToString(), "Basic", rt, s, location);
                else if (rt == typeof(MatrixPath))
                    AddFileInputToParametersPanel(r.ToString(), FileInputType.Folder, GetResearchParameterValue(r), s, location);
                else
                    throw new CoreException("Undefined type for research parameter.");
                ++location;
            }

            foreach (GenerationParameter g in gp)
            {
                Type gt = GetGenerationParameterType(g);
                if (gt != typeof(Boolean))
                    AddLabelToParametersPanel(g.ToString(), f, location);
                if (gt == typeof(UInt32) || gt == typeof(Double))
                    AddTextBoxToParametersPanel(g.ToString(), GetGenerationParameterValue(g), gt, s, location);
                else if (gt == typeof(Boolean))
                    AddCheckBoxToParametersPanel(g.ToString(), GetGenerationParameterValue(g), s, location);
                else if (gt == typeof(MatrixPath))
                    AddFileInputToParametersPanel(g.ToString(), FileInputType.File, GetGenerationParameterValue(g), s, location);
                else
                    throw new CoreException("Undefined type for generation parameter.");
                ++location;
            }
        }

        private void InitializeAnalyzeOptionsGroup()
        {
            optionsPanel.Controls.Clear();

            AnalyzeOption availableOptions = SessionManager.GetAvailableAnalyzeOptions(rType, GetCurrentModelType());
            AnalyzeOption checkedOptions = AnalyzeOption.None;
            if (dType != DialogType.Create)
                checkedOptions = SessionManager.GetAnalyzeOptions(researchId);

            Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
            int location = 0;
            foreach (AnalyzeOption opt in existingOptions)
            {
                if ((availableOptions & opt) == opt && opt != AnalyzeOption.None)
                {
                    CheckBox cb = new CheckBox();
                    cb.Name = opt.ToString();
                    cb.Text = opt.ToString();
                    cb.AutoSize = true;
                    cb.Location = new Point(10, (location++) * 20 + 5);
                    cb.Checked = false;
                    optionsPanel.Controls.Add(cb);
                    if (dType != DialogType.Create && (checkedOptions & opt) == opt)
                        cb.Checked = true;
                }
            }
        }

        private ModelType GetCurrentModelType()
        {
            if (modelTypeCmb.Text == "")
                return ModelType.BA;
            return (ModelType)Enum.Parse(typeof(ModelType), modelTypeCmb.Text);
        }

        private StorageType GetCurrentStorageType()
        {
            if (storageTypeCmb.Text == "")
                return StorageType.ExcelStorage;
            return (StorageType)Enum.Parse(typeof(StorageType), storageTypeCmb.Text);
        }

        private GenerationType GetCurrentGenerationType()
        {
            if (generationTypeCmb.Text == "")
                return GenerationType.Static;
            return (GenerationType)Enum.Parse(typeof(GenerationType), generationTypeCmb.Text);
        }

        private void InitializeModelTypeCmb()
        {
            modelTypeCmb.Items.Clear();
            modelTypeCmb.Sorted = true;
            foreach (ModelType m in SessionManager.GetAvailableModelTypes(rType))
                modelTypeCmb.Items.Add(m.ToString());

            if (dType != DialogType.Create)
                modelTypeCmb.SelectedText = SessionManager.GetResearchModelType(researchId).ToString();
            else
            {
                Debug.Assert(modelTypeCmb.Items.Count != 0);
                modelTypeCmb.SelectedIndex = 0;
            }
        }

        private void InitializeStorageTypeCmb()
        {
            storageTypeCmb.Items.Clear();
            storageTypeCmb.Sorted = true;
            string[] storageTypeNames = Enum.GetNames(typeof(StorageType));
            for (int i = 0; i < storageTypeNames.Length; ++i)
                storageTypeCmb.Items.Add(storageTypeNames[i]);

            if (dType != DialogType.Create)
                storageTypeCmb.SelectedText = SessionManager.GetResearchStorageType(researchId).ToString();
            else
            {
                Debug.Assert(storageTypeCmb.Items.Count != 0);
                storageTypeCmb.SelectedIndex = 0;
            }
        }

        private void InitializeGenerationTypeCmb()
        {
            generationTypeCmb.Items.Clear();
            generationTypeCmb.Sorted = true;
            foreach (GenerationType g in SessionManager.GetAvailableGenerationTypes(rType))
                generationTypeCmb.Items.Add(g.ToString());

            if (dType != DialogType.Create)
            {
                generationTypeCmb.SelectedText = SessionManager.GetResearchGenerationType(researchId).ToString();
                if (GetCurrentGenerationType() == GenerationType.Static)
                    realizationCountTxt.Enabled = false;
            }
            else
            {
                Debug.Assert(generationTypeCmb.Items.Count != 0);
                generationTypeCmb.SelectedIndex = 0;
            }
        }

        private String RetrieveStorageString(StorageType stType)
        {
            switch (stType)
            {
                case StorageType.XMLStorage:
                case StorageType.ExcelStorage:
                case StorageType.TXTStorage:
                    return RandNetSettings.StorageDirectory;
                default:
                    return null;
            }
        }

        private Type GetResearchParameterType(ResearchParameter r)
        {
            ResearchParameterInfo rInfo = (ResearchParameterInfo)(r.GetType().GetField(r.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false)[0]);
            return rInfo.Type;
        }

        private Type GetGenerationParameterType(GenerationParameter g)
        {
            GenerationParameterInfo gInfo = (GenerationParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
            return gInfo.Type;
        }

        private string GetDefaultValueForResearchParameter(ResearchParameter r)
        {
            ResearchParameterInfo rInfo = (ResearchParameterInfo)(r.GetType().GetField(r.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false)[0]);
            return rInfo.DefaultValue;
        }

        private string GetDefaultValueForGenerationParameter(GenerationParameter g)
        {
            GenerationParameterInfo gInfo = (GenerationParameterInfo)(g.GetType().GetField(g.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
            return gInfo.DefaultValue;
        }

        private Object GetResearchParameterValue(ResearchParameter rp)
        {
            if (dType == DialogType.Create)
                return GetDefaultValueForResearchParameter(rp);
            Dictionary<ResearchParameter, Object> rv = SessionManager.GetResearchParameterValues(researchId);
            if (rv.ContainsKey(rp))
                return rv[rp];
            else
                return GetDefaultValueForResearchParameter(rp);
        }

        private Object GetGenerationParameterValue(GenerationParameter gp)
        {
            if (dType == DialogType.Create)
                return GetDefaultValueForGenerationParameter(gp);
            Dictionary<GenerationParameter, Object> gv = SessionManager.GetGenerationParameterValues(researchId);
            if (gv.ContainsKey(gp))
                return gv[gp];
            else
                return GetDefaultValueForGenerationParameter(gp);
        }

        private void AddLabelToParametersPanel(string text, int x, int y)
        {
            Label l = new Label();
            l.AutoSize = true;
            l.Text = text + ":";
            l.Location = new Point(x, y * 30 + 5);
            parametersPanel.Controls.Add(l);
        }

        private void AddTextBoxToParametersPanel(string text, Object value, Type vType, int x, int y)
        {
            TextBox tb = new TextBox();
            tb.Name = text;
            tb.Location = new Point(x, y * 30 + 5);
            tb.CausesValidation = true;
            if (vType == typeof(UInt32))
                tb.Validating += new CancelEventHandler(integer_validator);
            else if (vType == typeof(Double))
                tb.Validating += new CancelEventHandler(float_validator);
            tb.Text = value.ToString();
            parametersPanel.Controls.Add(tb);
        }

        private void integer_validator(object sender, CancelEventArgs e)
        {
            Debug.Assert(sender is TextBox);
            TextBox tb = sender as TextBox;
            UInt32 n = 0;
            bool b = UInt32.TryParse(tb.Text, out n);
            if (!b)
            {
                MessageBox.Show(tb.Name + " parameter value should be non negative integer.", "Error");
                e.Cancel = true;
            }
            else
                e.Cancel = false;
        }

        private void float_validator(object sender, CancelEventArgs e)
        {
            Debug.Assert(sender is TextBox);
            TextBox tb = sender as TextBox;
            Double n = 0;
            bool b = Double.TryParse(tb.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out n);
            if (!b)
            {
                MessageBox.Show(tb.Name + " parameter value should be real number.", "Error");
                e.Cancel = true;
            }
            else
                e.Cancel = false;
        }

        private void AddCheckBoxToParametersPanel(string text, Object value, int x, int y)
        {
            CheckBox cb = new CheckBox();
            cb.Name = text;
            cb.AutoSize = true;
            cb.Text = text;
            cb.Location = new Point(x, y * 30 + 5);
            cb.Checked = (value is string) ? (value.ToString() == "true") : (Boolean)value;
            parametersPanel.Controls.Add(cb);
        }

        // Not supported
        private void AddComboBoxToParametersPanel(string text, Object value, Type eType, int x, int y)
        {
            ComboBox cb = new ComboBox();
            cb.Name = text;
            cb.Location = new Point(x, y * 30 + 5);
            cb.SelectedText = value.ToString();
            parametersPanel.Controls.Add(cb);
        }

        private void AddFileInputToParametersPanel(string text, FileInputType t, Object value, int x, int y)
        {
            FileInput fi = new FileInput();
            fi.Name = text;
            fi.Type = t;
            fi.Location = new Point(x, y * 30 + 5);
            if (value != null)
            {
                MatrixPath mp = (MatrixPath)value;
                fi.MatrixPath = mp.Path;
                fi.MatrixSize = mp.Size;
            }
            parametersPanel.Controls.Add(fi);
        }

        private void ChangeCheckedStatesForAnalyzeOptions(bool b)
        {
            foreach (Control c in optionsPanel.Controls)
            {
                Debug.Assert(c is CheckBox);
                CheckBox cc = c as CheckBox;
                cc.Checked = b;
            }
        }

        private void SetGeneralValues()
        {
            SessionManager.SetResearchName(ResultResearchId, researchNameTxt.Text);
            SessionManager.SetResearchModelType(ResultResearchId, GetCurrentModelType());
            StorageType st = GetCurrentStorageType();
            SessionManager.SetResearchStorage(ResultResearchId, st, RetrieveStorageString(st));
            SessionManager.SetResearchGenerationType(ResultResearchId, GetCurrentGenerationType());
            string p = tracingCheck.Checked ? RandNetSettings.TracingDirectory : "";
            SessionManager.SetResearchTracingPath(ResultResearchId, p);
            SessionManager.SetResearchCheckConnectedh(ResultResearchId, checkConnected.Checked);
            SessionManager.SetResearchRealizationCount(ResultResearchId, (int)realizationCountTxt.Value);
        }

        private void SetParameterValues()
        {
            foreach (Control c in parametersPanel.Controls)
            {
                if (c is Label)
                    continue;
                string pName = c.Name;
                Object pValue = GetValueFromControl(c);
                ResearchParameter rp;
                GenerationParameter gp;
                if (Enum.TryParse(pName, out rp))
                    SessionManager.SetResearchParameterValue(ResultResearchId, rp, pValue);
                else if (Enum.TryParse(pName, out gp))
                    SessionManager.SetGenerationParameterValue(ResultResearchId, gp, pValue);
                else
                    throw new CoreException("Unknown parameter.");
            }
        }

        private Object GetValueFromControl(Control c)
        {
            Debug.Assert(!(c is Label));
            if (c is TextBox)
                return (c as TextBox).Text;
            else if (c is CheckBox)
                return (c as CheckBox).Checked;
            else if (c is ComboBox)
                return (c as ComboBox).Text;
            else if (c is FileInput)
            {
                FileInput fi = c as FileInput;
                MatrixPath mp = new MatrixPath();
                mp.Path = fi.MatrixPath;
                mp.Size = fi.MatrixSize;
                return mp;
            }
            else
            {
                Debug.Assert(false);
                return null;
            }
        }

        private void SetAnalyzeOptionsValues()
        {
            AnalyzeOption opts = SessionManager.GetAnalyzeOptions(ResultResearchId);
            foreach (Control c in optionsPanel.Controls)
            {
                Debug.Assert(c is CheckBox);
                CheckBox cc = c as CheckBox;
                AnalyzeOption current = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), cc.Name);
                if (cc.Checked)
                    opts |= current;
                else
                    opts &= ~current;
            }
            SessionManager.SetAnalyzeOptions(ResultResearchId, opts);
        }

        #endregion
    }
}
