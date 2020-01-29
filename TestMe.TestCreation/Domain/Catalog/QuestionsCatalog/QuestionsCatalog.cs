using System;
using System.Collections.Generic;
using System.Text;
using TestMe.BuildingBlocks.Domain;

namespace TestMe.TestCreation.Domain
{
    internal sealed class QuestionsCatalog : Catalog
    {
        private readonly List<Question> _questions = new List<Question>();

        public int QuestionsCount { get; private set; } = 0;
        public IReadOnlyList<Question> Questions
        {
            get => _questions;           
        }


        private QuestionsCatalog(string name, long ownerId) : base(name, ownerId)
        {
        }


        public static QuestionsCatalog Create(string name, long ownerId)
        {
            return new QuestionsCatalog(name,  ownerId);
        }

        public override void Delete()
        {
            base.Delete();
            foreach (Question question in _questions)
            {
                question.Delete();
            }
        }

        public void AddQuestion(Question question, IAddQuestionPolicy policy)
        {
            if (policy.CanAddQuestion(QuestionsCount))
            {
                _questions.Add(question);
                QuestionsCount++;
            }
            else
            {
                throw new DomainException(DomainExceptions.Limit_of_questions_in_the_current_catalog_has_been_reached_thus_you_cannot_add_a_new_question);
            }
        }
        /// <summary>
        /// RemoveQuestion only removes question from catalog, it does not delete question.
        /// </summary>       
        public void RemoveQuestion(Question question)
        {
            if(_questions.Remove(question))
            {
                QuestionsCount--;
            }
        }
        /// <summary>
        /// DeleteQuestion removes question from catalog and also deletes it.
        /// </summary>     
        public void DeleteQuestion(Question question)
        {
            QuestionsCount--;
            question.Delete();            
        }
    }
}
