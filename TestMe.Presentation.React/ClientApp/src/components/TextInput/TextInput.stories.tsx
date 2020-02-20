import React from 'react';
import { action } from '@storybook/addon-actions';
import { TextInput } from './TextInput';

export default {
    title: 'Components/TextInput',
    component: TextInput,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onInputChange: action('onInputChange')   
};

export const Default = () =>
{
    return (
        <TextInput
            path="name"
            placeholder="placeholder"
            value="lorem dolorem"
            label="label"
            validationError=""
            conflictError=""
            onInputChange={actionsData.onInputChange}
        />
    );
};

export const WithError = () => {
    return (
        <TextInput
            path="name"
            placeholder="placeholder"
            value="lorem dolorem"
            label="label"
            validationError="WithError"
            conflictError=""
            onInputChange={actionsData.onInputChange}
        />
    );
};
