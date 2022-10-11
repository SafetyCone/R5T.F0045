using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using R5T.F0042;
using R5T.T0132;


namespace R5T.F0045
{
	[FunctionalityMarker]
	public partial interface IRepositoryOperations : IFunctionalityMarker
	{
		// <summary>
		/// Library includes two solutions: the library solution and a construction solution. Two projects are created as well: a library project, and a constuction console project.
		/// </summary>
		public async Task<RepositoryLocationsPair> CreateNew_LibraryRepository(
			string owner,
			string endeavorName,
			string description,
			bool isPrivate,
			ILogger logger)
		{
			/// Run.
			logger.LogInformation($"Creating library repository '{endeavorName}'...");

			/// Library.
			var unadjustedLibraryName = Instances.LibraryNameOperator.GetUnadjustedLibraryName(endeavorName);

			var libraryName = Instances.LibraryNameOperator.AdjustLibraryName_ForPrivacy(
				unadjustedLibraryName,
				isPrivate,
				logger);

			var libraryDescription = Instances.LibraryDescriptionOperator.GetLibraryDescription(description);

			/// Repository.
			var repositoryName = Instances.RepositoryNameOperator.GetRepositoryName_FromLibraryName(libraryName);
			var ownedRepositoryName = Instances.RepositoryNameOperator.GetOwnedRepositoryName(
				owner,
				repositoryName);

			logger.LogInformation($"Repository name: '{ownedRepositoryName}'.");

			// Repository description is just the library description.
			var repositoryDescription = Instances.RepositoryDescriptionOperator.GetRepositoryDescription_FromLibraryDescription(libraryDescription);

			var repositorySpecification = Instances.RepositoryOperator.Get_RepositorySpecification(
				owner,
				repositoryName,
				repositoryDescription,
				isPrivate);

			/// Safety check: stop if repository already exists.
			await Instances.RepositoryOperator.SafetyCheck_VerifyRepositoryDoesNotAlreadyExist(
				repositoryName,
				owner,
				logger);

			/// Create - Repository.
			// As of now, we can assume the repository does not exist.
			var repositoryLocations = await Instances.RepositoryOperator.CreateNew_NonIdempotent(
				repositorySpecification,
				logger);

			// Setup repository.
			var repositorySourceDirectoryPath = Instances.RepositoryOperator.SetupRepository(
				repositoryLocations.LocalDirectoryPath,
				logger);

			/// Create - Solution.
			// Now create the solution and project.
			var unadjustedSolutionName = Instances.SolutionNameOperator.GetUnadjustedSolutionName_FromUnadjustedLibraryName(unadjustedLibraryName);
			var solutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedSolutionName, isPrivate);

			var solutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
				repositorySourceDirectoryPath,
				solutionName,
				logger);

			// Create - project.
			// Project name is just the unadjusted repository name. No adjustments needed.
			var projectName = unadjustedLibraryName;

			// Script project description is just the library description.
			var projectDescription = libraryDescription;

			// Namespace name is just the program name.
			var projectNamespaceName = Instances.ProjectNamespacesOperator.GetDefaultNamespaceName_FromProjectName(projectName);

			var projectFilePath = Instances.ProjectOperator.Create_New(
				solutionFilePath,
				projectName,
				F0020.ProjectType.Library,
				logger);

			// Set package properties.
			Instances.ProjectOperations.AddPackageProperties(projectFilePath);

			// Setup project.
			Instances.ProjectOperator.SetupProject_Library(
				projectFilePath,
				projectDescription,
				projectName,
				projectNamespaceName);

			// Add project to solution.
			Instances.SolutionFileOperator.AddProject(
				solutionFilePath,
				projectFilePath);

			// Construction solution.
			var unadjustedConstructionSolutionName = Instances.SolutionNameOperator.GetConstructionSolutionName(unadjustedSolutionName);
			var constructionSolutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedConstructionSolutionName, isPrivate);

			var constructionSolutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
				repositorySourceDirectoryPath,
				constructionSolutionName,
				logger);

			// Construction project.
			// Project name is just the unadjusted repository name. No adjustments needed.
			var constructionProjectName = Instances.ProjectNameOperator.GetConstructionProjectName(projectName);

			// Script project description is just the library description.
			var constructionProjectDescription = $"Construction console project for the {projectName} library.";

			// Namespace name is just the program name.
			var constructionProjectNamespaceName = Instances.ProjectNamespacesOperator.GetDefaultNamespaceName_FromProjectName(constructionProjectName);

			var constructionProjectFilePath = Instances.ProjectOperator.Create_New(
				solutionFilePath,
				constructionProjectName,
				F0020.ProjectType.Console,
				logger);

			// Set package properties.
			Instances.ProjectOperations.AddPackageProperties(constructionProjectFilePath);

			// Setup project.
			Instances.ProjectOperator.SetupProject_Console(
				constructionProjectFilePath,
				constructionProjectDescription,
				constructionProjectName,
				constructionProjectNamespaceName);

			// Add project reference to library project to the construction project.
			Instances.ProjectFileOperator.AddProjectReference_Synchronous(
				constructionProjectFilePath,
				projectFilePath);

			// Add projects to construction solution.
			// Add construction project first so it will be the default startup project.
			Instances.SolutionFileOperator.AddProject(
				constructionSolutionFilePath,
				constructionProjectFilePath);

			Instances.SolutionFileOperator.AddProject(
				constructionSolutionFilePath,
				projectFilePath);


			// Perform initial commit.
			Instances.RepositoryOperator.PerformInitialCommit(
				repositoryLocations.LocalDirectoryPath,
				logger);

			return repositoryLocations;
		}
	}
}