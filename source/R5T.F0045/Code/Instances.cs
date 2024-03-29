using System;


namespace R5T.F0045
{
    public static class Instances
    {
        public static F0043.ILibraryDescriptionOperator LibraryDescriptionOperator { get; } = F0043.LibraryDescriptionOperator.Instance;
        public static F0043.ILibraryNameOperator LibraryNameOperator { get; } = F0043.LibraryNameOperator.Instance;
        public static F0020.IProjectFileOperator ProjectFileOperator { get; } = F0020.ProjectFileOperator.Instance;
        public static F0040.F000.IProjectNamespacesOperator ProjectNamespacesOperator { get; } = F0040.F000.ProjectNamespacesOperator.Instance;
        public static F0051.IProjectOperator ProjectOperator { get; } = F0051.ProjectOperator.Instance;
        public static F0055.IProjectNameOperator ProjectNameOperator { get; } = F0055.ProjectNameOperator.Instance;
        public static F0056.IProjectOperations ProjectOperations { get; } = F0056.ProjectOperations.Instance;
        public static F0046.IRepositoryDescriptionOperator RepositoryDescriptionOperator { get; } = F0046.RepositoryDescriptionOperator.Instance;
        public static F0046.IRepositoryNameOperator RepositoryNameOperator { get; } = F0046.RepositoryNameOperator.Instance;
        public static F0056.IRepositoryOperator RepositoryOperator { get; } = F0056.RepositoryOperator.Instance;
        public static F0063.ISolutionFileOperator SolutionFileOperator { get; } = F0063.SolutionFileOperator.Instance;
        public static F0048.ISolutionNameOperator SolutionNameOperator { get; } = F0048.SolutionNameOperator.Instance;
        public static F0049.ISolutionOperator SolutionOperator { get; } = F0049.SolutionOperator.Instance;
        public static F0063.ISolutionOperations SolutionOperations { get; } = F0063.SolutionOperations.Instance;
    }
}