# FatCatGit

Cats like to live a simple life.  A fat cat wants an even simpler one.  In order to accomplish this FatCatGit goal is to create a simple Git user interface for Windows that is simple to use.  This will be simple for me at my job.  I will not guarantee this will work for any fat cat.  This is inspired by [GitExtensions](http://groups.google.com/group/gitextensions).

I am using [PivotalTracker](https://www.pivotaltracker.com/projects/265427/overview) to house the user stories for this project.

## Running Unit Tests

In order to get all the unit tests to pass, change the configuraiton in FatCat.Git.UnitTets.GitCommands .config file to the correct settings.  
		
		<add key="GitTestProjectLocation" value="C:\Test\Repo1"/>
        <add key="GitTestCloneLocation" value="C:\Test\UnitTestRepo"/>
        <add key="GitInstallLocation" value="C:\Program Files (x86)\Git\bin\git.exe"/>
		
GitTestProjectLocation is a test git project that will be used in the unit tests.  