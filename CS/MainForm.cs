﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid;

namespace FormatCondititonsDesigner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            countriesTableAdapter1.Fill(countriesDBDataSet1.Countries);
            ViewFormatDesigner newFormatDesigner = new ViewFormatDesigner(pivotGridControl1);
        }
    }
}
