# AppScan Standard - Burp Traffic Importer extension

This library is an extension for HCL AppScan Standard.

It allows the user to import a Burp file containing a list of requests into AppScan. AppScan will explore these requests as if they were explored manually.

## Installation

To install, the extension must be imported into AppScan Standard using the AppScan extension manager.

1. In AppScan Standard, go to Tools > Extensions > Extension Manager
1. Import the extension BurpTrafficImporter.zip using Add Extensions From > This Computer 
1. Restart AppScan Standard for the change to take effect.
1. Open Extension Manager.
1. Click "Trust" to trust the extension.

## Usage

Once installed, you can access the extension using Tools > Extensions > Import Burp Traffic

Configuration:

- **Browse**:The file you want to import. Must be a Burp file with valid requests.
- **Use first request as Starting Point URL**: If the file includes relative paths, add here the base URL for these paths.
- **Select domains to add to Additional Domains**: 

After configuring the extension, click *Import* to convert the list of requests into a temporary EXD (EXplore Data) file and import it into AppScan. AppScan will then explore these requests automatically. You can then decide whether to continue with a Test stage or more manual exploring.

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
	c) Click Add > Reference  
	d) In the Browse section, select Browse and add the following 4 DLLs from the AppScan folder:     
	- AppScanSDK.dll  
	- EngineAPI.dll  
	- HttpProxy.dll
	- CommonEngineProvider.dll
	- UserControls.dll
	- ScanTypes.dll
	- LocalizationUtils.dll
	- InfraTypes.dll


The file `BurpTrafficImporterExt.zip` is automatically generated in the sub folder: \appscan-standard-BurpTrafficImporter\output 