﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/

import { QuestionItemDTO } from "../dtos/TestMe.TestCreation.App.Tests.Output.QuestionItemDTO";
import { TestHeaderDTO } from "../dtos/TestMe.TestCreation.App.Tests.Output.TestHeaderDTO";

export class TestDTO extends TestHeaderDTO
{ 
    questionItems: QuestionItemDTO[];

    constructor()
    {  
         super();   
         this.questionItems = [];
    }
}