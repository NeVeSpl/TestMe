using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class Test
    {
        public const int TitleMaxLength = 2048;

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


        private Test()
        {

        }


        public static Test Create(long ownerId, string title)
        {
            return new Test() {  OwnerId = ownerId, Title = title };
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
