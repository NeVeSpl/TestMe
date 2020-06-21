import { Action } from 'redux';
import { MagicForm, MagicFormState } from '..';




export function magicFormReducer<T>(state :  MagicFormState<T>, action: Action): MagicFormState<T>
{
    switch (action.type)
    {
        case InputHasChanged.Type:
            const inputHasChanged = action as InputHasChanged;
            state.isFormItemTouched.push(inputHasChanged.event.target.name);
            return { ...state, data: MagicForm.ApplyEventTo(inputHasChanged.event, state.data) };        
    }
    return state;
}



export class InputHasChanged
{
    static Type = Symbol('InputHasChanged');

    constructor(public event: React.ChangeEvent<HTMLInputElement>, public formId : string, public type = InputHasChanged.Type) { }
}