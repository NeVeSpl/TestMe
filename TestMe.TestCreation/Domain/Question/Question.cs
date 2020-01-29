using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{

    internal sealed class Question
    {
        private readonly List<Answer> _answers = new List<Answer>();


        public long QuestionId
        {
            get;
            private set;
        }
        public string Content { get;  set; }
        public IReadOnlyList<Answer> Answers
        {
            get => _answers;
        }
        public long CatalogId { get; private set; }
        public long OwnerId { get; private set; }
        public bool IsDeleted { get; private set; }
        public uint ConcurrencyToken { get; set; }


        private Question(string content)
        {
            Content = content;
        }


        public static Question Create(string content,  long ownerId)
        {
            return new Question(content) { OwnerId = ownerId };
        }


        public void Delete()
        {           
            IsDeleted = true;
        }    
        public Answer AddAnswer(string content, bool isCorrect)
        {
            var answer = Answer.Create(content, isCorrect);           
            _answers.Add(answer);
            return answer;
        }
        public void DeleteAnswer(Answer answer)
        {
            _answers.Remove(answer);
        }
    }
}
