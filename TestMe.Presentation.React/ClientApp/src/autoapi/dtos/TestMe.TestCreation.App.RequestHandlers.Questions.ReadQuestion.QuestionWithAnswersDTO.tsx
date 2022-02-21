﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/

import { AnswerDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion.AnswerDTO";

export class QuestionWithAnswersDTO
{
    questionId: number;
    content: string;
    answers: AnswerDTO[];
    concurrencyToken: number;

    //eslint-disable-next-line
    constructor()
    {
    
        this.questionId = 0;
        this.content = "";
        this.answers = [];
        this.concurrencyToken = 0;
    }
}