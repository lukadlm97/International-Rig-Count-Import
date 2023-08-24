### Project Assignment Details
In the following text, you will find detailed explanations of my solution for the Enverus project assignment topic.

## Solution Structure
This solution comprises 5 projects:
- Homework.Enverus.InternationalRigCountImport.Core: Contains the completed logic required for creating assets used in this import/export project. By utilizing the extension "ConfigureApplicationServices," you can fully prepare your project to make use of these project assets.
- Homework.Enverus.InternationalRigCountImport.Test: This project covers the Core library with tests using xUnit and Moq frameworks.
- Homework.Enverus.Shared.Logging: A library used as a high-performance logging provider. (Refer to the provided references for more details.)
- Homework.Enverus.InternationalRigCountImport.Console: A project that consumes all features supplied by the Core library, allowing you to host background services performing import/export actions.
- Homework.Enverus.InternationalRigCountImport.CAF: This project offers a convenient ConsoleAppFramework (CAF) that facilitates consumer interaction for import explanations, commands, and argument calling.

## Detailed Explanation
The main task involved fetching data from an Excel file and exporting a portion of its content to a CSV file. Initially, it is necessary to determine the URL of the Excel file on the site. The index page provides a link to that file, with the current file titled "Worldwide Rig Count Jul 2023.xlsx." This URL is obtained using the AngleSharp library. In case the document's title changes, the file URL remains constant over time, making the current URL usable. Configuration for these aspects is present in the app settings of both applications:

```json
"ExternalDataSources": {
	"BaseUrl": "https://bakerhughesrigcount.gcs-web.com",
	"RetrieveFileUrl": "intl-rig-count?c=79687&p=irol-rigcountsintl",
	"StaticFileUrl": "static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8",
	"RetrieveTagFile": "Worldwide Rig Count Jul 2023.xlsx",
	"UserAgent": "PostmanRuntime/7.32.3"
}
```
Next, data for export needs to be located. Currently, it is situated within Excel range 7:740 for rows and 2:11 for columns. Since we're interested in extracting data from only the last few years, and each year comprises 15 rows in Excel, this section is configurable through app settings or can be specified using CAF parameters.
```json
"DataSourceSettings": {
  "ExcelWorkbookSettings": {
    "Worksheet": "Worldwide_Rigcount",
    "StartRow": 7,
    "StartColumn": 2,
    "EndRow": 740,
    "EndColumn": 11,
    "RowsPerYear": 15,
    "Years": 2
  }
}
```
With all the necessary assets in place, creating the export becomes straightforward. The desired CSV file name and delimiter need to be chosen.
```
"ExportDestinationSettings": {
  "CsvSettings": {
    "FileName": "output.csv",
    "Delimiter": ","
  }
}
```
For advanced handling, such as maintaining a history of downloaded Excel files or specifying storage locations for import and export files, the following configuration is available:
```
"AdvancedSettings": {
  "Enabled": false,
  "ArchiveOldSamples": false,
  "OriginalExcelLocation": "TODO",
  "CsvExportLocation": "TODO"
}
```
If "Enabled" is set to true, import/export will use "OriginalExcelLocation" and "CsvExportLocation" for file storage. When "ArchiveOldSamples" is enabled, a folder structure will be created at the Excel location in the format "OriginalExcelRoot -> DateDir -> TimeDir," where DateDir and TimeDir indicate UTC creation times. In the CsvExportLocation, the requested output.csv file will be generated with the specified delimiter.
## Demo

## CAF application
### Start screen:
<img width="956" alt="Import-Start-From-ConsoleAppFramework" src="https://github.com/lukadlm97/International-Rig-Count-Import/assets/36825550/1b6c9b4a-4d03-429f-acc4-d0b10621257b">

### Files created through import could be found at working directory:
<img width="521" alt="result-ConsoleAppFramework" src="https://github.com/lukadlm97/International-Rig-Count-Import/assets/36825550/6f8718f9-7738-46ef-baaf-52c0f7a2f6e6">

### Resulting CSV looks like:
<img width="956" alt="Csv-Output" src="https://github.com/lukadlm97/International-Rig-Count-Import/assets/36825550/34e119b4-15d4-4cef-adfd-a4c3f4cd151b">

## Simple hosted app
### Start screen:
<img width="925" alt="Import-Start-HostedService" src="https://github.com/lukadlm97/International-Rig-Count-Import/assets/36825550/c2b676e4-fa19-4535-b7c1-a4f219ac4723">

### Files created through import could be found at working directory:
<img width="521" alt="result-ConsoleAppFramework" src="https://github.com/lukadlm97/International-Rig-Count-Import/assets/36825550/25de608d-fb27-406c-b837-cd8a5256223e">

### Resulting CSV looks like:
<img width="957" alt="Csv-output-HostedService" src="https://github.com/lukadlm97/International-Rig-Count-Import/assets/36825550/c0c9c04d-e53f-48fc-b6b5-af9e79de36f8">

### References
- **Resources:**
  - Logging:
    1. [High-Performance Logging in .NET Core](https://www.stevejgordon.co.uk/high-performance-logging-in-net-core)
    2. [High-Performance Logging Extension](https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging)
  - HttpClientFactory:
    1. [Implement Resilient HTTP Requests with HttpClientFactory](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)
    2. [HttpClientFactory Extension](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)
  - AngleSharp:
    1. [AngleSharp GitHub Repository](https://github.com/AngleSharp/AngleSharp)
  - ClosedXML:
    1. [ClosedXML GitHub Repository](https://github.com/ClosedXML/ClosedXML)
    2. ClosedXML allows Excel file creation without the Excel application.
  - NLog:
    1. [NLog Official Website](https://nlog-project.org/)
  - ConsoleAppFramework:
    [ConsoleAppFramework GitHub Repository](https://github.com/Cysharp/ConsoleAppFramework)

### Note
The UserAgent "PostmanRuntime/7.32.3" is used for retrieving content from the base URL, as it has consistently provided stable results for me.



