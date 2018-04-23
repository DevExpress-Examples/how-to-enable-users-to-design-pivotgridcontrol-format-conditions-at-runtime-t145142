﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;

namespace FormatCondititonsDesigner
{
    public partial class CondititonItemControl : UserControl
    {
        BaseEdit basEditValue1;
        BaseEdit basEditValue2;
        Type currentColumnType = null;
        PivotGridStyleFormatCondition currentFormatCondition;

        public event EventHandler RefreshValues;

        public CondititonItemControl()
        {
            InitializeComponent();
            InitConditions();
            comboBoxEditCondititon.EditValueChanged += new EventHandler(CurrentEditor_EditValueChanged);
            checkEditApplyToCell.EditValueChanged += new EventHandler(CurrentEditor_EditValueChanged);
            checkEditApplyToTotal.EditValueChanged += new EventHandler(CurrentEditor_EditValueChanged);
            checkEditApplyToGrandTotal.EditValueChanged += new EventHandler(CurrentEditor_EditValueChanged);
        }

        public void Init(ItemCondition item)
        {
            if (item != null)
            {
                currentFormatCondition = item.Condition;
                CreateValues(item.Condition.Field.DataType);
            }
            else
            {
                currentFormatCondition = null;
            }
            RefreshEditorsValues(item);
            UpdateEditors(item);
        }

        private void UpdateEditors(ItemCondition item)
        {
            comboBoxEditCondititon.Enabled = (bool)(item != null);
            propertyGridControlAppearance.Enabled = (bool)(item != null);
            if (basEditValue1 != null) basEditValue1.Visible = (bool)(item != null);
            if (basEditValue1 != null) basEditValue2.Visible = (bool)(item != null);
            checkEditApplyToCell.Enabled = (bool)(item != null);
        }

        void CreateValues(Type type)
        {
            if (type != currentColumnType)
            {
                currentColumnType = type;
                DisposeEditors();
                if (type.Equals(typeof(Boolean)))
                {
                    CreateEditorsAsCheckEdit();
                }
                else if (type.IsValueType)
                {
                    CreateEditorsAsSpinEdit(type);
                }
                else
                {
                    basEditValue1 = new TextEdit();
                    basEditValue2 = new TextEdit();
                }
                AddEditors();
                CalculateEditorsSizeLocation(type);
            }
        }

        void CurrentEditor_EditValueChanged(object sender, EventArgs e)
        {
            if (currentFormatCondition != null)
            {
                BaseEdit currentEditor = sender as BaseEdit;
                switch (currentEditor.Name)
                {
                    case "basEditValue1":
                        currentFormatCondition.Value1 = currentEditor.EditValue;
                        break;
                    case "basEditValue2":
                        currentFormatCondition.Value2 = currentEditor.EditValue;
                        break;
                    case "checkEditApplyToCell":
                        currentFormatCondition.ApplyToCell = (bool)currentEditor.EditValue;
                        break;
                    case "checkEditApplyToTotal":
                        currentFormatCondition.ApplyToTotalCell = (bool)currentEditor.EditValue;
                        break;
                    case "checkEditApplyToGrandTotal":
                        currentFormatCondition.ApplyToGrandTotalCell = (bool)currentEditor.EditValue;
                        break;
                    case "comboBoxEditCondititon":
                        currentFormatCondition.Condition = (FormatConditionEnum)currentEditor.EditValue;
                        break;
                }
                if (RefreshValues != null) RefreshValues(this, new EventArgs());
            }
        }

        void RefreshEditorsValues(ItemCondition item)
        {
            comboBoxEditCondititon.EditValue = item == null ? FormatConditionEnum.Equal : item.Condition.Condition;
            if (basEditValue1 != null && basEditValue2 != null)
            {
                basEditValue1.EditValue = item == null ? 0 : item.Condition.Value1;
                basEditValue2.EditValue = item == null ? 0 : item.Condition.Value2;
            }
            checkEditApplyToCell.EditValue = item == null ? false : item.Condition.ApplyToCell;
            checkEditApplyToTotal.EditValue = item == null ? false : item.Condition.ApplyToTotalCell;
            checkEditApplyToGrandTotal.EditValue = item == null ? false : item.Condition.ApplyToGrandTotalCell;
            propertyGridControlAppearance.SelectedObject = item == null ? null : item.Condition.Appearance;
        }

        void InitConditions()
        {
            Array values = System.Enum.GetValues(typeof(FormatConditionEnum));
            foreach (var item in values)
            {
                if ((FormatConditionEnum)item != FormatConditionEnum.Expression)
                {
                    comboBoxEditCondititon.Properties.Items.Add(item);
                }
            }
        }

        void DisposeEditors()
        {
            if (basEditValue1 != null && basEditValue2 != null)
            {
                basEditValue1.EditValueChanged -= new EventHandler(CurrentEditor_EditValueChanged);
                basEditValue2.EditValueChanged -= new EventHandler(CurrentEditor_EditValueChanged);
                this.Controls.Remove(basEditValue1);
                this.Controls.Remove(basEditValue2);
            }
        }

        void AddEditors()
        {
            basEditValue1.Name = "basEditValue1";
            basEditValue2.Name = "basEditValue2";
            this.Controls.Add(basEditValue1);
            this.Controls.Add(basEditValue2);
            basEditValue1.EditValueChanged += new EventHandler(CurrentEditor_EditValueChanged);
            basEditValue2.EditValueChanged += new EventHandler(CurrentEditor_EditValueChanged);
        }

        void CreateEditorsAsCheckEdit()
        {
            basEditValue1 = new CheckEdit();
            basEditValue2 = new CheckEdit();
            basEditValue1.Text = "Value1";
            basEditValue2.Text = "Value2";
            labelControlValue1.Visible = labelControlValue2.Visible = false;
        }

        void CreateEditorsAsSpinEdit(Type type)
        {
            basEditValue1 = new SpinEdit();
            basEditValue2 = new SpinEdit();
            if ("System.Int16System.Int32System.Int64System.Byte".IndexOf(type.ToString()) > -1)
            {
                (basEditValue1 as SpinEdit).Properties.IsFloatValue = false;
                (basEditValue2 as SpinEdit).Properties.IsFloatValue = false;
            }
        }

        void CalculateEditorsSizeLocation(Type type)
        {
            basEditValue1.Size = basEditValue2.Size = new Size(comboBoxEditCondititon.Width, comboBoxEditCondititon.Height);
            if (type.Equals(typeof(Boolean)))
            {
                basEditValue1.Location = labelControlValue1.Location;
                basEditValue2.Location = labelControlValue2.Location;
            }
            else
            {
                basEditValue1.Location = new Point(comboBoxEditCondititon.Location.X, labelControlValue1.Location.Y);
                basEditValue2.Location = new Point(comboBoxEditCondititon.Location.X, labelControlValue2.Location.Y);
            }
        }
    }
}
