using System;

using R5T.F0020;
using R5T.F0024;
using R5T.F0040.F000;
using R5T.F0042;
using R5T.F0043;
using R5T.F0046;
using R5T.F0048;
using R5T.F0049;
using R5T.F0051;
using R5T.F0055;
using R5T.F0056;
using R5T.F0063;


namespace R5T.F0045
{
    public static class Instances
    {
        public static ILibraryDescriptionOperator LibraryDescriptionOperator { get; } = F0043.LibraryDescriptionOperator.Instance;
        public static ILibraryNameOperator LibraryNameOperator { get; } = F0043.LibraryNameOperator.Instance;
        public static IProjectFileOperator ProjectFileOperator { get; } = F0020.ProjectFileOperator.Instance;
        public static IProjectNamespacesOperator ProjectNamespacesOperator { get; } = F0040.F000.ProjectNamespacesOperator.Instance;
        public static F0051.IProjectOperator ProjectOperator { get; } = F0051.ProjectOperator.Instance;
        public static IProjectNameOperator ProjectNameOperator { get; } = F0055.ProjectNameOperator.Instance;
        public static IProjectOperations ProjectOperations { get; } = F0056.ProjectOperations.Instance;
        public static IRepositoryDescriptionOperator RepositoryDescriptionOperator { get; } = F0046.RepositoryDescriptionOperator.Instance;
        public static IRepositoryNameOperator RepositoryNameOperator { get; } = F0046.RepositoryNameOperator.Instance;
        public static F0056.IRepositoryOperator RepositoryOperator { get; } = F0056.RepositoryOperator.Instance;
        public static ISolutionFileOperator SolutionFileOperator { get; } = F0024.SolutionFileOperator.Instance;
        public static ISolutionNameOperator SolutionNameOperator { get; } = F0048.SolutionNameOperator.Instance;
        public static ISolutionOperator SolutionOperator { get; } = F0049.SolutionOperator.Instance;
        public static ISolutionOperations SolutionOperations { get; } = F0063.SolutionOperations.Instance;
    }
}