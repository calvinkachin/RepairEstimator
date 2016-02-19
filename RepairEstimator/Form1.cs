using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RepairEstimator
{
    public partial class Form1 : Form
    {
        List<string> UsedParts = new List<string>();
        List<ListViewItem> VirtualList = new List<ListViewItem>();
        double min = 0;
        double max = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void ReadProblems(string n)
        {
            if (n != "")
            {
                try
                {
                    var reader = new StreamReader(File.OpenRead(@"T:\Databases\"+n + "_Problems.csv"));
                    List<string> listProb = new List<string>();


                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        listProb.Add(values[0]);

                    }
                    reader.Dispose();
                    listProblems.Items.Clear();
                    listProblems.Items.AddRange(listProb.ToArray());
                }
                catch
                {
                    MessageBox.Show(n + "_Problems.csv is currently open in another program. Please close it and retry.");
                }
            }
        }

        private void UpdateReport()
        {
            lvwReport.Items.Clear();
            foreach (ListViewItem lvi in VirtualList)
            {
                ListViewItem clonelvi = new ListViewItem(lvi.Text);
                clonelvi.SubItems.Add(lvi.SubItems[1].Text);
                clonelvi.SubItems.Add(lvi.SubItems[2].Text);

                lvwReport.Items.Add(clonelvi);
            }
            
            if (chkPartNames.Checked == true)
            {
                GetNames();
            }

            GetPricing();

        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex >= 0)
            {
                ClearForm();
                btnNewEntry.Visible = true;

                
                Util.Animate(picProduct, Util.Effect.Slide, 100, 0);
                picProduct.Visible = false;

                switch (cmbProduct.SelectedIndex)
                {
                    case 0:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.E_SERIES;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 1:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.M_SERIES;
                        chkMSeries.Visible = true;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 2:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.CCT;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 3:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.R_SERIES;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 4:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.X_SERIES;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 5:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.AED_PLUS;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 6:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.AED_PRO;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 7:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.AUTOPULSE;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    case 8:
                        picProduct.BackgroundImage = RepairEstimator.Properties.Resources.PROPAQ;
                        chkMSeries.Visible = false;
                        ReadProblems(cmbProduct.Text);
                        break;
                    default:
                        break;
                }

                Util.Animate(picProduct, Util.Effect.Slide, 100, 180);
                picProduct.Visible = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ReadProblems(cmbProduct.Text);


            List<string> results = new List<string>();
            List<string> search = new List<string>();

            search.AddRange(txtSearch.Text.ToUpper().Split(' ').ToArray());

            foreach (string q in search)
            {
                foreach (string item in listProblems.Items)
                {
                    if (item.Contains(q))
                    {
                        results.Add(item);
                    }
                }
                listProblems.Items.Clear();
                listProblems.Items.AddRange(results.ToArray());
                results.Clear();
            }
        }

        private void listProblems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listProblems.SelectedIndex >= 0)
            {
                bool exists = false;

                foreach(ListViewItem lvi in VirtualList)
                {
                    if(lvi.SubItems[0].Text== listProblems.Items[listProblems.SelectedIndex].ToString())
                    {
                        exists = true;
                    }
                }

                if (!exists)
                    {
                        try
                        {
                        ListViewItem lvi = new ListViewItem(listProblems.Items[listProblems.SelectedIndex].ToString());
                        lvi.SubItems.Add(ReadSolutions(listProblems.Items[listProblems.SelectedIndex].ToString()));
                        lvi.SubItems.Add("N");
                        
                        VirtualList.Add(lvi);
                        UpdateReport();
                        
                        

                        }
                        catch
                        {
                            MessageBox.Show(cmbProduct.Text + "_Problems.csv is currently open in another program. Please close it and retry.");
                        }

                    }
            }
        }

        private void lvwReport_DoubleClick(object sender, EventArgs e)
        {
            if (lvwReport.SelectedIndices.Count > 0)
            {
                VirtualList.RemoveAt(lvwReport.SelectedIndices[0]);
                chkAbuse.Visible = false;

                UpdateReport();
                
            }
        }

        private void GetNames()
        {
            foreach(ListViewItem lvi in lvwReport.Items)
            {
                List<string> values = new List<string>(lvi.SubItems[1].Text.Split(','));

                foreach (string pn2 in values)
                {
                    List<string> values2 = new List<string>(pn2.Split('+'));

                    foreach (string pn in values2) {
                        var reader = new StreamReader(File.OpenRead(@"T:\Databases\Parts_List.csv"));

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var parts = line.Split(',');

                            if (parts[0] == pn)
                            {
                                lvi.SubItems[1].Text = lvi.SubItems[1].Text.Replace(pn, parts[1]);
                                break;
                            }
                        }

                        reader.Dispose();
                    }
                }
             
                lvi.SubItems[1].Text = lvi.SubItems[1].Text.Replace(","," or ");
            }

        }

        private void GetPricing()
        {
            min = 0;
            max = 0;
            UsedParts.Clear();



            foreach (ListViewItem lvi in VirtualList)
            {
                if (lvi.SubItems[1].Text.Contains(","))
                {
                    int indmin = 0;
                    int indmax = 0;

                    List<string> values = new List<string>(lvi.SubItems[1].Text.Split(','));

                    foreach (string pn2 in values)
                    {
                        if (!UsedParts.Contains(pn2) || lvi.SubItems[2].Text=="Y")
                        {
                            if (!UsedParts.Contains(pn2))
                            {
                                UsedParts.Add(pn2);
                            }

                            List<string> values2 = new List<string>(pn2.Split('+'));
                            int combined = 0;

                            foreach (string pn in values2)
                            {

                                var reader = new StreamReader(File.OpenRead(@"T:\Databases\Parts_List.csv"));

                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var parts = line.Split(',');


                                    if (parts[0] == pn)
                                    {
                                        combined = combined + Int32.Parse(parts[3]);
                                        break;
                                    }
                                }
                                reader.Dispose();
                            }

                            if (indmin <= 0 && indmax <= 0)
                            {
                                indmin = combined;
                                indmax = combined;
                            }
                            else
                            {
                                if (combined > indmax)
                                {
                                    indmax = combined;
                                }
                                if (combined < indmin)
                                {
                                    indmin = combined;
                                }
                            }

                        }

                        //lvi.SubItems[1].Text = lvi.SubItems[1].Text.Replace(",", " or ");

                    }
                    if (chkWarranty.Checked == true)
                    {
                        if (chkFrench.Checked == true)
                        {
                            if (lvi.SubItems[2].Text == "Y")
                            {
                                indmin = indmin / 2;
                                indmax = indmax / 2;
                            }
                        }

                        if (lvi.SubItems[2].Text == "N")
                        {
                            indmin = 0;
                            indmax = 0;
                        }
                    }
                    min = min + indmin;
                    max = max + indmax;
                }
            }

            foreach (ListViewItem lvi in VirtualList)
            {
                if (!lvi.SubItems[1].Text.Contains(","))
                {
                    int indmin = 0;
                    int indmax = 0;

                    List<string> values = new List<string>(lvi.SubItems[1].Text.Split(','));

                    foreach (string pn2 in values)
                    {
                        if (!UsedParts.Contains(pn2) || lvi.SubItems[2].Text == "Y")
                        {
                            if (!UsedParts.Contains(pn2))
                            {
                                UsedParts.Add(pn2);
                            }
                            List<string> values2 = new List<string>(pn2.Split('+'));
                            int combined = 0;

                            foreach (string pn in values2)
                            {

                                var reader = new StreamReader(File.OpenRead(@"T:\Databases\Parts_List.csv"));

                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    var parts = line.Split(',');


                                    if (parts[0] == pn)
                                    {
                                        combined = combined + Int32.Parse(parts[3]);
                                        break;
                                    }
                                }
                                reader.Dispose();
                            }

                            if (indmin <= 0 && indmax <= 0)
                            {
                                indmin = combined;
                                indmax = combined;
                            }
                            else
                            {
                                if (combined > indmax)
                                {
                                    indmax = combined;
                                }
                                if (combined < indmin)
                                {
                                    indmin = combined;
                                }
                            }

                        }

                        //lvi.SubItems[1].Text = lvi.SubItems[1].Text.Replace(",", " or ");

                    }
                    if (chkWarranty.Checked == true)
                    {
                        if (chkFrench.Checked == true)
                        {
                            if (lvi.SubItems[2].Text == "Y")
                            {
                                indmin = indmin / 2;
                                indmax = indmax / 2;
                            }
                        }

                        if (lvi.SubItems[2].Text == "N")
                        {
                            indmin = 0;
                            indmax = 0;
                        }
                    }
                    min = min + indmin;
                    max = max + indmax;
                }
            }

            if (chkMSeries.Checked == true)
                {
                    min = min * 1.15;
                    max = max * 1.15;
                }
            
            min = min + PM_Fee() + LabourHours() + Shipping_Fee();
            max = max + PM_Fee() + LabourHours() + Shipping_Fee();

            

            lblMin.Text = "$"+min.ToString()+"  to  $"+max.ToString();
            
        }

        private int LabourHours()
        {
            int i = 0;
            
            var reader = new StreamReader(File.OpenRead(@"T:\Databases\Labour.csv"));

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (cmbProduct.Text == values[0])
                {
                    i = Convert.ToInt32(Convert.ToDecimal(values[1]) * Convert.ToDecimal(cmbLabour.SelectedItem.ToString()));
                }

            }

            if (chkWarranty.Checked == true)
            {
                if (chkFrench.Checked == true)
                {
                    bool abused = false;

                    foreach (ListViewItem lvi in VirtualList)
                    {
                        if (lvi.SubItems[2].Text=="Y")
                        {
                            abused = true;
                        }
                    }

                    if (abused == true)
                    {
                        i = i / 2;
                    }
                    else
                    {
                        i = 0;
                    }
                }
                else
                {
                    bool abused = false;

                    foreach (ListViewItem lvi in VirtualList)
                    {
                        if (lvi.SubItems[2].Text == "Y")
                        {
                            abused = true;
                        }
                    }

                    if (abused == true)
                    {
                        i = i/1;
                    }
                    else
                    {
                        i = 0;
                    }
                }
            }
            
            //Dispose of reader and add lists to listboxs
            reader.Dispose();
            return (i);
        }

        private int PM_Fee()
        {
            int i = 0;
            if (chkPM.Checked == true)
            {
                var reader = new StreamReader(File.OpenRead(@"T:\Databases\PM.csv"));


                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (cmbProduct.Text == values[0])
                    {
                        i = Convert.ToInt32(values[1]);
                    }
                }

                //Dispose of reader and add lists to listboxs
                reader.Dispose();
            }
            return (i);
        }

        private string ReadSolutions(string problem)
        {
                var reader = new StreamReader(File.OpenRead(@"T:\Databases\"+cmbProduct.Text + "_Problems.csv"));
               

                //Find solutions matching to the problems
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    //Does it match the problem?
                    if (values[0] == problem)
                    {
                        List<string> valueslist = new List<string>();

                        //Adds entire list of solutions, then removes the first cell (the problem), and all empty cells
                        valueslist.AddRange(values);
                        valueslist.RemoveAt(0);
                        valueslist = valueslist.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                    string repairs = "";

                    foreach (string i in valueslist)
                    {
                        if (repairs != "")
                        {
                            repairs = repairs +","+ i;
                        }
                        else
                        {
                            repairs = i;
                        }
                    }
                    

                        reader.Dispose();
                        return repairs;
                    }
                }
                reader.Dispose();
            return "";
        }
        
        private void ClearForm()
        {


            lvwReport.Items.Clear();
            VirtualList.Clear();
            UsedParts.Clear();

            min = 0;
            max = 0;
            chkAbuse.Checked = false;
            chkAbuse.Visible = false;

            lblMin.Text = "$" + min.ToString() + "  to  $" + max.ToString();
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void lvwReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (lvwReport.SelectedIndices.Count > 0)
            {
                if(chkAbuse.Visible==false)
                Util.Animate(chkAbuse, Util.Effect.Slide, 90, 270);
                chkAbuse.Visible = true ;
                

                if (VirtualList[lvwReport.SelectedIndices[0]].SubItems[2].Text == "Y")
                {
                    chkAbuse.Checked = true;
                }
                else
                {
                    chkAbuse.Checked = false;
                }
                
            }
        }

        private int Shipping_Fee()
        {
            int i = 0;
            if (chkShipping.Checked == true)
            {
                var reader = new StreamReader(File.OpenRead(@"T:\Databases\Shipping.csv"));


                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (cmbProduct.Text == values[0])
                    {
                        i = Convert.ToInt32(values[1]);
                    }

                }

                //Dispose of reader and add lists to listboxs
                reader.Dispose();
            }
            return (i);
        }


        private void chkPartNames_CheckedChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        private void chkPM_CheckedChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        private void chkShipping_CheckedChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        private void cmbLabour_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        private void chkWarranty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWarranty.Checked == true)
            {
                chkShipping.Checked = false;
                chkPM.Checked = false;
                grpWarranty.Enabled = false;

            }
            else
            {
                chkShipping.Checked = true;
                chkPM.Checked = true;
                grpWarranty.Enabled = true;

            }
            UpdateReport();

        }

        private void chkAbuse_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chkFrench_CheckedChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        private void chkMSeries_CheckedChanged(object sender, EventArgs e)
        {
            UpdateReport();
        }

        private void chkMSeries_VisibleChanged(object sender, EventArgs e)
        {
            if (chkMSeries.Visible == true)
            {
                chkMSeries.Checked = true;
            }
            else
            {
                chkMSeries.Checked = false;
            }
        }
        

        private void txtSR_TextChanged(object sender, EventArgs e)
        {

        }

        private void CheckDirectory(string sr)
        {
            string path = @"T:\! SR FOLDERS\" + sr;

            if (Directory.Exists(path) == false)
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
        }

        private void GenerateEstimate()
        {
            btnGenerate.Text = "Submit Estimate";

            Util.Animate(lblSR, Util.Effect.Slide, 90, 270);
            lblSR.Visible = false;

            Util.Animate(txtSR, Util.Effect.Slide, 90, 270);
            txtSR.Visible = false;


            CheckDirectory(txtSR.Text);
            string outputFileName = @"T:\! SR FOLDERS\" + txtSR.Text + "\\problems.csv";

            var writer = new StreamWriter(txtSR.Text+".csv");

            writer.WriteLine(cmbProduct.Text);

            foreach (ListViewItem lvi in VirtualList)
            {
                writer.WriteLine(lvi.SubItems[0].Text);
            }

            writer.Close();
            //Create folder if not exist
            //Generates csv with problems
            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtSR.Visible == true)
            {
                if (txtSR.Text != "")
                {
                    GenerateEstimate();
                }
            }
            else
            {
                txtSR.Clear();
                btnGenerate.Text = "Confirm";

                Util.Animate(txtSR, Util.Effect.Slide, 90, 180);
                txtSR.Visible = true;
                Util.Animate(lblSR, Util.Effect.Slide, 90, 270);
                lblSR.Visible = true;
                
                txtSR.Focus();
            }
        }

        private void lvwReport_Leave(object sender, EventArgs e)
        {
        }

        private void lvwReport_Click(object sender, EventArgs e)
        {
            if (lvwReport.SelectedIndices.Count <= 0)
            {
                Util.Animate(chkAbuse, Util.Effect.Slide, 90, 270);
                chkAbuse.Visible = false;
            }
        }

        private void chkAbuse_MouseClick(object sender, MouseEventArgs e)
        {
            if (lvwReport.SelectedIndices.Count > 0)
            {

                if (chkAbuse.Checked == true)
                {
                    VirtualList[lvwReport.SelectedIndices[0]].SubItems[2].Text = "Y";

                }
                else
                {
                    VirtualList[lvwReport.SelectedIndices[0]].SubItems[2].Text = "N";
                }

            }
            UpdateReport();
        }

        private void txtSR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSR.Text != "")
                {
                    GenerateEstimate();
                }
            }
        }

        private void InsertNewEntry()
        {
            btnNewEntry.Text = "New Entry";

            bool exists = false;

            foreach (ListViewItem lvi in VirtualList)
            {
                if (listProblems.SelectedIndex >= 0)
                {
                    if (lvi.SubItems[0].Text == listProblems.Items[listProblems.SelectedIndex].ToString())
                    {
                        exists = true;
                    }
                }
                else
                {
                    if (lvi.SubItems[0].Text == txtNew.Text)
                    {
                        exists = true;
                    }
                }


            }

            if (!exists)
            {
                ListViewItem lvi = new ListViewItem(txtNew.Text);
                lvi.SubItems.Add("???");
                lvi.SubItems.Add("N");

                VirtualList.Add(lvi);
                UpdateReport();
            }

            Util.Animate(txtNew, Util.Effect.Slide, 95, 180);
            txtNew.Visible = false;

            Util.Animate(lblNew, Util.Effect.Slide, 95, 180);
            lblNew.Visible = false;

            Util.Animate(listProblems, Util.Effect.Slide, 90, 0);
            listProblems.Visible = true;
        }

        private void btnNewEntry_Click(object sender, EventArgs e)
        {
            if (txtNew.Visible == true)
            {
                if (txtNew.Text != "")
                {
                    InsertNewEntry();
                }
                else
                {
                    Util.Animate(txtNew, Util.Effect.Slide, 95, 270);
                    txtNew.Visible = false;

                    Util.Animate(lblNew, Util.Effect.Slide, 95, 270);
                    lblNew.Visible = false;

                    Util.Animate(listProblems, Util.Effect.Slide, 90, 0);
                    listProblems.Visible = true;
                }
            }
            else
            {
                txtNew.Clear();
                btnNewEntry.Text = "Add";

                Util.Animate(listProblems, Util.Effect.Slide, 90, 0);
                listProblems.Visible = false;

                Util.Animate(txtNew, Util.Effect.Slide, 90, 270);
                txtNew.Visible = true;

                Util.Animate(lblNew, Util.Effect.Slide, 90, 270);
                lblNew.Visible = true;

                txtNew.Focus();
            }
        }

        private void txtNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InsertNewEntry();
            }
        }
    }
}
