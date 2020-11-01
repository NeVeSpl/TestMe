namespace TestMe.TestCreation.App.RequestHandlers.Questions.UpdateQuestion
{
    public sealed class UpdateAnswer
    {
        public long AnswerId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}