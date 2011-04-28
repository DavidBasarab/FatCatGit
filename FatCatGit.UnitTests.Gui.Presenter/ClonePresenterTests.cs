using System;
using FatCatGit.Common.Interfaces;
using FatCatGit.GitCommands.Interfaces;
using FatCatGit.Gui.Presenter.Exceptions;
using FatCatGit.Gui.Presenter.Presenters;
using FatCatGit.Gui.Presenter.Views;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExtensions;

namespace FatCatGit.UnitTests.Gui.Presenter
{
    [TestFixture]
    [Category("Presentation")]
    public class ClonePresenterTests : BasePresenterTests
    {
        private static string DestinationLocation
        {
            get { return @"C:\DestinationLocation\Fake"; }
        }

        private const string TestRepository = @"C:\SomeTestRepository";
        private const string TestRepositoryWithSubFolder = @"C:\SomeTestRepository\SubFolderTest";

        private CloneView SetUpClonePresenterTestWithRepositry(string repositoryToClone, string expectedDestinationFolder)
        {
            var cloneView = Mocks.DynamicMock<CloneView>();

            cloneView.Expect(v => v.RepositoryToClone).Return(repositoryToClone);

            cloneView.DestinationFolder = expectedDestinationFolder;
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            return cloneView;
        }

        private void VerifyGitRepositoryName(string gitUrl)
        {
            string expectedDestinationFolder = string.Format("{0}\\{1}", DestinationLocation, "FatCatGit");

            CloneView cloneView = SetUpClonePresenterTestWithRepositry(gitUrl, expectedDestinationFolder);

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);

            Assert.That(cloneView.DestinationFolder, Is.EqualTo(expectedDestinationFolder));
        }

