import * as React from 'react';
import { FormikProps } from 'formik';
import { TextInput } from '../TextInput/TextInput';
import { ApiError } from '../../autoapi/base';

interface FormikTextInputProp
{
    label?: string;
    name: string;  
    formik: FormikProps<any>;
    placeholder?: string;
    type?: string;
    apiError?: ApiError | undefined;
}

export class FormikTextInput extends React.Component<FormikTextInputProp>
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
                validationError={this.formatError(this.props)}
                conflictError=""
                type={this.props.type}
                />
        );
    }


    formatError(props: FormikTextInputProp) : string
    {
        return (this.props.apiError?.errors[this.props.name] || "") + (this.props.formik.errors[this.props.name]?.toString() || "");
    }
}

