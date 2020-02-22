import * as React from 'react';

interface TextInputProp
{
    label?: string;
    path: string;   
    value: string;
    onInputChange: any
    placeholder: string;
    validationError: string;   
    conflictError: string;
    type?: string;
}

function TextInputInternal(props: TextInputProp)
{
    let isInvalid: boolean = false;
    let inputClassName = 'form-control';

    if (props.validationError !== "")
    {        
        isInvalid = true;
        inputClassName += " is-invalid";
    }
    return (
        <>
            {props.label &&
                <label htmlFor={props.path}>{props.label}</label>
            }
            <input type={props.type ?? "text"} className={inputClassName} name={props.path} value={props.value} onChange={props.onInputChange} placeholder={props.placeholder} />
            {isInvalid &&
                <div className="invalid-feedback">
                {props.validationError}
                </div>
            }  

            {props.conflictError &&
                <div className="text-warning">
                    Conflicted value: {props.conflictError}
                </div>
            } 
        </>
       );
}

export const TextInput = React.memo<TextInputProp>(TextInputInternal);