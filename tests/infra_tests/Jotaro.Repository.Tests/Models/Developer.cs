namespace Jotaro.Repository.Tests.Models
{
    public class Developer : Employee
    {
        // Testing embedded documents.
        public ProgrammingLanguageSkill[] Skills { get; set; }
    }
}
