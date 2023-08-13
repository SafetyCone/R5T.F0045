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
			var repositorySetup = Instances.RepositoryOperator.SetupRepository(
				repositoryLocations.LocalDirectoryPath,
				logger);

			/// Create - Solution.
			var unadjustedSolutionName = Instances.SolutionNameOperator.GetUnadjustedSolutionName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);
			var solutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedSolutionName, isPrivate);

			var solutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
                repositorySetup.SourceDirectoryPath,
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

		public async Task<RepositoryLocationsPair> CreateNew_RepositoryOnly(
			string owner,
			string endeavorName,
			string description,
			bool isPrivate,
			ILogger logger)
		{
			/// Run.
			logger.LogInformation($"Creating console repository '{endeavorName}'...");

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

			// NO repository setup.

			// Nothing to commit.

			return repositoryLocations;
		}

		public async Task<RepositoryLocationsPair> CreateNew_ProgramAsService_ConsoleRepository(
			string owner,
			string endeavorName,
			string description,
			bool isPrivate,
			ILogger logger)
		{
			/// Run.
			logger.LogInformation($"Creating program-as-service console repository '{endeavorName}'...");

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
			var repositorySetup = Instances.RepositoryOperator.SetupRepository(
				repositoryLocations.LocalDirectoryPath,
				logger);

			/// Create - Solution.
			var unadjustedSolutionName = Instances.SolutionNameOperator.GetUnadjustedSolutionName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);
			var solutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedSolutionName, isPrivate);

			var solutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
                repositorySetup.SourceDirectoryPath,
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
			Instances.ProjectOperator.SetupProject_Console(
				projectFilePath,
				projectDescription,
				projectName,
				projectNamespaceName);

			Instances.ProjectOperator.PostSetupProject_ProgramAsService(
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

		public async Task<RepositoryLocationsPair> CreateNew_ConsoleRepository(
			string owner,
			string endeavorName,
			string description,
			bool isPrivate,
			ILogger logger)
		{
			/// Run.
			logger.LogInformation($"Creating console repository '{endeavorName}'...");

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
			var repositorySetup = Instances.RepositoryOperator.SetupRepository(
				repositoryLocations.LocalDirectoryPath,
				logger);

			/// Create - Solution.
			var unadjustedSolutionName = Instances.SolutionNameOperator.GetUnadjustedSolutionName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);
			var solutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedSolutionName, isPrivate);

			var solutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
                repositorySetup.SourceDirectoryPath,
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
			Instances.ProjectOperator.SetupProject_Console(
				projectFilePath,
				projectDescription,
				projectName,
				projectNamespaceName);

			// Add project to solution.
			Instances.SolutionFileOperator.AddProject(
				solutionFilePath,
				projectFilePath);

			// Perform initial commit.
			Instances.RepositoryOperator.PerformInitialCommit(
				repositoryLocations.LocalDirectoryPath,
				logger);

			return repositoryLocations;
		}

		/// <summary>
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

			/// Create - Repository.
			// As of now, we can assume the repository does not exist.
			var repositoryLocations = await Instances.RepositoryOperator.CreateNew_NonIdempotent(
				repositorySpecification,
				logger);

			// Setup repository.
			var repositorySetup = Instances.RepositoryOperator.SetupRepository(
				repositoryLocations.LocalDirectoryPath,
				logger);

			/// Create - Solution.
			// Now create the solution and project.
			var unadjustedSolutionName = Instances.SolutionNameOperator.GetUnadjustedSolutionName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);
			var solutionName = Instances.SolutionNameOperator.AdjustSolutionName_ForPrivacy(unadjustedSolutionName, isPrivate);

			var solutionFilePath = Instances.SolutionOperator.Create_Solution_SourceDirectoryPath(
                repositorySetup.SourceDirectoryPath,
				solutionName,
				logger);

			// Create - project.
			// Project name is just the unadjusted repository name. No adjustments needed.
			var projectName = Instances.ProjectNameOperator.GetProjectName_FromUnadjustedLibraryName(libraryDescriptors.UnadjustedName);

			// Script project description is just the library description.
			var projectDescription = Instances.ProjectOperator.Get_ProjectDescription_FromLibraryDescription(libraryDescriptors.Description);

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
                repositorySetup.SourceDirectoryPath,
				constructionSolutionName,
				logger);

			// Construction project.
			// Project name is just the unadjusted repository name. No adjustments needed.
			var constructionProjectName = Instances.ProjectNameOperator.GetConstructionProjectName(projectName);

            // Script project description is just the library description.
            // Obsolete("See R5T.O0007.IProjectDescriptionOperations.Get_ConstructionProjectDescription()")
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
			Instances.ProjectFileOperator.AddProjectReference_Idempotent_Synchronous(
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