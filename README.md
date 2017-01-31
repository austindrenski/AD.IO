# AD.IO

C# extension library for input/output operations and data management.

## Install from NuGet:

```Powershell 
PM> Install-Package AD.IO
```

## Documentation

### [AD.IO.Compression](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Compression)     
|                                                                                                |                                                                 |
|:-----------------------------------------------------------------------------------------------|:----------------------------------------------------------------|
|[GetZipFile](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Compression#GetZipFile)          |GetZipFile(this UrlPath, ZipFilePath, bool)                      |
|[TryGetZipFile](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Compression#TryGetZipFile)    |TryGetZipFile(this UrlPath, ZipFilePath, bool, string)           |
|[ExtractZipFile](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Compression#ExtractZipFile)  |ExtractZipFile(this ZipFilePath, FilePath, bool)                 |
|                                                                                                |ExtractZipFile(this ZipFilePath, FilePath, bool, string)         |
|                                                                                                |ExtractZipFile(this ZipFilePath, ZipFilePath, bool)              |
|                                                                                                |ExtractZipFile(this ZipFilePath, ZipFilePath, bool, string)      |
|                                                                                                |ExtractZipFile(this ZipFilePath, DelimitedFilePath, bool)        |
|                                                                                                |ExtractZipFile(this ZipFilePath, DelimitedFilePath, bool, string)|
|                                                                                                |ExtractZipFile(this ZipFilePath, XmlFilePath, bool)              |
|                                                                                                |ExtractZipFile(this ZipFilePath, XmlFilePath, bool, string)      |
|[ExtractZipFiles](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Compression#ExtractZipFiles)|ExtractZipFiles(this ZipFilePath, DirectoryPath, bool)           |
|                                                                                                |ExtractZipFiles(this ZipFilePath, DirectoryPath, bool, string)   |

|[AD.IO.Paths](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths)                         |
|:----------------------------------------------------------------------------------------------|
|[DelimitedFilePath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#DelimitedFilePath) |
|[DirectoryPath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#DirectoryPath)         |
|[DocxFilePath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#DocxFilePath)           |
|[FilePath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#FilePath)                   |
|[HtmlFilePath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#HtmlFilePath)           |
|[IPath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#IPath)                         |
|[UrlPath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#UrlPath)                     |
|[XmlFilePath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#XmlFilePath)             |
|[ZipFilePath](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Paths#ZipFilePath)             |


|[AD.IO.Read](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Read)                           |
|:----------------------------------------------------------------------------------------------|
|[ReadAsXml](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Read#ReadAsXml)                  |

|[AD.IO.Text](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Text)                           |
|:----------------------------------------------------------------------------------------------|
|[SplitDelimited](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Text#SplitDelimited)        |
|[ToCapitalized](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Text#ToCapitalized)          |
|[ToDelimited](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Text#ToDelimited)              |

### [AD.IO.Write](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Write)

* [WriteDelimited](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Write#WriteDelimited)
* [WriteHtml](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Write#WriteHtml)
* [WriteInto](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Write#WriteInto)
* [WriteXml](https://github.com/austindrenski/AD.IO/wiki/AD.IO.Write#WriteXml)
