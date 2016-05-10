#LionsSoft.ProcessAsService

This is a little utility i made to let me start spamd for windows as a service.
But you can use it for any process by configuring it in the configuration file.

##Configuration
Put this configuration in %programdata%\LionsSoft\ProcessAsService.config.
Customize as your need.

```
<?xml version="1.0" encoding="utf-8"?>
<Conf xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
   <FileToService>c:\somedir\LionsSoft.ExampleProcess.exe</FileToService>
   <StartDirectory>c:\somedir</StartDirectory>
   <Arguments>some optional arguments</Arguments>
</Conf>
```

##Try and start

Start LionsSoft.ProcessAsService.exe, make your test and finally start LionsSoft.ProcessAsService.exe with "install" argument.
See TopShelf documentation for more command line arguments.


##Thanks to
* http://topshelf-project.com/
* http://nlog-project.org/
* http://www.west-wind.com/Westwind.ApplicationConfiguration/