/******************************************************************
* Licensed Materials - Property of HCL
* (c) Copyright HCL Technologies Ltd. 2015, 2017.
* Note to U.S. Government Users Restricted Rights.
******************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Watchfire.EngineAPI;

namespace BurpTrafficImporter
{
    public partial class BurpImportForm : Form
    {
        private readonly IWin32Window _owner;
        private readonly string _useFirstText;
        private string _startingPointUrl;

        public BurpImportForm(Form owner)
        {
            Owner = owner;
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            _owner = owner;
            _useFirstText = checkBoxUseFirstAsSTP.LabelControl.Text;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (!string.IsNullOrEmpty(textBoxSourceFilePath.Text))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(textBoxSourceFilePath.Text);
                ofd.FileName = Path.GetFileName(textBoxSourceFilePath.Text);
            }

            DialogResult res = (_owner == null) ? ofd.ShowDialog() : ofd.ShowDialog(_owner);

            if (res == DialogResult.OK)
            {
                textBoxSourceFilePath.Text = ofd.FileName;

                HandleNewFile(ofd.FileName);
            }
        }

        private void textBoxSourceFilePath_TextChanged(object sender, EventArgs e)
        {
            HandleNewFile(textBoxSourceFilePath.Text);
        }

        private void HandleNewFile(string filePath)
        {
            if (File.Exists(filePath) && _burpTrafficFile == "")
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    string line;
                    string path = EngineRegistries.EngineOptions.AppScanTempDir;
                    string id = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                    if (!path.Contains(id))
                    {
                        path = Path.Combine(path, id);
                    }
                    path = Path.Combine(path, "burp.tmp");


                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        using (StreamWriter writer = new StreamWriter(path))
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains("?xml version=\"1.1\""))
                                {
                                    line = line.Replace("?xml version=\"1.1\"", "?xml version=\"1.0\"");
                                }
                                writer.WriteLine(line);
                            }
                        }
                    }

                    _burpTrafficFile = path;

                    xmlDoc.Load(_burpTrafficFile);

                    XmlNode firstUrl = xmlDoc.SelectSingleNode("items/item/url");
                    if (firstUrl != null)
                    {
                        _startingPointUrl = firstUrl.InnerText;
                        checkBoxUseFirstAsSTP.LabelControl.Text = _useFirstText + " - " + _startingPointUrl;
                        checkBoxUseFirstAsSTP.LabelControl.Refresh();
                    }

                    XmlNodeList hosts = xmlDoc.SelectNodes("items/item/host");
                    if (hosts != null)
                    {
                        string first = hosts[0].InnerText;
                        for (int i = 1; i < hosts.Count; i++)
                        {
                            if (hosts[i].InnerText != first)
                            {
                                checkedListBoxAdditionalDomains.Items.Add(hosts[i].InnerText);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error");
                    
                    BurpTrafficFile = "";
                    textBoxSourceFilePath.Text = "";
                    checkBoxUseFirstAsSTP.LabelControl.Text = _useFirstText;
                    checkedListBoxAdditionalDomains.Items.Clear();
                }
            }
            else
            {
                checkBoxUseFirstAsSTP.LabelControl.Text = _useFirstText;
                checkedListBoxAdditionalDomains.Items.Clear();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ShouldImport = false;
            Close();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            ShouldImport = true;
            if (_burpTrafficFile != "")
            {
                Close(); 
            }  
        }

        public bool ShouldImport { get; private set; }

        private string _burpTrafficFile = "";
        public string BurpTrafficFile
        {
            get { return _burpTrafficFile == "" ? textBoxSourceFilePath.Text : _burpTrafficFile; }
            set { _burpTrafficFile = value; }
        }


        public bool SetStartingPointUrl { get { return checkBoxUseFirstAsSTP.CheckBoxControl.Checked; } }
        public string StartingPointUrl { get { return _startingPointUrl; } }

        public IEnumerable<string> AdditionalDomains
        {
            get 
            {
                return checkedListBoxAdditionalDomains.CheckedItems.Cast<string>();
            }
        }
    }
}
