using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class Test
    {   
        private readonly List<QuestionItem> _questions = new List<QuestionItem>();


        public long TestId { get; private set; }
        public string Title { get; set; }
        public IReadOnlyList<QuestionItem> Questions
        {
            get => _questions;
        }
        public long CatalogId { get; private set; }
        public long OwnerId { get; private set; }
        public bool IsDeleted { get; private set; }


        private Test(string title)
        {
            Title = title;
        }


        public static Test Create(long ownerId, string title)
        {
            return new Test(title) { OwnerId = ownerId };
        }


        public void Delete()
        {
            IsDeleted = true;
        }      
        public QuestionItem AddQuestion(Question question)
        {
            var item = QuestionItem.Create(question);
            _questions.Add(item);
            return item;
        }
        public void RemoveQuestion(QuestionItem questionItem)
        {
            _questions.Remove(questionItem);
        }
    }
}
