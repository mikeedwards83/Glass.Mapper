
[![Dev CI](https://github.com/mikeedwards83/Glass.Mapper/workflows/Dev%20CI/badge.svg?branch=develop)](https://github.com/mikeedwards83/Glass.Mapper/actions?query=workflow%3A%22Dev+CI%22)  ![NuGet](https://img.shields.io/nuget/v/Glass.Mapper.Sc.100)

Glass.Mapper
============  
  
![alt text](http://glass.lu/-/media/Images/Common/Horizon-Bordered-BlazeOrange-250b3fb.png?h=250&w=250&la=en&hash=EE06CAB08F72FA2AFE420EBB41BC60015BC139A4 "Glass logo")
  
Glass.Mapper has been the redevelopment of the very popular Glass.Sitecore.Mapper project. This project aims to create a more robust and more flexible solution that not only works with many different CMS's.

A key feature of the implementation is the introduction of pipelines for most of the tasks carried out by Glass.Mapper allowing you to swap in your preferred solution, for example if you don't like the standard way to create a concrete object then you can implement you own and swap it in.

## Setup Instructions
Glass can be installed as a NuGet package. Simply install the package that is relevant to your platform version e.g. Glass.Mapper.Sc.100. 

To register Glass.Mapper.Sc in your application during service configuration call AddGlassMapper:

    using  Glass.Mapper.Sc;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGlassMapper();
    }

If you are upgrading from a legacy Glass.Mapper.Sc setup you can continue to use the files in the App_Start folder.
