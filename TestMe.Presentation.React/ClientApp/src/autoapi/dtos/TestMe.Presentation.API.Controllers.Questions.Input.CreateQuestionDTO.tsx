﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/

import { CreateAnswerDTO } from "../dtos/TestMe.Presentation.API.Controllers.Questions.Input.CreateAnswerDTO";

export class CreateQuestionDTO 
{ 
    content: string;
    answers: CreateAnswerDTO[];
    catalogId: number;

    constructor()
    {  
            
         this.content = "";
         this.answers = [];
         this.catalogId = 0;
    }
}