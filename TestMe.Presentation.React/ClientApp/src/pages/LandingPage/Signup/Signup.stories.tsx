import React from 'react';
import { action } from '@storybook/addon-actions';
import { Signup } from './Signup';
import { CreateUserDTO } from '../../../autoapi/services/UsersService';
import { withKnobs, text, boolean, number } from "@storybook/addon-knobs";

export default {
    title: 'LandingPage/Signup',
    component: Signup,
    excludeStories: /.*Data$/,
    decorators: [withKnobs]
};

export const actionsData = {
    onSubmit: action('onSubmit'),
    onValidate: action('onValidate'),   
};

export const initialData: CreateUserDTO = {
    emailAddress: "jon.doe@eod.noj",
    name: "Jon Doe",
    password : "verySecretPassword",
};



export const Default = () => (
    <Signup {...actionsData} initialValues={initialData} userWasCreated={boolean("userWasCreated", false)}/>
);
