
[![Dev CI](https://github.com/mikeedwards83/Glass.Mapper/workflows/Dev%20CI/badge.svg?branch=develop)](https://github.com/mikeedwards83/Glass.Mapper/actions?query=workflow%3A%22Dev+CI%22)  ![NuGet](https://img.shields.io/nuget/v/Glass.Mapper.Sc.100)

Glass.Mapper
============  
  
![alt text](http://glass.lu/-/media/Images/Common/Horizon-Bordered-BlazeOrange-250b3fb.png?h=250&w=250&la=en&hash=EE06CAB08F72FA2AFE420EBB41BC60015BC139A4 "Glass logo")
  
Glass.Mapper has been the redevelopment of the very popular Glass.Sitecore.Mapper project. This project aims to create a more robust and more flexible solution that not only works with many different CMS's.

A key feature of the implementation is the introduction of pipelines for most of the tasks carried out by Glass.Mapper allowing you to swap in your preferred solution, for example if you don't like the standard way to create a concrete object then you can implement you own and swap it in.

## Setup Instructions
Glass can be installed as a NuGet package. Simply install the package that is relevant to your platform version e.g. Glass.Mapper.Sc.100. 

If you are using PackageReference format, following installation you will need to include the files from the 'App_Start' and 'App_Config' folders found at _'\obj\\{Build-Configuration}\NuGet\\{Guid}\Glass.Mapper.Sc.xx\\{Version}'_ in your solution if they are not already present. You will also be required to copy the [Glass.Mapper.Sc.config](https://github.com/mikeedwards83/Glass.Mapper/blob/master/Source/Glass.Mapper.Sc/App_Config/Include/Glass/Glass.Mapper.Sc.config) file to your solution.

You can install the 'core' package if you do not wish to overwrite the App_Config and App_Start folders. e.g. Glass.Mapper.Sc.100.Core 
