import { Thunk } from '../../../../redux.base';
import { QuestionDTO } from '../../../../autoapi/services/QuestionsService';









export class QuestionUpdated
{
    static Type = Symbol('QuestionUpdated');

    constructor(public question: QuestionDTO, public type = QuestionUpdated.Type) { }
}

export class QuestionCreated
{
    static Type = Symbol('QuestionCreated');

    constructor(public question: QuestionDTO, public type = QuestionCreated.Type) { }
}