import * as React from 'react';
import { FormikProps, Formik } from 'formik';

interface TextInputProp
{
    label?: string;
    name: string;  
    formik: FormikProps<any>;
    placeholder?: string;
}

export class TextInput extends React.Component<TextInputProp>
{
    

    render()
    {       
        return (
            <>
                {this.props.label &&
                    <label htmlFor={this.props.name}>{this.props.label}</label>
                }
                <div>
                    <input
                        type="text"
                        className="form-control"
                        name={this.props.name}
                        value={this.props.formik.values[this.props.name]}
                        onChange={this.props.formik.handleChange}
                        onBlur={this.props.formik.handleBlur}
                        placeholder={this.props.placeholder} />
                    <small className="form-text text-muted">{this.props.formik.errors[this.props.name]}</small>
                </div>
            </>
        );
    }
}

