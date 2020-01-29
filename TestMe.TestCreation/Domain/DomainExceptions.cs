using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal static class DomainExceptions
    {
        public static string Limit_of_questions_catalogs_has_been_reached_thus_you_cannot_add_a_new_questions_catalog = "Limit of questions catalogs has been reached, thus you cannot add a new questions catalog.";
        public static string Catalog_not_found = "Catalog not found";
        public static string Question_can_not_be_moved_to_catalog_that_you_do_not_own = "Question can not be moved to catalog that you do not own";
        public static string Limit_of_questions_in_the_current_catalog_has_been_reached_thus_you_cannot_add_a_new_question = "Limit of questions in the current catalog has been reached, thus you cannot add a new question.";
    }
}
