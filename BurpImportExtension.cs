/******************************************************************
* Licensed Materials - Property of HCL
* (c) Copyright HCL Technologies Ltd. 2015, 2022.
* Note to U.S. Government Users Restricted Rights.
******************************************************************/
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AppScan;
using AppScan.Extensions;

namespace BurpTrafficImporter
{
    class BurpImportExtension : IExtensionLogic, IExtensionSDKLogic
    {
        private Form mainForm;

        SynchronizationContext _uiContext;
        BurpImportForm _bif;

        private IAppScan _appScan;

        public void Load(IAppScan appScan, IAppScanGui appScanGui, string extensionDir)
        {
            appScanGui.ExtensionsMenu.Add(new MenuItem<EventArgs>("Import Burp Traffic",ImportBurpTraffic));
            appScanGui.MainFormStarted += AppScanGuiOnMainFormStarted;
            _uiContext = SynchronizationContext.Current;
            _bif = new BurpImportForm(mainForm, appScan);
            _appScan = appScan;
        }

        private void AppScanGuiOnMainFormStarted(object sender, MainFormStartedEventArgs mainFormStartedEventArgs)
        {
            mainForm = (Form)(mainFormStartedEventArgs.MainForm);
        }

        private void ImportBurpTraffic(EventArgs args)
        {
            _bif = new BurpImportForm(mainForm, _appScan);
            _bif.FormClosed += BifOnFormClosed;
            _uiContext.Send(delegate
            {
                //This executes on the ui thread
                _bif.Show(mainForm);
            }, null);
        }

        private void BifOnFormClosed(object sender, FormClosedEventArgs formClosedEventArgs)
        {
            if (_bif.ShouldImport)
            {
                string exdFile = string.Empty;
                try
                {
                    _uiContext.Send(delegate
                    {
                        if (_bif.SetStartingPointUrl)
                        {
                            _appScan.Scan.ScanData.Config.StartingUrl = _bif.StartingPointUrl;
                        }
                        foreach (string domain in _bif.AdditionalDomains)
                        {
                            _appScan.Scan.ScanData.Config.AdditionalServers.Add(domain);
                        }

                        BurpToExd b2e = new BurpToExd();
                        exdFile = Path.GetFileNameWithoutExtension(_bif.BurpTrafficFile);
                        exdFile += ".converted.exd";
                        exdFile = Path.Combine(_appScan.AppScanTempDir, exdFile);
                        b2e.Convert(_bif.BurpTrafficFile, exdFile);
                        using (FileStream fs = new FileStream(exdFile, FileMode.Open))
                        {
                            _appScan.Scan.RequestRecorder.ImportRecordedRequests(fs, true);
                            _appScan.Scan.RequestRecorder.Analyse();
                            _appScan.Scan.AnalysisEnded += (o,e) =>
                            {
                                ClearFiles(exdFile);
                            };
                        }
                    }, null);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error");
                    ClearFiles(exdFile);         
                }
            }
        }

        private void ClearFiles(string exdFile)
        {
            if (File.Exists(exdFile))
            {
                File.Delete(exdFile);
            }

            if (File.Exists(_bif.BurpTrafficFile))
            {
                File.Delete(_bif.BurpTrafficFile);
            }

            _bif.BurpTrafficFile = string.Empty;
        }

        public ExtensionVersionInfo GetUpdateData(Edition edition, Version targetAppVersion)
        {
            return null;
        }

        public void Init(IAppScan appScan, string extensionDir)
        {
            
        }

        public void Run(object data)
        {
            
        }

        public ConfigurationErrorCode SetConfiguration(string configurationItemName, object value)
        {
            return ConfigurationErrorCode.NoError;
        }

        public ConfigurationErrorCode GetConfiguration(string configurationItemName, out object value)
        {
            value = null;
            return ConfigurationErrorCode.NoError;
        }

        public ConfigurationErrorCode GetConfigurationMinimumMaximumValue(string configurationItemName, out object minValue, out object maxValue)
        {
            minValue = null;
            maxValue = null;
            return ConfigurationErrorCode.NoError;
        }

    }
}
