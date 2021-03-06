﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/

import { UpdateAnswerDTO } from "../dtos/TestMe.Presentation.API.Controllers.Questions.Input.UpdateAnswerDTO";

export class UpdateQuestionDTO
{
    content: string;
    answers: UpdateAnswerDTO[];
    catalogId: number;
    concurrencyToken: number | null;

    //eslint-disable-next-line
    constructor()
    {
    
        this.content = "";
        this.answers = [];
        this.catalogId = 0;
        this.concurrencyToken = null;
    }
}