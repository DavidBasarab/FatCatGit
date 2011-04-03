# FatCatGit

Cats like to live a simple life.  A fat cat wants an even simpler one.  In order to accomplish this FatCatGit goal is to create a simple Git user interface for Windows that is simple to use.  This will be simple for me at my job.  I will not guarantee this will work for any fat cat.  This is inspired by [GitExtensions](http://groups.google.com/group/gitextensions).

I am using [PivotalTracker](https://www.pivotaltracker.com/projects/265427/overview) to house the user stories for this project.

## Running Unit Tests

In order to get all the unit tests to pass, the line 
		
		const string GitProjectLocation = @"F:\Code\FatCatGit";
		
must be changed to a valid project.  The file is ..\FatCatGit.UnitTests.GitCommands\GitStatusCommandTests.cs