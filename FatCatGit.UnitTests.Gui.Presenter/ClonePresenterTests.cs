using System.Configuration;
using FatCatGit.Gui.Presenter.Exceptions;
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

        private const string TestRepository = @"C:\SomeTestRepository";
        private const string TestRepositoryWithSubFolder = @"C:\SomeTestRepository\SubFolderTest";

        private CloneView SetUpClonePresenterTestWithRepositry(string repositoryToClone, string expectedDestinationFolder)
        {
            var cloneView = Mocks.DynamicMock<CloneView>();

            cloneView.Expect(v => v.RepositoryToClone).Return(repositoryToClone);

            cloneView.DestinationFolder = expectedDestinationFolder;

            Mocks.ReplayAll();

            return cloneView;
        }

        [Test]
        public void DestinationFolderWillNotAddExtraSlashAtEndOfTheFolder()
        {
            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepositoryWithSubFolder, string.Format("{0}\\{1}", DestinationLocation, "SubFolderTest"));

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);
        }

        [Test]
        public void SpecificyDestionationFolderWillUseRepositoryNameAsSubFolder()
        {
            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepository, string.Format("{0}\\{1}", DestinationLocation, "SomeTestRepository"));

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);
        }

        [Test]
        public void SpecificyDestionationFolderWillUseRepositoryNameAsSubFolderWhenRepoUnderParentFolder()
        {
            CloneView cloneView = SetUpClonePresenterTestWithRepositry(TestRepositoryWithSubFolder, string.Format("{0}\\{1}", DestinationLocation, "SubFolderTest"));

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(DestinationLocation);
        }

        [Test]
        [ExpectedException(typeof(InvalidDirectoryException))]
        public void InvalidFolderWillThrowAnInvalidDirectoryException()
        {
            var cloneView = Mocks.DynamicMock<CloneView>();

            Mocks.ReplayAll();

            var presenter = new ClonePresenter(cloneView);

            presenter.SetDestinationFolder(@"C:\REALLY_FAKE_FOLDER_LOCATION");
        }
    }
}