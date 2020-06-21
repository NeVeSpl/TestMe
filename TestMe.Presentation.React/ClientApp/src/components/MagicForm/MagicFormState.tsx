import { MagicDict } from './MagicDict';

export class MagicFormState<T>
{
    data: T;
    validationErrors: MagicDict;
    conflictErrors: MagicDict;
    hasValidationErrors: boolean;
    isFormItemTouched: string[];



    constructor(dataFactory: new () => T)
    {
        this.validationErrors = {};
        this.conflictErrors = {};
        this.hasValidationErrors = false;     
        this.data = new dataFactory();
        this.isFormItemTouched = [];
    }
}