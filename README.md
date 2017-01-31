# AD.IO
C# extension library for input/output operations and data management.
## Install from NuGet:
```
PM> Install-Package AD.IO
```
## AD.IO.Compression
### GetZipFile
```C#
UrlPath url = "www.google.com/example.zip";
ZipFilePath zip = ZipFilePath.Create("c:/users/example/desktop/example.zip");

url.GetZipFile(zip, overwrite: true);
```
### TryGetZipFile
```C#
UrlPath url = "www.google.com/example.zip";
ZipFilePath zip = ZipFilePath.Create("c:/users/example/desktop/example.zip");

url.TryGetZipFile(zip, overwrite: true, "Download successful");

// Console: Download successful
```
### ExtractZipFile
```C#
ZipFilePath zip = "c:/users/example/desktop/example.zip";
FilePath outFilePath = FilePath.Create("c:/users/example/desktop/example.txt");

zip.ExtractZipFile(outFilePath, overwrite: true, message: "'zip' extracted to 'outFilePath'");

// Console: 'zip' extracted to 'outFilePath'
```
### ExtractZipFile
```C#
ZipFilePath zip = "c:/users/example/desktop/example.zip";
ZipFilePath outZipFilePath = ZipFilePath.Create("c:/users/example/desktop/example.zip");

zip.ExtractZipFile(outZipFilePath, overwrite: true, message: "'zip' extracted to 'outZipFilePath'");

// Console: 'zip' extracted to 'outZipFilePath'
```
### ExtractZipFile
```C#
ZipFilePath zip = "c:/users/example/desktop/example.zip";
DelimitedFilePath outDelimitedFile = DelimitedFilePath.Create("c:/users/example/desktop/example.csv");

zip.ExtractZipFile(outDelimitedFile, overwrite: true, message: "'zip' extracted to 'outDelimitedFilePath'");

// Console: 'zip' extracted to 'outDelimitedFilePath'
```
### ExtractZipFile
```C#
ZipFilePath zip = "c:/users/example/desktop/example.zip";
XmlFilePath outXmlFilePath = XmlFilePath.Create("c:/users/example/desktop/example.xml");

zip.ExtractZipFile(outXmlFilePath, overwrite: true, message: "'zip' extracted to 'outXmlFilePath'");

// Console: 'zip' extracted to 'outXmlFilePath'
```
### ExtractZipFiles
```C#
ZipFilePath zip = "c:/users/example/desktop/example.zip";
DirectoryPath directoryPath = "c:/users/example/desktop";

zip.ExtractZipFiles(directorPath, overwrite: true, message: "Contents of 'zip' extracted to 'outDirectoryPath'");

// Console: Contents of 'zip' extracted to 'outDirectoryPath'
```
