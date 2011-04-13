using System.Configuration;
using FatCatGit.Gui.Presenter.Presenters;
using FatCatGit.Gui.Presenter.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatGit.UnitTests.Gui.Presenter
{
    [TestFixture]
    [Category("Presentation")]
    public class ClonePresenterTests : BasePresenterTests
    {
        private static string DestinationLocation
        {
            get { return ConfigurationManager.AppSettings["DestinationLocation"]; }
        }

        private const string TestRepository = @"F:\SomeTestRepository";
        private const string TestRepositoryWithSubFolder = @"F:\SomeTestRepository\SubFolderTest";

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
        public void SecondTImeRepositoryToCloneChangedAndValidTextNoChangeWithDestionationFolderOccuers()
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
        public void HideDestionationFolderWillNotBeCalledTwiceWhenNoChange()
        {
            var view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.HideDestinationFolder()).Repeat.Once();
            view.Expect(v => v.DisplayDestinationFolder()).Repeat.Once();

            view.RepositoryToClone = "ve";
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
        public void WhenRepositoryAndDestinaitonArePopulatedCloneButtonIsShown()
        {
            CloneView view = Mocks.StrictMock<CloneView>();

            view.Expect(v => v.ShowCloneButton());

            view.RepositoryToClone = null;
            LastCall.PropertyBehavior();

            view.DestinationFolder = null;
            LastCall.PropertyBehavior();

            Mocks.ReplayAll();

            view.RepositoryToClone = "some value";

            ClonePresenter presenter = new ClonePresenter(view);

            presenter.SetDestinationFolder("another value");

            Assert.That(presenter.IsCloneButtonShown, Is.True);
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