# AppScan Standard - Burp Traffic Importer extension

This library is an extension for HCL AppScan Standard.

It allows the user to import an XML-formatted Burp traffic file containing a list of requests into AppScan. AppScan will explore these requests as if they were explored manually.

## Installation

To install, the extension must be imported into AppScan Standard using the AppScan extension manager.

1. In AppScan Standard, go to **Tools > Extensions > Extension Manager**
1. Import the extension BurpTrafficImporter.zip using **Add Extensions From > This Computer** 
1. Restart AppScan Standard for the change to take effect.
1. Open **Extension Manager**.
1. Click **Trust** to trust the extension.

## Usage

Once installed in AppScan, the following steps outline the proper usage:
1. Navigate to **Tools > Extensions > Import Burp Traffic**
2. Click **Browse** and select an XML-formatted Burp Traffic file (exported via the **Save Item(s)** context menu in Burp)
3. *(Optional)* Un-check Use first request as Starting Point URL if you don't want the extension to set the **Configuration > Starting URL** during Import. *Note: If a Starting URL is not assigned, AppScan Standard will assign one during the import of the Manual Explore data regardless of this option.*
4. *(Optional)* The **Select domains to add to Additional Domains** list will be populated with any additional domains found in the Burp file. Check any you want AppScan to test and the extension will add them to the **Configuration > Starting URLs & domains > Domains being tested** list.
5. Click **Import**
the extension will then read the Burp traffic file, convert it to a temporary EXD (EXplore Data) file and subsequently import it. AppScan will then explore these requests automatically. You can then decide whether to continue with a Test stage or more manual exploring.

## Building

Prerequisites:
- You must be able to compile a C# Project.
- You must have 7-zip installed (http://www.7-zip.org/).
- You must have AppScan Standard version 10.0.0 or later.


Skip this step if AppScan Standard is installed in the default folder:
"C:\Program Files (x86)\HCL\AppScan Standard\"

Attach DLLs from AppScan install folder:  
	a) Open “BurpTrafficImporter.sln” with Visual Studio or other IDE of your choice. 
	b) Locate and right-click on BurpTrafficImporter project. 
	c) Click **Add > Reference**  
	d) In the Browse section, select Browse and add the following DLLs from the AppScan folder:  
		- AppScanSDK.dll  
		- HttpProxy.dll  
		- CommonEngineProvider.dll  
		- UserControls.dll  
		- ScanTypes.dll  
		- InfraTypes.dll  
The file `BurpTrafficImporterExt.zip` is automatically generated in the sub folder: \appscan-standard-BurpTrafficImporter\output 

Tip: If you get error MSB3821:
a) Right-click on the ZIP file or cloned file "BurpImportForm.resx".
b) In the general tab, select Unblock and click OK.