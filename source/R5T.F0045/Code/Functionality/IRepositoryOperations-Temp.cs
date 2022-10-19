using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using R5T.F0042;
using R5T.T0132;


namespace R5T.F0045
{
	public partial interface IRepositoryOperations : IFunctionalityMarker
	{
		public async Task<RepositoryLocationsPair> CreateNew_WebApplicationRepository(
			string owner,
			string endeavorName,
			string description,
			bool isPrivate,
			ILogger logger)
		{
			/// Run.
			logger.LogInformation($"Creating web application repository '{endeavorName}'...");

			/// Library.
			var libraryDescriptors = F0043.LibraryOperator.Instance.GetDescriptors(
				endeavorName,
				description,
				isPrivate,
				logger);

			/// Repository.
			var repositoryDescriptors = F0046.RepositoryOperator.Instance.GetDescriptors(
				libraryDescriptors.Name,
				libraryDescriptors.Description,
				owner);

			logger.LogInformation($"Repository name: '{repositoryDescriptors.OwnedName}'.");

			var repositorySpecification = Instances.RepositoryOperator.Get_RepositorySpecification(
				owner,
				repositoryDescriptors.Name,
				repositoryDescriptors.Description,
				isPrivate);

			/// Safety check: stop if repository already exists.
			await Instances.RepositoryOperator.SafetyCheck_VerifyRepositoryDoesNotAlreadyExist(
				repositoryDescriptors.Name,
				owner,
				logger);

			// As of now, we can assume the repository does not exist.

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
			var unadjustedSolutionName = Instances.SolutionNameOperator.GetUnadjustedSolutionName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);
			var solutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedSolutionName, isPrivate);

			var solutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
				repositorySourceDirectoryPath,
				solutionName,
				logger);

			/// Create - project.
			var projectName = Instances.ProjectNameOperator.GetProjectName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);

			var projectDescription = Instances.ProjectOperator.Get_ProjectDescription_FromLibraryDescription(libraryDescriptors.Description);

			var projectNamespaceName = Instances.ProjectNamespacesOperator.GetDefaultNamespaceName_FromProjectName(projectName);

			var projectFilePath = Instances.ProjectOperator.Create_New(
				solutionFilePath,
				projectName,
				F0020.ProjectType.Console,
				logger);

			// Set package properties.
			Instances.ProjectOperations.AddPackageProperties(projectFilePath);

			// Setup project.
			Instances.ProjectOperator.SetupProject_WebApplication(
				projectFilePath,
				projectDescription,
				projectName,
				projectNamespaceName);

			Instances.ProjectOperator.PostSetupProject_WebApplication(
				projectFilePath,
				projectDescription,
				projectName,
				projectNamespaceName);

			// Add project to solution.
			Instances.SolutionFileOperator.AddProject(
				solutionFilePath,
				projectFilePath);

			// Add all dependencies to solution.
			await Instances.SolutionOperations.AddMissingDependencies(solutionFilePath);

			// Perform initial commit.
			Instances.RepositoryOperator.PerformInitialCommit(
				repositoryLocations.LocalDirectoryPath,
				logger);

			return repositoryLocations;
		}
	}
}