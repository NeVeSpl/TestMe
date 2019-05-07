import * as React from 'react';
import { ObjectFormItem } from './ObjectFormItem';
import { ObjectForm } from './ObjectForm';

interface TextInputProp
{
    label?: string;
    name: string;
    data: ObjectFormItem;
    onInputChange: any
    placeholder?: string;
}

export class TextInput extends React.Component<TextInputProp>
{
    handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) =>
    {
        const target = event.target;
        const value = target.value;
        this.props.data.isTouched = true;
        this.props.data.newValue = value;
        this.props.onInputChange(event);
    }

    shouldComponentUpdate(nextProps: TextInputProp)
    {
        return !ObjectForm.areItemsEqual(this.props.data, nextProps.data);
    }

    render()
    {       
        return (
            <>
                {this.props.label &&
                    <label htmlFor={this.props.name}>{this.props.label}</label>
                }
                <div>
                    <input type="text" className="form-control" name={this.props.name} value={this.props.data.value} onChange={this.handleInputChange} placeholder={this.props.placeholder} />
                    <small className="form-text text-muted">{this.props.data.error}</small>
                </div>
            </>
        );
    }
}

