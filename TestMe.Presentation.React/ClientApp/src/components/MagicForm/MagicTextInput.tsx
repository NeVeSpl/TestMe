import * as React from 'react';
import { MagicFormContext } from './MagicForm';
import { TextInput } from '../TextInput/TextInput';


interface MagicTextInputProp
{
    label?: string;
    path: string;   
    value: string; 
    placeholder?: string;
}

export class MagicTextInput extends React.Component<MagicTextInputProp>
{
    static contextType = MagicFormContext;

    shouldComponentUpdate(nextProps: MagicTextInputProp)
    {       
        return true;
    }

    render()
    {
        return (
            <TextInput
                label={this.props.label}
                path={this.props.path}
                value={this.props.value}    
                placeholder={this.props.placeholder || ""}                        
                onInputChange={this.context.onInputChange}                 
                validationError={this.context.validationErrors[this.props.path] || ""}
                conflictError={this.context.conflictErrors[this.props.path] || ""}
            />
        );
    }
}