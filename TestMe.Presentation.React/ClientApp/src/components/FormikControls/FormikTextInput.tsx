import * as React from 'react';
import { FormikProps, Formik } from 'formik';
import { TextInput } from '../TextInput/TextInput';

interface TextInputProp
{
    label?: string;
    name: string;  
    formik: FormikProps<any>;
    placeholder?: string;
}

export class FormikTextInput extends React.Component<TextInputProp>
{
    

    render()
    {       
        return (          
                <TextInput
                    label={this.props.label}
                    path={this.props.name}
                    value={this.props.formik.values[this.props.name]}
                    placeholder={this.props.placeholder || ""}
                    onInputChange={this.props.formik.handleChange}                
                    validationError={this.props.formik.errors[this.props.name]?.toString() || ""}
                    conflictError= ""
                />
        );
    }
}