        private CloneView SetUpTestForDisplayCloneButton()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.ShowCloneButton());

            view.RepositoryToClone = null;
            LastCall.PropertyBehavior();

            view.DestinationFolder = null;
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();
            return view;
        }

        private CloneView SetUpTestForHideCloneButton()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.ShowCloneButton()).Repeat.Once();

            view.Expect(v => v.HideCloneButton());

            view.RepositoryToClone = null;
            LastCall.PropertyBehavior();

            view.DestinationFolder = null;
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();
            return view;
        }

        [Test]
        public void CloneButtonIsOnlyHiddenOnce()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.ShowCloneButton()).Repeat.Once();
            view.Expect(v => v.HideCloneButton()).Repeat.Once();

            view.RepositoryToClone = null;
            LastCall.PropertyBehavior();

            view.DestinationFolder = null;
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = "some value";
            view.DestinationFolder = "more value";

            var presenter = new ClonePresenter(view);

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.True);

            view.RepositoryToClone = null;
            view.DestinationFolder = null;

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.False);

            view.RepositoryToClone = null;
            view.DestinationFolder = null;

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.False);
        }

        [Test]
        public void CloneWillCloneTheRepositoryToTheDestination()
        {
            var cloneView = Mocks.StrictMock<CloneView>();

            cloneView.SetPropertyAsBehavior(v => v.RepositoryToClone);
            cloneView.SetPropertyAsBehavior(v => v.DestinationFolder);

            var clone = Mocks.StrictMock<Clone>();

            clone.SetPropertyAsBehavior(v => v.RepositoryToClone);
            clone.SetPropertyAsBehavior(v => v.Destination);

            var result = new TestAsyncResults
                             {
                                 AsyncState = new FakeOutput
                                                  {
                                                      Output = "Clone completed successfully"
                                                  }
                             };

            clone.Expect(v => v.Run()).Return(result);

            Mocks.ReplayAll();

            const string repoToClone = @"C:\UnitTestRepo\ToClone";
            const string destinationFolder = @"C:\NewClone\Location";

            cloneView.RepositoryToClone = repoToClone;
            cloneView.DestinationFolder = destinationFolder;

            var presenter = new ClonePresenter(cloneView)
                                {
                                    Clone = clone
                                };

            bool eventTriggered = false;
            string output = string.Empty;

            Action<Output> cloneComplete = o =>
                                               {
                                                   eventTriggered = true;
                                                   output = o.Output;
                                               };

            presenter.CloneComplete += cloneComplete;

            presenter.PerformClone();

            Assert.That(clone.RepositoryToClone, Is.EqualTo(repoToClone));
            Assert.That(clone.Destination, Is.EqualTo(destinationFolder));
            Assert.That(eventTriggered);
            Assert.That(output, Is.EqualTo("Clone completed successfully"));
        }

        [Test]
        [ExpectedException(typeof (CannotCloneException))]
        public void CloneWillThrowAnExceptionIfDestinationToCloneIsNotPopulated()
        {
            var cloneView = Mocks.StrictMock<CloneView>();

            cloneView.SetPropertyAsBehavior(v => v.RepositoryToClone);
            cloneView.SetPropertyAsBehavior(v => v.DestinationFolder);

            var clone = Mocks.StrictMock<Clone>();

            clone.SetPropertyAsBehavior(v => v.RepositoryToClone);
            clone.SetPropertyAsBehavior(v => v.Destination);

            clone.Expect(v => v.Run()).Repeat.Never();

            Mocks.ReplayAll();

            const string repoToClone = @"C:\UnitTestRepo\ToClone";

            cloneView.RepositoryToClone = repoToClone;
            cloneView.DestinationFolder = null;

            var presenter = new ClonePresenter(cloneView)
                                {
                                    Clone = clone
                                };

            presenter.PerformClone();
        }

        [Test]
        [ExpectedException(typeof (CannotCloneException))]
        public void CloneWillThrowAnExceptionIfRepoToCloneIsNotPopulated()
        {
            var cloneView = Mocks.StrictMock<CloneView>();

            cloneView.SetPropertyAsBehavior(v => v.RepositoryToClone);
            cloneView.SetPropertyAsBehavior(v => v.DestinationFolder);

            var clone = Mocks.StrictMock<Clone>();

            clone.SetPropertyAsBehavior(v => v.RepositoryToClone);
            clone.SetPropertyAsBehavior(v => v.Destination);

            clone.Expect(v => v.Run()).Repeat.Never();

            Mocks.ReplayAll();

            const string destinationFolder = @"C:\NewClone\Location";

            cloneView.RepositoryToClone = null;
            cloneView.DestinationFolder = destinationFolder;

            var presenter = new ClonePresenter(cloneView)
                                {
                                    Clone = clone
                                };

            presenter.PerformClone();
        }

        [Test]
        public void DestinationFolderWillNotAddExtraSlashAtEndOfTheFolder()
        {
            string expectedDestinationFolder = string.Format("{0}\\{1}", DestinationLocation, "SubFolderTest");

            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepositoryWithSubFolder, expectedDestinationFolder);

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);

            Assert.That(cloneView.DestinationFolder, Is.EqualTo(expectedDestinationFolder));
        }

        [Test]
        public void HideDestionationFolderWillNotBeCalledTwiceWhenNoChange()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.HideDestinationFolder()).Repeat.Once();
            view.Expect(v => v.DisplayDestinationFolder()).Repeat.Once();

            view.RepositoryToClone = "does_not_matter";
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = "some data to get test set up";

            var presenter = new ClonePresenter(view);

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.True);

            view.RepositoryToClone = null;

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.False);

            view.RepositoryToClone = null;

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.False);
        }

        [Test]
        public void IfDestinationFolderBlankItIsNotSet()
        {
            var cloneView = Mocks.DynamicMock<CloneView>();

            cloneView.Expect(v => v.RepositoryToClone).IgnoreArguments().Repeat.Never();

            cloneView.DestinationFolder = null;
            LastCall.PropertyBehavior();
            LastCall.IgnoreArguments();
            LastCall.Repeat.Never();

            Mocks.ReplayAll();

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(null);

            Assert.That(cloneView.DestinationFolder, Is.Null);
        }

        [Test]
        public void IfDirectoryEndsInRepoNameDoNotAdd()
        {
            string expectedDestinationFolder = string.Format("{0}\\{1}\\", DestinationLocation, "SomeTestRepository");

            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepository, expectedDestinationFolder);

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(expectedDestinationFolder);

            Assert.That(cloneView.DestinationFolder, Is.EqualTo(expectedDestinationFolder));
        }

        [Test]
        public void SecondTimeRepositoryToCloneChangedAndValidTextNoChangeWithDestionationFolderOccuers()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.DisplayDestinationFolder()).Repeat.Once();

            view.RepositoryToClone = "ve";
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = "somedata";

            var presenter = new ClonePresenter(view);

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.True);

            view.RepositoryToClone = "new data";

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.True);
        }

        [Test]
        public void SpecificyDestionationFolderWillUseRepositoryNameAsSubFolder()
        {
            string expectedDestinationFolder = string.Format("{0}\\{1}", DestinationLocation, "SomeTestRepository");

            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepository, expectedDestinationFolder);

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);

            Assert.That(cloneView.DestinationFolder, Is.EqualTo(expectedDestinationFolder));
        }

        [Test]
        public void SpecificyDestionationFolderWillUseRepositoryNameAsSubFolderWhenRepoUnderParentFolder()
        {
            string expectedDestinationFolder = string.Format("{0}\\{1}", DestinationLocation, "SubFolderTest");

            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepositoryWithSubFolder, expectedDestinationFolder);

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);

            Assert.That(cloneView.DestinationFolder, Is.EqualTo(expectedDestinationFolder));
        }

        [Test]
        public void WhenCloneButtonIsVisibleItNotDisplayedAgain()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.ShowCloneButton()).Repeat.Once();

            view.SetPropertyAsBehavior(v => v.RepositoryToClone);
            view.SetPropertyAsBehavior(v => v.DestinationFolder);

            Mocks.ReplayAll();

            view.RepositoryToClone = "some value";
            view.DestinationFolder = "more value";

            var presenter = new ClonePresenter(view);

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.True);

            view.RepositoryToClone = "changes value";
            view.DestinationFolder = "other changes value";

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.True);
        }

        [Test]
        public void WhenDestinationIsNotPopulatedTheCloneButtonIsNotShown()
        {
            CloneView view = SetUpTestForHideCloneButton();

            view.RepositoryToClone = "some value";
            view.DestinationFolder = null;

            var presenter = new ClonePresenter(view);

            view.RepositoryToClone = "some value";
            view.DestinationFolder = "other value";

            presenter.DestionFolderTextChanged();

            view.RepositoryToClone = null;
            view.DestinationFolder = null;

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.False);
        }

        [Test]
        public void WhenRepositoryAndDestinaitonArePopulatedCloneButtonIsShown()
        {
            CloneView view = SetUpTestForDisplayCloneButton();

            view.RepositoryToClone = "some value";
            view.DestinationFolder = "some value, can be same of different";

            var presenter = new ClonePresenter(view);

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.True);
        }

        [Test]
        public void WhenRepositoryIsNotPopulatedTheCloneButtonIsNotShown()
        {
            CloneView view = SetUpTestForHideCloneButton();

            var presenter = new ClonePresenter(view);

            view.RepositoryToClone = "value";
            view.DestinationFolder = "some value";

            presenter.DestionFolderTextChanged();

            view.RepositoryToClone = null;
            view.DestinationFolder = "some value";

            presenter.DestionFolderTextChanged();

            Assert.That(presenter.IsCloneButtonShown, Is.False);
        }

        [Test]
        public void WhenRespitoryToChangeIsNullAndAChangeEventWillNotDisplayDestination()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.HideDestinationFolder()).Repeat.Never();

            view.RepositoryToClone = "ve";
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = null;

            var presenter = new ClonePresenter(view);

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.False);
        }

        [Test]
        public void WhenTextIsAddedToRepositoryDisplayDestionationIsCalled()
        {
            var view = Mocks.DynamicMock<CloneView>();

            view.Expect(v => v.DisplayDestinationFolder());

            view.RepositoryToClone = "ve";
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = "somedata";

            var presenter = new ClonePresenter(view);

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.True);
        }

        [Test]
        public void WhenTextIsRemovedToRepositoryHideDisplayIsCalled()
        {
            var view = Mocks.DynamicMock<CloneView>();

            view.Expect(v => v.HideDestinationFolder());

            view.RepositoryToClone = "ve";
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = "some early data";

            var presenter = new ClonePresenter(view);

            presenter.RepositoryToCloneChanged();

            view.RepositoryToClone = null;

            presenter.RepositoryToCloneChanged();

            Assert.That(presenter.DestinationFolderDisplayed, Is.False);
        }

        [Test]
        public void WillDetermineFtpsRepoName()
        {
            const string gitUrl = @"ftp[s]://host.xz[:port]/path/to/FatCatGit.git/";

            VerifyGitRepositoryName(gitUrl);
        }

        [Test]
        public void WillDetermineHttpsRepoName()
        {
            const string gitUrl = @"http[s]://host.xz[:port]/path/to/FatCatGit.git/";

            VerifyGitRepositoryName(gitUrl);
        }

        [Test]
        public void WillDetermineRepoFromGitHubUrl()
        {
            const string gitUrl = @"git@github.com:DavidBasarab/FatCatGit.git";

            VerifyGitRepositoryName(gitUrl);
        }

        [Test]
        public void WillDetermineRepoNameFromRemoteRepoistory()
        {
            const string gitUrl = @"git@127.0.0.1:FatCatGit.git";

            VerifyGitRepositoryName(gitUrl);
        }

        [Test]
        public void WillDetermineSshRepoName()
        {
            const string gitUrl = @"ssh://[user@]host.xz[:port]/path/to/FatCatGit.git/";

            VerifyGitRepositoryName(gitUrl);
        }
    }
}